using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext context;
        public CourseController(AppDbContext context) => this.context = context;
        public IActionResult Index()
        {
            List<Course> courses = context.Courses.ToList();
            return View("Index", courses);
        }
        public IActionResult Details(int id)
        {
            var course = context.Courses.Find(id);
            return View("Details", course);
        }
        public IActionResult Create()
        {
            ViewBag.Departments = context.Departments.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Course course)
        {
            context.Add(course);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var course = context.Courses.Find(id);
            ViewBag.Departments = context.Departments.ToList();
            return View("Edit", course);
        }
        [HttpPost]
        public IActionResult Edit(Course course)
        {
            context.Update(course);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var course = context.Courses.Find(id);
            context.Remove(course);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
