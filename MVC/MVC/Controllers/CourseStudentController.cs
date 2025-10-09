using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;
using MVC.Repositories;
using MVC.Services;

namespace MVC.Controllers
{
    [Authorize]
    public class CourseStudentController : Controller
    {
        private readonly CourseStudentRepository _repo;
        private readonly IAuthService _authService;

        public CourseStudentController(CourseStudentRepository repo, IAuthService authService)
        {
            _repo = repo;
            _authService = authService;
        }

        public IActionResult Index(string searchString)
        {
            var userRole = _authService.GetCurrentUserRole(HttpContext);

            if (userRole == Roles.Student)
            {
                var studentId = _authService.GetCurrentStudentId(HttpContext);
                if (studentId == null)
                {
                    TempData["Error"] = "Student profile not found.";
                    return View(new List<CourseStudents>());
                }
                return View(_repo.GetEnrollmentsByStudentId(studentId.Value));
            }
            else if (userRole == Roles.Instructor)
            {
                var instructorId = _authService.GetCurrentInstructorId(HttpContext);
                if (instructorId == null)
                {
                    TempData["Error"] = "Instructor profile not found.";
                    return View(new List<CourseStudents>());
                }
                return View(_repo.GetEnrollmentsByInstructorId(instructorId.Value));
            }
            else
            {
                // Admin and HR can see all enrollments
                return View(_repo.GetEnrollmentsBySearch(searchString));
            }
        }

        [Authorize(Roles = Roles.Student)]
        public IActionResult Create()
        {
            var studentId = _authService.GetCurrentStudentId(HttpContext);
            if (studentId == null)
            {
                TempData["Error"] = "Student profile not found.";
                return RedirectToAction("Index");
            }

            // Use ReturnAvailableCourses instead of ReturnCourses
            ViewBag.Courses = _repo.ReturnAvailableCourses(studentId.Value);
            ViewBag.StudentId = studentId;
            ViewBag.StudentName = _repo.GetStudentName(studentId.Value);

            return View();
        }

        [HttpPost]
        [Authorize(Roles = Roles.Student)]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CourseStudents courseStudent)
        {
            var studentId = _authService.GetCurrentStudentId(HttpContext);
            if (studentId == null)
            {
                TempData["Error"] = "Student profile not found.";
                return RedirectToAction("Index");
            }

            // Set the student ID before validation
            courseStudent.StdId = studentId.Value;

            // Set initial degree to 0
            if (courseStudent.Degree == 0)
            {
                courseStudent.Degree = 0;
            }

            // Remove validation errors for StdId since we're setting it manually
            ModelState.Remove("StdId");
            ModelState.Remove("Student");
            ModelState.Remove("Course");

            if (!ModelState.IsValid)
            {
                ViewBag.Courses = _repo.ReturnAvailableCourses(studentId.Value);
                ViewBag.StudentId = studentId;
                ViewBag.StudentName = _repo.GetStudentName(studentId.Value);
                return View(courseStudent);
            }

            if (_repo.EnrollmentExists(studentId.Value, courseStudent.CrsId))
            {
                ModelState.AddModelError("", "You are already enrolled in this course.");
                ViewBag.Courses = _repo.ReturnAvailableCourses(studentId.Value);
                ViewBag.StudentId = studentId;
                ViewBag.StudentName = _repo.GetStudentName(studentId.Value);
                return View(courseStudent);
            }

            _repo.CreateEnrollment(courseStudent);
            TempData["SuccessMessage"] = "Successfully enrolled in the course!";
            return RedirectToAction("Index");
        }

        [Authorize(Roles = $"{Roles.Admin},{Roles.HR}")]
        public IActionResult Edit(int id)
        {
            var enrollment = _repo.FindEnrollment(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            ViewBag.Students = _repo.ReturnStudents();
            ViewBag.Courses = _repo.ReturnCourses();
            return View(enrollment);
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Admin},{Roles.HR}")]
        public IActionResult Edit(CourseStudents courseStudent)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Students = _repo.ReturnStudents();
                ViewBag.Courses = _repo.ReturnCourses();
                return View(courseStudent);
            }

            _repo.EditEnrollment(courseStudent);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var enrollment = _repo.ReturnDetails(id);
            if (enrollment == null)
            {
                return NotFound();
            }

            // Authorization checks
            var userRole = _authService.GetCurrentUserRole(HttpContext);
            if (userRole == Roles.Student)
            {
                var studentId = _authService.GetCurrentStudentId(HttpContext);
                if (enrollment.StdId != studentId)
                {
                    return RedirectToAction("AccessDenied", "Account");
                }
            }
            else if (userRole == Roles.Instructor)
            {
                var instructorId = _authService.GetCurrentInstructorId(HttpContext);
                if (!_repo.IsCourseTaughtByInstructor(enrollment.CrsId, instructorId.Value))
                {
                    return RedirectToAction("AccessDenied", "Account");
                }
            }

            return View(enrollment);
        }

        [HttpPost]
        [Authorize(Roles = $"{Roles.Admin},{Roles.HR}")]
        public IActionResult Delete(int id)
        {
            _repo.DeleteEnrollment(id);
            return RedirectToAction("Index");
        }
    }
}