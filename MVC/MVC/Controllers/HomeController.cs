using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Populate ViewBag with counts for authenticated users
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.DepartmentCount = await _context.Departments.CountAsync();
                ViewBag.CourseCount = await _context.Courses.CountAsync();
                ViewBag.StudentCount = await _context.Students.CountAsync();
                ViewBag.InstructorCount = await _context.Instructors.CountAsync();
            }
            else
            {
                // Default values for unauthenticated users
                ViewBag.DepartmentCount = 0;
                ViewBag.CourseCount = 0;
                ViewBag.StudentCount = 0;
                ViewBag.InstructorCount = 0;
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}