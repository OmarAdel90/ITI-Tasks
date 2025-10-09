using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Course name is required")]
        [RegularExpression(@"^[A-Za-z0-9\s\-&]+$", ErrorMessage = "Only letters, numbers, spaces, hyphens, and ampersands allowed")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Degree is required")]
        [Range(100, 300, ErrorMessage = "Degree must be between 100 and 200")]
        public int Degree { get; set; }
        [Required(ErrorMessage = "Minimum Degree is required")]
        [Range(50, 100, ErrorMessage = "Minimum Degree must be between 50 and 100")]
        public int MinimumDegree { get; set; }
        [Required(ErrorMessage = "Hours are required")]
        [Range(1, 4, ErrorMessage = "Hours must be between 1 and 4")]
        public int Hours { get; set; }
        [Required(ErrorMessage = "Department is required")]
        public int DeptId { get; set; }

        // Navigation
        public Department Department { get; set; }
        public ICollection<CourseStudents> CourseStudents { get; set; }
        public ICollection<Instructor> Instructors { get; set; }
    }
}
