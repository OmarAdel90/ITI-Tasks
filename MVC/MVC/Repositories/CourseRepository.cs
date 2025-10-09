using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Repositories
{
    public class CourseRepository
    {
        private readonly AppDbContext context;

        public CourseRepository(AppDbContext context) => this.context = context;

        public List<Course> ReturnCourses()
        {
            return context.Courses.Include(c => c.Department).ToList();
        }

        public Course ReturnDetails(int id)
        {
            return context.Courses
                .Include(c => c.Department)
                .Include(c => c.Instructors)
                .Include(c => c.CourseStudents)
                .FirstOrDefault(c => c.Id == id);
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

        public void DeleteCourse(int id)
        {
            var course = context.Courses.Find(id);
            if (course == null)
            {
                throw new InvalidOperationException("Course not found.");
            }

            // Check dependencies
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

        // Methods for role-based filtering
        public List<Course> GetCoursesByInstructor(int instructorId)
        {
            return context.Courses
                .Include(c => c.Department)
                .Where(c => c.Instructors.Any(i => i.Id == instructorId))
                .ToList();
        }

        public List<Course> GetCoursesByStudent(int studentId)
        {
            return context.Courses
                .Include(c => c.Department)
                .Where(c => c.CourseStudents.Any(cs => cs.StdId == studentId))
                .ToList();
        }

        public bool IsInstructorTeachingCourse(int instructorId, int courseId)
        {
            return context.Instructors.Any(i => i.Id == instructorId && i.CrsId == courseId);
        }

        public bool IsStudentEnrolledInCourse(int studentId, int courseId)
        {
            return context.CourseStudents.Any(cs => cs.StdId == studentId && cs.CrsId == courseId);
        }
    }
}