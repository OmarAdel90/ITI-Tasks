using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public int Grade { get; set; }
        public int DeptId { get; set; }

        // Navigation
        public Department Department { get; set; }
        public ICollection<CourseStudents> CourseStudents { get; set; }
    }
}
