using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public int DeptId { get; set; }
        public int CrsId { get; set; }

        // Navigation
        public Department Department { get; set; }
        public Course Course { get; set; }
    }
}
