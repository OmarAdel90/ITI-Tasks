using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Repositories
{
    public class CourseStudentRepository
    {
        // Db Context
        private readonly AppDbContext context;
        public CourseStudentRepository(AppDbContext context) => this.context = context;

        public List<Course> ReturnCourses()
        {
            return context.Courses.Include(c => c.Department).ToList();

        }
        public List<Student> ReturnStudents()
        {
            return context.Students.Include(c => c.Department).ToList();
        }
        public void CreateEnrollment(CourseStudents courseStudent)
        {
            context.Add(courseStudent);
            context.SaveChanges();
        }
        public CourseStudents FindEnrollment(int id) => context.CourseStudents.Find(id);
        public void EditEnrollment(CourseStudents courseStudent)
        {
            context.Update(courseStudent);
            context.SaveChanges();
        }
        public CourseStudents ReturnDetails(int id)
        {
            return context.CourseStudents
                .Include(cs => cs.Student)
                .Include(cs => cs.Course)
                .FirstOrDefault(cs => cs.Id == id);
        }
        public void DeleteEnrollment(int id)
        {
            context.CourseStudents.Remove(context.CourseStudents.Find(id));
            context.SaveChanges();
        }
        public List<CourseStudents> GetEnrollmentsBySearch(string searchString)
        {
            var query = context.CourseStudents
                .Include(cs => cs.Student)
                .Include(cs => cs.Course)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(e =>
                    (e.Student != null && e.Student.Name.Contains(searchString)) ||
                    (e.Course != null && e.Course.Name.Contains(searchString))
                );
            }

            return query.ToList();
        }
    }
}
