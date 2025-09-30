using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class CourseStudents
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Degree is required")]
        [Range(0, 100, ErrorMessage = "Degree must be between 0 and 100")]
        public int Degree { get; set; }

        [Required(ErrorMessage = "Course is required")]
        public int CrsId { get; set; }

        [Required(ErrorMessage = "Student is required")]
        public int StdId { get; set; }

        // Navigation
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
