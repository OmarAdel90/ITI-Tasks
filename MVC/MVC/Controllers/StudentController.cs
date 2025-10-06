using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly StudentRepository repo;
        public StudentController(StudentRepository repo) => this.repo = repo;
        public IActionResult Index(string searchString)
        {
            return View(repo.GetStudentsBySearch(searchString));
        }
        public IActionResult Details(int id)
        {
            return View(repo.ReturnDetails(id));
        }
        public IActionResult Create()
        {
            ViewBag.Departments = repo.ReturnDepartments();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student student)
        {
            repo.CreateStudent(student);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var student = repo.ReturnDetails(id);
            ViewBag.Departments = repo.ReturnDepartments();
            return View("Edit", student);
        }
        [HttpPost]
        public IActionResult Edit(Student student)
        {
            repo.EditStudent(student);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            repo.DeleteStudent(id);
            return RedirectToAction("Index");
        }

    }
}
