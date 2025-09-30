using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class InstructorController : Controller
    {
        private readonly AppDbContext context;
        public InstructorController(AppDbContext context) => this.context = context;
        public IActionResult Index()
        {
            var instructors = context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .ToList();
            return View(instructors);
        }
        public IActionResult Create(int id)
        {
            ViewBag.Departments = context.Departments.ToList();
            ViewBag.Courses = context.Courses.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Instructor instructor)
        {
            context.Add(instructor);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var instructor = context.Instructors.Find(id);
            ViewBag.Departments = context.Departments.ToList();
            ViewBag.Courses = context.Courses.ToList();
            return View(instructor);
        }
        [HttpPost]
        public IActionResult Edit(Instructor instructor)
        {
            context.Update(instructor);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var instructor = context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .FirstOrDefault(i => i.Id == id);
            return View(instructor);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var instructor = context.Instructors.Find(id);
            context.Remove(instructor);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
