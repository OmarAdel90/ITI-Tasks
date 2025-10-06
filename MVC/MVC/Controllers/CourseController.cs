using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class CourseController : Controller
    {
        private readonly CourseRepository repo;
        public CourseController(CourseRepository repo) => this.repo = repo;
        public IActionResult Index(string searchString)
        {
            var courses = repo.ReturnCourses();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(searchString))
            {
                courses = courses.Where(c => c.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewBag.CurrentFilter = searchString;
            }

            return View(courses);
        }
        public IActionResult Details(int id)
        {
            var course = repo.ReturnDetails(id);
            return View(course);
        }
        public IActionResult Create()
        {
            ViewBag.Departments = repo.GetAllDepartments();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Course course)
        {
            repo.AddCourse(course);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var course = repo.FindCourse(id);
            ViewBag.Departments = repo.GetAllDepartments();
            return View(course);
        }
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            repo.UpdateCourse(course);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                repo.DeleteCourse(id);
                TempData["Success"] = "Course deleted successfully!";
            }
            catch(InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                TempData.Keep();
            }
            return RedirectToAction("Index");
        }
    }
}
