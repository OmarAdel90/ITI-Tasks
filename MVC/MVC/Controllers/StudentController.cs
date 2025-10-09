using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using System.Security.Claims;

namespace MVC.Controllers
{
    [Authorize] // Require authentication for all actions
    public class StudentController : Controller
    {
        private readonly StudentRepository repo;

        public StudentController(StudentRepository repo) => this.repo = repo;

        // Admin, HR can view all students, Student can only view themselves
        [Authorize(Roles = "Admin,HR,Student")]
        public IActionResult Index(string searchString)
        {
            var students = repo.GetStudentsBySearch(searchString);

            // If user is a Student, filter to show only themselves
            if (User.IsInRole("Student") && !User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                var studentId = GetCurrentStudentId();
                if (studentId > 0)
                {
                    students = students.Where(s => s.Id == studentId).ToList();
                }
                else
                {
                    // No student record found, show empty list
                    students = new List<Student>();
                }
            }

            ViewBag.CurrentFilter = searchString;
            return View(students);
        }

        // Admin, HR, Student (their own details) can view details
        [Authorize(Roles = "Admin,HR,Student")]
        public IActionResult Details(int id)
        {
            var student = repo.ReturnDetails(id);
            if (student == null)
            {
                return NotFound();
            }

            // Authorization check for Student
            if (User.IsInRole("Student") && !User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                var currentStudentId = GetCurrentStudentId();
                if (student.Id != currentStudentId)
                {
                    return RedirectToAction("AccessDenied", "Account");
                }
            }

            return View(student);
        }

        // Only Admin and HR can create students
        [Authorize(Roles = "Admin,HR")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = repo.ReturnDepartments();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            // Remove navigation properties from ModelState validation
            ModelState.Remove("Department");
            ModelState.Remove("CourseStudents");
            ModelState.Remove("User");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = repo.ReturnDepartments();
                return View(student);
            }

            repo.CreateStudent(student);
            TempData["Success"] = "Student created successfully!";
            return RedirectToAction("Index");
        }

        // Admin, HR can edit any student, Student can only edit themselves
        [Authorize(Roles = "Admin,HR,Student")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var student = repo.ReturnDetails(id);
            if (student == null)
            {
                return NotFound();
            }

            // Authorization check for Student
            if (User.IsInRole("Student") && !User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                var currentStudentId = GetCurrentStudentId();
                if (student.Id != currentStudentId)
                {
                    return RedirectToAction("AccessDenied", "Account");
                }
            }

            ViewBag.Departments = repo.ReturnDepartments();
            return View("Edit", student);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,HR,Student")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Student student)
        {
            // Remove navigation properties from ModelState validation
            ModelState.Remove("Department");
            ModelState.Remove("CourseStudents");
            ModelState.Remove("User");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = repo.ReturnDepartments();
                return View(student);
            }

            // Authorization check for Student
            if (User.IsInRole("Student") && !User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                var currentStudentId = GetCurrentStudentId();
                if (student.Id != currentStudentId)
                {
                    return RedirectToAction("AccessDenied", "Account");
                }
            }

            repo.EditStudent(student);
            TempData["Success"] = User.IsInRole("Student")
                ? "Your profile has been updated successfully!"
                : "Student updated successfully!";
            return RedirectToAction("Index");
        }

        // Admin, HR can delete any student, Student can only delete themselves
        [HttpPost]
        [Authorize(Roles = "Admin,HR,Student")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Authorization check for Student
                if (User.IsInRole("Student") && !User.IsInRole("Admin") && !User.IsInRole("HR"))
                {
                    var currentStudentId = GetCurrentStudentId();
                    if (id != currentStudentId)
                    {
                        TempData["Error"] = "You can only delete your own profile.";
                        return RedirectToAction("Index");
                    }
                }

                await repo.DeleteStudent(id);
                TempData["Success"] = "Student deleted successfully!";
            }
            catch (InvalidOperationException ex)
            {
                // This will catch the friendly error message from the repository
                TempData["Error"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        // Helper method that returns the Student table ID
        private int GetCurrentStudentId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                return 0;

            // Find student by UserId (the link to AspNetUsers)
            var student = repo.GetStudentByUserId(userIdClaim);
            return student?.Id ?? 0;
        }
    }
}