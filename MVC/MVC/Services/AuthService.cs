using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using MVC.Models;
using MVC.Repositories;
using System.Security.Claims;

namespace MVC.Services
{
    public class AuthService : IAuthService
    {
        private readonly AccountRepository _accountRepository;
        private readonly StudentRepository _studentRepository;
        private readonly InstructorRepository _instructorRepository;

        public AuthService(
            AccountRepository accountRepository,
            StudentRepository studentRepository,
            InstructorRepository instructorRepository)
        {
            _accountRepository = accountRepository;
            _studentRepository = studentRepository;
            _instructorRepository = instructorRepository;
        }

        public async Task<bool> LoginAsync(string username, string password, HttpContext httpContext)
        {
            var user = await _accountRepository.LoginUserAsync(username, password);

            if (user == null)
            {
                return false;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Add student/instructor ID if applicable
            if (user.Role == Roles.Student)
            {
                var student = _studentRepository.GetStudentByUserId(user.Id.ToString());
                if (student != null)
                {
                    claims.Add(new Claim("StudentId", student.Id.ToString()));
                }
            }
            else if (user.Role == Roles.Instructor)
            {
                var instructor = _instructorRepository.GetInstructorByUserId(user.Id.ToString());
                if (instructor != null)
                {
                    claims.Add(new Claim("InstructorId", instructor.Id.ToString()));
                }
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)
            };

            await httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return true;
        }

        public async Task LogoutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public string GetCurrentUserRole(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
        }

        public int? GetCurrentStudentId(HttpContext httpContext)
        {
            var studentIdClaim = httpContext.User.FindFirst("StudentId")?.Value;
            if (int.TryParse(studentIdClaim, out int studentId))
            {
                return studentId;
            }
            return null;
        }

        public int? GetCurrentInstructorId(HttpContext httpContext)
        {
            var instructorIdClaim = httpContext.User.FindFirst("InstructorId")?.Value;
            if (int.TryParse(instructorIdClaim, out int instructorId))
            {
                return instructorId;
            }
            return null;
        }

        public string GetCurrentUserId(HttpContext httpContext)
        {
            return httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}