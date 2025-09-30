using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Department
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Department name is required")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters and spaces allowed")]
        public string Name { get; set; }
        public string ManagerName { get; set; }
        
        // Navigation
        public ICollection<Student> Students { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<Instructor> Instructors { get; set; }
    }
}
