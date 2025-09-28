using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class CourseStudents
    {
        public int Id { get; set; }
        public int Degree { get; set; }
        public int CrsId { get; set; }
        public int StdId { get; set; }

        // Navigation
        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
