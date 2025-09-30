using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly AppDbContext context;
        public DepartmentController(AppDbContext context) => this.context = context;
        public IActionResult Index()
        {
            List<Department> departments = context.Departments.ToList();
            return View("Index",departments);
        }
        public IActionResult Details(int id)
        {
            var department = context.Departments.Find(id);
            return View("Details", department);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Department department)
        {
            context.Add(department);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            var department = context.Departments.Find(id);
            return View("Edit", department);
        }
        [HttpPost]
        public IActionResult Edit(Department department)
        {
            context.Update(department);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var department = context.Departments.Find(id);
            context.Remove(department);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
