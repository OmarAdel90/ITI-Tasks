using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using MVC.Services;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly AccountRepository _accountRepository;

        public AccountController(IAuthService authService, AccountRepository accountRepository)
        {
            _authService = authService;
            _accountRepository = accountRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(model.Username, model.Password, HttpContext);

            if (result)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password.");
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Departments = _accountRepository.GetDepartments();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // Clear validation errors for fields not required by the selected role
            if (model.RoleCode == "333") // HR
            {
                // Remove validation errors for fields not needed by HR
                ModelState.Remove("Name");
                ModelState.Remove("Address");
                ModelState.Remove("DepartmentId");
                ModelState.Remove("Salary");
            }
            else if (model.RoleCode == "111") // Student
            {
                // Remove validation errors for salary (not needed by students)
                ModelState.Remove("Salary");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _accountRepository.GetDepartments();
                return View(model);
            }

            // Validate role-specific fields
            if (model.RoleCode == "111") // Student
            {
                if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Address) || !model.DepartmentId.HasValue)
                {
                    ModelState.AddModelError("", "Name, Address, and Department are required for student registration.");
                    ViewBag.Departments = _accountRepository.GetDepartments();
                    return View(model);
                }
            }
            else if (model.RoleCode == "222") // Instructor
            {
                if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Address) ||
                    !model.DepartmentId.HasValue || !model.Salary.HasValue)
                {
                    ModelState.AddModelError("", "Name, Address, Department, and Salary are required for instructor registration.");
                    ViewBag.Departments = _accountRepository.GetDepartments();
                    return View(model);
                }
            }
            // HR (333) doesn't require additional validation - just username and password

            var result = await _accountRepository.RegisterUserAsync(
                model.Username,
                model.Password,
                model.RoleCode,
                model.Name,
                model.Address,
                model.DepartmentId,
                model.Salary);

            if (result.Success)
            {
                TempData["Success"] = "Registration successful! Please login.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", result.Error);
            ViewBag.Departments = _accountRepository.GetDepartments();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync(HttpContext);
            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}