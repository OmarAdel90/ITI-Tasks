using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System.Security.Cryptography;
using System.Text;

namespace MVC.Repositories
{
    public class AccountRepository
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(bool Success, string Error)> RegisterUserAsync(
            string username,
            string password,
            string roleCode,
            string name = null,
            string address = null,
            int? departmentId = null,
            int? salary = null)
        {
            try
            {
                // Validate username and password
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return (false, "Username and password are required");
                }

                // Check if user already exists
                if (await _context.ApplicationUsers.AnyAsync(u => u.Email == username || u.Username == username))
                {
                    return (false, "User with this username already exists");
                }

                // Validate role code
                if (string.IsNullOrWhiteSpace(roleCode) || !new[] { "111", "222", "333" }.Contains(roleCode))
                {
                    return (false, "Invalid role code. Please use 111 (Student), 222 (Instructor), or 333 (HR)");
                }

                // Map role codes to role names
                string roleName = roleCode switch
                {
                    "111" => Roles.Student,
                    "222" => Roles.Instructor,
                    "333" => Roles.HR,
                    _ => null
                };

                // Validate role-specific fields
                if (roleName == Roles.Student)
                {
                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address) || !departmentId.HasValue)
                    {
                        return (false, "Name, Address, and Department are required for student registration");
                    }
                }
                else if (roleName == Roles.Instructor)
                {
                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address) ||
                        !departmentId.HasValue || !salary.HasValue)
                    {
                        return (false, "Name, Address, Department, and Salary are required for instructor registration");
                    }
                }
                // HR role doesn't require additional fields

                // Create user
                var user = new ApplicationUser
                {
                    Username = username,
                    Email = username,
                    PasswordHash = HashPassword(password),
                    Role = roleName
                };

                _context.ApplicationUsers.Add(user);
                await _context.SaveChangesAsync();

                // Create Student record if role is Student
                if (roleName == Roles.Student)
                {
                    var student = new Student
                    {
                        UserId = user.Id.ToString(),
                        Name = name,
                        Address = address,
                        Grade = 0,
                        DeptId = departmentId.Value,
                        Image = ""
                    };

                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();

                    // Link student to user
                    user.StudentId = student.Id;
                    await _context.SaveChangesAsync();
                }
                // Create Instructor record if role is Instructor
                else if (roleName == Roles.Instructor)
                {
                    var instructor = new Instructor
                    {
                        UserId = user.Id.ToString(),
                        Name = name,
                        Address = address,
                        Salary = salary.Value,
                        DeptId = departmentId.Value,
                        CrsId = null, // Course can be assigned later
                        Image = ""
                    };

                    _context.Instructors.Add(instructor);
                    await _context.SaveChangesAsync();

                    // Link instructor to user
                    user.InstructorId = instructor.Id;
                    await _context.SaveChangesAsync();
                }
                // HR users don't need additional records - just the ApplicationUser

                return (true, null);
            }
            catch (Exception ex)
            {
                // Log the exception if you have logging set up
                return (false, $"An error occurred during registration: {ex.Message}");
            }
        }

        public async Task<ApplicationUser> LoginUserAsync(string username, string password)
        {
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Username == username || u.Email == username);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }

        public List<Department> GetDepartments()
        {
            return _context.Departments.ToList();
        }

        // Password hashing methods
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hashedPassword;
        }
    }
}