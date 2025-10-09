using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters", MinimumLength = 3)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Role code is required")]
        [Display(Name = "Role Code")]
        [RegularExpression("^(111|222|333|999)$", ErrorMessage = "Role code must be 111 (Student), 222 (Instructor), 333 (HR), or 999 (Admin)")]
        public string RoleCode { get; set; }

        // Student/Instructor specific fields
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentId { get; set; }

        // Instructor specific field
        [Display(Name = "Salary")]
        [Range(1000, 100000, ErrorMessage = "Salary must be between 1,000 and 100,000")]
        public int? Salary { get; set; }
    }
}