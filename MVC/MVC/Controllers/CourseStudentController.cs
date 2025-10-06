using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class CourseStudentController : Controller
    {
        private readonly CourseStudentRepository repo;
        public CourseStudentController(CourseStudentRepository repo) => this.repo = repo;
        public IActionResult Index()
        {
            var enrollments = repo.ReturnEnrollments();
            return View(enrollments);
        }

        public IActionResult Create()
        {
            ViewBag.Students = repo.ReturnStudents();
            ViewBag.Courses = repo.ReturnCourses();
            return View();
        }
        [HttpPost]
        public IActionResult Create(CourseStudents courseStudent)
        {
            repo.CreateEnrollment(courseStudent);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var enrollment = repo.FindEnrollment(id);
            ViewBag.Students = repo.ReturnStudents();
            ViewBag.Courses = repo.ReturnCourses();
            return View(enrollment);
        }
        [HttpPost]
        public IActionResult Edit(CourseStudents courseStudent)
        {
            repo.EditEnrollment(courseStudent);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var enrollment = repo.ReturnDetails(id);
            return View(enrollment);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            repo.DeleteEnrollment(id);
            return RedirectToAction("Index");
        }
    }
}
