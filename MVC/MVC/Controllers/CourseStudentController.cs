using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class CourseStudentController : Controller
    {
        private readonly AppDbContext context;
        public CourseStudentController(AppDbContext context) => this.context = context;
        public IActionResult Index()
        {
            var enrollments = context.CourseStudents
                .Include(cs => cs.Student)
                .Include(cs => cs.Course)
                .ToList();
            return View(enrollments);
        }

        public IActionResult Create()
        {
            ViewBag.Students = context.Students.ToList();
            ViewBag.Courses = context.Courses.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(CourseStudents courseStudent)
        {
            context.Add(courseStudent);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var enrollment = context.CourseStudents.Find(id);
            ViewBag.Students = context.Students.ToList();
            ViewBag.Courses = context.Courses.ToList();
            return View(enrollment);
        }
        [HttpPost]
        public IActionResult Edit(CourseStudents courseStudent)
        {
            context.Update(courseStudent);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var enrollment = context.CourseStudents
                .Include(cs => cs.Student)
                .Include(cs => cs.Course)
                .FirstOrDefault(cs => cs.Id == id);
            return View(enrollment);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var enrollment = context.CourseStudents.Find(id);
            if (enrollment != null)
            {
                context.CourseStudents.Remove(enrollment);
                context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
