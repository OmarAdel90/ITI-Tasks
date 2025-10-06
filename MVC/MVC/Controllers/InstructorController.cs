using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class InstructorController : Controller
    {
        private readonly InstructorRepository repo;
        public InstructorController(InstructorRepository repo) => this.repo = repo;
        public IActionResult Index(string searchString)
        {
            // If searchString is null it will return all instructors
            return View(repo.GetInstructorsBySearch(searchString));
        }
        public IActionResult Create(int id)
        {
            ViewBag.Departments = repo.ReturnDepartments();
            ViewBag.Courses = repo.ReturnCourses();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Instructor instructor)
        {
            repo.CreateInstructor(instructor);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var instructor = repo.FindInstructor(id);
            ViewBag.Departments = repo.ReturnDepartments();
            ViewBag.Courses = repo.ReturnCourses();
            return View(instructor);
        }
        [HttpPost]
        public IActionResult Edit(Instructor instructor)
        {
            repo.EditInstructor(instructor);
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            return View(repo.ReturnDetails(id));
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            repo.DeleteInstructor(id);
            return RedirectToAction("Index");
        }
    }
}
