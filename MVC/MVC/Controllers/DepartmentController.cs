using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using System.Security.Claims;

namespace MVC.Controllers
{
    [Authorize] // Require authentication for all actions
    public class DepartmentController : Controller
    {
        private readonly DepartmentRepository repo;

        public DepartmentController(DepartmentRepository repo) => this.repo = repo;

        // Admin, HR, Instructor, Student can view departments
        [Authorize(Roles = "Admin,HR,Instructor,Student")]
        public IActionResult Index(string searchString)
        {
            var departments = repo.GetDepartmentsBySearch(searchString);
            ViewBag.CurrentFilter = searchString;
            return View(departments);
        }

        // Admin, HR, Instructor, Student can view details
        [Authorize(Roles = "Admin,HR,Instructor,Student")]
        public IActionResult Details(int id)
        {
            var department = repo.ReturnDetails(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // Only Admin can create departments
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department department)
        {
            // Remove navigation properties from ModelState validation
            ModelState.Remove("Students");
            ModelState.Remove("Courses");
            ModelState.Remove("Instructors");

            if (!ModelState.IsValid)
            {
                return View(department);
            }

            repo.CreateDepartment(department);
            TempData["Success"] = "Department created successfully!";
            return RedirectToAction("Index");
        }

        // Only Admin can edit departments
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var department = repo.ReturnDetails(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department department)
        {
            // Remove navigation properties from ModelState validation
            ModelState.Remove("Students");
            ModelState.Remove("Courses");
            ModelState.Remove("Instructors");

            if (!ModelState.IsValid)
            {
                return View(department);
            }

            repo.EditDepartment(department);
            TempData["Success"] = "Department updated successfully!";
            return RedirectToAction("Index");
        }

        // Only Admin can delete departments
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
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
            }
            return RedirectToAction("Index");
        }
    }
}