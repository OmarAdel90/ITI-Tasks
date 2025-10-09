using MVC.Models;
using System.Security.Cryptography;
using System.Text;

namespace MVC.Data
{
    public static class Seed
    {
        public static void Initialize(AppDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if admin already exists
            if (context.ApplicationUsers.Any(u => u.Username == "admin"))
            {
                return; // DB has been seeded
            }

            // Create default admin user
            var adminUser = new ApplicationUser
            {
                Username = "admin",
                Email = "admin@school.com",
                PasswordHash = HashPassword("Admin123!"),
                Role = Roles.Admin,
                CreatedAt = DateTime.Now
            };

            context.ApplicationUsers.Add(adminUser);
            context.SaveChanges();

            Console.WriteLine("Default admin user created:");
            Console.WriteLine("Username: admin");
            Console.WriteLine("Password: Admin123!");
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}