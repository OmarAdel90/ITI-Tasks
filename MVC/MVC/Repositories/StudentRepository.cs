using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Repositories
{
    public class StudentRepository
    {
        private readonly AppDbContext context;
        public StudentRepository(AppDbContext context) => this.context = context;

        public List<Student> ReturnStudents()
        {
            return context.Students.Include(s => s.Department).ToList();
        }
        public List<Department> ReturnDepartments()
        {
            return context.Departments.ToList();
        }
        public Student ReturnDetails(int id)
        {
            return context.Students
                .Include(s => s.Department)
                .Include(s => s.CourseStudents)
                    .ThenInclude(cs => cs.Course)
                .FirstOrDefault(s => s.Id == id);
        }
        public void CreateStudent(Student student)
        {
            context.Add(student);
            context.SaveChanges();
        }
        public void EditStudent(Student student)
        {
            context.Update(student);
            context.SaveChanges();
        }
        public void DeleteStudent(int id)
        {
            var student = ReturnDetails(id);
            // Removes Enrollments
            var enrollments = context.CourseStudents.Where(cs => cs.StdId == id).ToList();
            context.CourseStudents.RemoveRange(enrollments);
            context.Remove(student);
            context.SaveChanges();
        }
    }
}
