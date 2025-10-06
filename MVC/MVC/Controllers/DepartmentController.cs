using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DepartmentRepository repo;
        public DepartmentController(DepartmentRepository repo) => this.repo = repo;
        public IActionResult Index(string searchString)
        {
            var departments = repo.GetDepartmentsBySearch(searchString);
            return View(departments);
        }
        public IActionResult Details(int id)
        {
            var department = repo.ReturnDetails(id);
            return View(department);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            repo.CreateDepartment(department);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var department = repo.ReturnDetails(id);
            return View(department);
        }
        [HttpPost]
        public IActionResult Edit(Department department)
        {
            repo.EditDepartment(department);
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            try
            {
                repo.DeleteDepartment(id);
                TempData["Success"] = "Department deleted successfully!";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                TempData.Keep();
            }
            return RedirectToAction("Index");
        }
    }
}
