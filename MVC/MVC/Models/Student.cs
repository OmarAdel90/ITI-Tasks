using System.ComponentModel.DataAnnotations;
namespace MVC.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Student name is required")]
        [StringLength(100, ErrorMessage = "Student name cannot exceed 100 characters")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces allowed")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "Image path cannot exceed 255 characters")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Grade is required")]
        [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100")]
        public int Grade { get; set; }

        [Required(ErrorMessage = "Department is required")]
        public int DeptId { get; set; }

        // Navigation
        public Department Department { get; set; }
        public ICollection<CourseStudents> CourseStudents { get; set; }
    }
}