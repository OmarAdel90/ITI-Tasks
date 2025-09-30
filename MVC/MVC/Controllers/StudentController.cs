using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly AppDbContext context;
        public StudentController(AppDbContext context) => this.context = context;
        public IActionResult Index()
        {
            var students = context.Students.Include(s => s.Department).ToList();
            return View("Index", students);
        }
        public IActionResult Details(int id)
        {
            var student = context.Students
                .Include(s => s.Department)
                .Include(s => s.CourseStudents)
                    .ThenInclude(cs => cs.Course)
                .FirstOrDefault(s => s.Id == id);
            return View("Details", student);
        }
        public IActionResult Create()
        {
            ViewBag.Departments = context.Departments.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student student)
        {
            context.Add(student);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var student = context.Students.Find(id);
            ViewBag.Departments = context.Departments.ToList();
            return View("Edit", student);
        }
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            context.Update(student);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var student = context.Students.Find(id);
            context.Remove(student);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
