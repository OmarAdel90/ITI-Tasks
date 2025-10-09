using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } // "Admin", "HR", "Instructor", "Student"

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Links to specific entities based on role
        public int? InstructorId { get; set; }
        public int? StudentId { get; set; }

        // Navigation properties
        public Instructor Instructor { get; set; }
        public Student Student { get; set; }
    }
}