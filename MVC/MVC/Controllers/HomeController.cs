using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                // Get real counts from database
                ViewBag.DepartmentCount = _context.Departments.Count();
                ViewBag.CourseCount = _context.Courses.Count();
                ViewBag.StudentCount = _context.Students.Count();
                ViewBag.InstructorCount = _context.Instructors.Count();
            }
            catch (Exception ex)
            {
                // If there's an error, set default values
                ViewBag.DepartmentCount = 0;
                ViewBag.CourseCount = 0;
                ViewBag.StudentCount = 0;
                ViewBag.InstructorCount = 0;

                Console.WriteLine($"Error getting counts: {ex.Message}");
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}