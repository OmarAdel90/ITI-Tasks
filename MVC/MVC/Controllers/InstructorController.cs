using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System.Security.Claims;

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin,HR,Instructor")]
    public class InstructorController : Controller
    {
        private readonly AppDbContext _context;

        public InstructorController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Instructor
        public async Task<IActionResult> Index(string searchString)
        {
            ViewBag.CurrentFilter = searchString;

            IQueryable<Instructor> instructorsQuery = _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course);

            // If user is an instructor, only show their own profile
            if (User.IsInRole("Instructor") && !User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

                if (user?.InstructorId != null)
                {
                    instructorsQuery = instructorsQuery.Where(i => i.Id == user.InstructorId.Value);
                }
                else
                {
                    // Instructor has no profile yet
                    return View(new List<Instructor>());
                }
            }
            else if (!string.IsNullOrEmpty(searchString))
            {
                instructorsQuery = instructorsQuery.Where(i =>
                    i.Name.Contains(searchString) ||
                    i.Department.Name.Contains(searchString));
            }

            var instructors = await instructorsQuery.ToListAsync();
            return View(instructors);
        }

        // GET: Instructor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            // Check if instructor is viewing their own profile
            if (User.IsInRole("Instructor") && !User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

                if (user?.InstructorId != instructor.Id)
                {
                    return RedirectToAction("AccessDenied", "Account");
                }
            }

            return View(instructor);
        }

        // GET: Instructor/Create
        [Authorize(Roles = "Admin,HR")]
        public IActionResult Create()
        {
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Courses = _context.Courses.ToList();
            return View();
        }

        // POST: Instructor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR")]
        public async Task<IActionResult> Create(Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Instructor created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Courses = _context.Courses.ToList();
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (instructor == null)
            {
                return NotFound();
            }

            // Only regular instructors (not Admin/HR) need to check if it's their own profile
            if (User.IsInRole("Instructor") && !User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

                if (user?.InstructorId != instructor.Id)
                {
                    TempData["Error"] = "You can only edit your own profile.";
                    return RedirectToAction("AccessDenied", "Account");
                }
            }

            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Courses = _context.Courses.ToList();
            return View(instructor);
        }

        // POST: Instructor/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Name,Salary,Address,Image,DeptId,CrsId")] Instructor instructor)
        {
            if (id != instructor.Id)
            {
                return NotFound();
            }

            // Only regular instructors (not Admin/HR) need authorization check
            if (User.IsInRole("Instructor") && !User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

                if (user?.InstructorId != instructor.Id)
                {
                    TempData["Error"] = "You can only edit your own profile.";
                    return RedirectToAction("AccessDenied", "Account");
                }
            }

            // Remove validation errors for navigation properties
            ModelState.Remove("Department");
            ModelState.Remove("Course");

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing instructor to preserve UserId if not provided
                    var existingInstructor = await _context.Instructors
                        .AsNoTracking()
                        .FirstOrDefaultAsync(i => i.Id == id);

                    if (existingInstructor != null && string.IsNullOrEmpty(instructor.UserId))
                    {
                        instructor.UserId = existingInstructor.UserId;
                    }

                    // Handle empty CrsId from dropdown (convert empty string to null)
                    if (!instructor.CrsId.HasValue || instructor.CrsId.Value == 0)
                    {
                        instructor.CrsId = null;
                    }

                    _context.Update(instructor);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Instructor updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Error updating instructor: {ex.Message}";
                }
            }
            else
            {
                // Debug: Show model state errors
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                TempData["Error"] = "Validation failed: " + string.Join(", ", errors);
            }

            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Courses = _context.Courses.ToList();
            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                TempData["Error"] = "Instructor not found.";
                return RedirectToAction(nameof(Index));
            }

            // Only regular instructors (not Admin/HR) need authorization check
            if (User.IsInRole("Instructor") && !User.IsInRole("Admin") && !User.IsInRole("HR"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

                if (user?.InstructorId != instructor.Id)
                {
                    TempData["Error"] = "You don't have permission to delete this instructor.";
                    return RedirectToAction("AccessDenied", "Account");
                }
            }

            try
            {
                // First, find and delete the associated user account
                var user = await _context.ApplicationUsers
                    .FirstOrDefaultAsync(u => u.InstructorId == id);

                if (user != null)
                {
                    _context.ApplicationUsers.Remove(user);
                }

                // Then delete the instructor
                _context.Instructors.Remove(instructor);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Instructor deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting instructor: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Instructor/MyCourses
        [Authorize(Roles = "Instructor")]
        public async Task<IActionResult> MyCourses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user?.InstructorId == null)
            {
                TempData["Error"] = "Instructor profile not found.";
                return RedirectToAction("Index", "Home");
            }

            var instructor = await _context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                    .ThenInclude(c => c.Department)
                .FirstOrDefaultAsync(i => i.Id == user.InstructorId);

            if (instructor == null)
            {
                TempData["Error"] = "Instructor profile not found.";
                return RedirectToAction("Index", "Home");
            }

            return View(instructor);
        }

        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.Id == id);
        }
    }
}