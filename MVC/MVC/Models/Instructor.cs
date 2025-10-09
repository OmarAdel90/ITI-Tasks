using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        // For AspNetUsers
        public string UserId { get; set; }

        [Required(ErrorMessage = "Instructor name is required")]
        [StringLength(100, ErrorMessage = "Instructor name cannot exceed 100 characters")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces allowed")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Salary is required")]
        [Range(1000, 100000, ErrorMessage = "Salary must be between 1,000 and 100,000")]
        public int Salary { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [StringLength(255, ErrorMessage = "Image path cannot exceed 255 characters")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int DeptId { get; set; }
        public int? CrsId { get; set; }

        // Navigation
        public Department Department { get; set; }
        public Course? Course { get; set; }
    }
}
