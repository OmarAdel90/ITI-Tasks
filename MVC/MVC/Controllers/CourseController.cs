using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using System.Security.Claims;

namespace MVC.Controllers
{
    [Authorize] // Require authentication for all actions
    public class CourseController : Controller
    {
        private readonly CourseRepository repo;
        private readonly AppDbContext _context;

        public CourseController(CourseRepository repo, AppDbContext context)
        {
            this.repo = repo;
            _context = context;
        }

        // Admin, HR, Instructor, Student can view courses
        [Authorize(Roles = "Admin,HR,Instructor,Student")]
        public IActionResult Index(string searchString)
        {
            var courses = repo.ReturnCourses();

            // If user is an Instructor, filter to show only their courses
            if (User.IsInRole("Instructor") && !User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                var instructorId = GetCurrentInstructorId();
                if (instructorId > 0)
                {
                    courses = repo.GetCoursesByInstructor(instructorId);
                }
                else
                {
                    // Instructor has no profile or no courses assigned
                    courses = new List<Course>();
                }
            }

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(c => c.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewBag.CurrentFilter = searchString;
            }

            return View(courses);
        }

        // Allow students to view any course details (they might want to enroll)
        [Authorize(Roles = "Admin,HR,Instructor,Student")]
        public IActionResult Details(int id)
        {
            var course = repo.ReturnDetails(id);

            if (course == null)
            {
                return NotFound();
            }

            // Authorization checks for Instructor only (not Admin/HR)
            if (User.IsInRole("Instructor") && !User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                var instructorId = GetCurrentInstructorId();
                if (instructorId > 0 && !repo.IsInstructorTeachingCourse(instructorId, id))
                {
                    TempData["Error"] = "You can only view courses you are teaching.";
                    return RedirectToAction("AccessDenied", "Account");
                }
            }

            return View(course);
        }

        // Only Admin can create courses
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = repo.GetAllDepartments();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course course)
        {
            // Remove navigation properties from ModelState validation
            ModelState.Remove("Department");
            ModelState.Remove("Instructors");
            ModelState.Remove("CourseStudents");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = repo.GetAllDepartments();
                return View(course);
            }

            // Custom validation: MinimumDegree should be less than Degree
            if (course.MinimumDegree >= course.Degree)
            {
                ModelState.AddModelError("MinimumDegree", "Minimum Degree must be less than the total Degree");
                ViewBag.Departments = repo.GetAllDepartments();
                return View(course);
            }

            repo.AddCourse(course);
            TempData["Success"] = "Course created successfully!";
            return RedirectToAction("Index");
        }

        // Only Admin can edit courses
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var course = repo.FindCourse(id);
            if (course == null)
            {
                return NotFound();
            }

            ViewBag.Departments = repo.GetAllDepartments();
            return View(course);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Course course)
        {
            // Remove navigation properties from ModelState validation
            ModelState.Remove("Department");
            ModelState.Remove("Instructors");
            ModelState.Remove("CourseStudents");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = repo.GetAllDepartments();
                return View(course);
            }

            // Custom validation: MinimumDegree should be less than Degree
            if (course.MinimumDegree >= course.Degree)
            {
                ModelState.AddModelError("MinimumDegree", "Minimum Degree must be less than the total Degree");
                ViewBag.Departments = repo.GetAllDepartments();
                return View(course);
            }

            repo.UpdateCourse(course);
            TempData["Success"] = "Course updated successfully!";
            return RedirectToAction("Index");
        }

        // Only Admin can delete courses
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                repo.DeleteCourse(id);
                TempData["Success"] = "Course deleted successfully!";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        // Get the instructor ID for the current logged-in user
        private int GetCurrentInstructorId()
        {
            // Get the ApplicationUser ID from claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return 0;
            }

            // Find the ApplicationUser and get their linked InstructorId
            var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id.ToString() == userIdClaim);

            if (user?.InstructorId != null)
            {
                return user.InstructorId.Value;
            }

            return 0;
        }
    }
}