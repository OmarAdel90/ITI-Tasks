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

        public List<CourseStudents> ReturnEnrollments()
        {
            return context.CourseStudents
                .Include(cs => cs.Student)
                .Include(cs => cs.Course)
                .ToList();
        }
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
    }
}
