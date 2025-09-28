using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Degree { get; set; }
        public int MinimumDegree { get; set; }
        public int Hours { get; set; }
        public int DeptId { get; set; }

        // Navigation
        public Department Department { get; set; }
        public ICollection<CourseStudents> CourseStudents { get; set; }
        public ICollection<Instructor> Instructors { get; set; }
    }
}
