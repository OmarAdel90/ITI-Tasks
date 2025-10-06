using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Repositories
{
    public class CourseRepository
    {
        // Db Context
        private readonly AppDbContext context;
        public CourseRepository(AppDbContext context) => this.context = context;

        public List<Course> ReturnCourses()
        {
            return context.Courses.Include(c => c.Department).ToList();
            
        }
        public Course ReturnDetails(int id)
        {
            return context.Courses.Find(id);

        }
        public Course FindCourse(int id)
        {
            return context.Courses.Find(id);
        }
        public List<Department> GetAllDepartments()
        {
            return context.Departments.ToList();
        }
        public void AddCourse(Course course)
        {
            context.Add(course);
            context.SaveChanges();
        }
        public void UpdateCourse(Course course)
        {
            context.Update(course);
            context.SaveChanges();
        }
        [HttpPost]
        public void DeleteCourse(int id)
        {
            var course = context.Courses.Find(id);

            // Check dependencies in repository
            var instructorCount = context.Instructors.Count(i => i.CrsId == id);
            var enrollmentCount = context.CourseStudents.Count(cs => cs.CrsId == id);

            if (instructorCount > 0 || enrollmentCount > 0)
            {
                var message = $"Cannot delete '{course.Name}' because it has ";

                if (instructorCount > 0 && enrollmentCount > 0)
                    message += $"{instructorCount} instructors and {enrollmentCount} student enrollments.";
                else if (instructorCount > 0)
                    message += $"{instructorCount} instructors assigned.";
                else
                    message += $"{enrollmentCount} student enrollments.";

                throw new InvalidOperationException(message);
            }
            context.Courses.Remove(course);
            context.SaveChanges();
        }
        public List<Course> GetCoursesByName(string searchString)
        {
            var query = context.Courses.Include(c => c.Department).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(c => c.Name.Contains(searchString));
            }

            return query.ToList();
        }

    }
}
