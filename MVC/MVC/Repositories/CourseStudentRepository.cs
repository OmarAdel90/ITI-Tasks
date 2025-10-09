using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Repositories
{
    public class CourseStudentRepository
    {
        private readonly AppDbContext context;

        public CourseStudentRepository(AppDbContext context) => this.context = context;

        public List<Course> ReturnCourses()
        {
            return context.Courses.Include(c => c.Department).ToList();
        }

        public List<Course> ReturnAvailableCourses(int studentId)
        {
            // Get courses the student is NOT enrolled in
            var enrolledCourseIds = context.CourseStudents
                .Where(cs => cs.StdId == studentId)
                .Select(cs => cs.CrsId)
                .ToList();

            return context.Courses
                .Include(c => c.Department)
                .Where(c => !enrolledCourseIds.Contains(c.Id))
                .ToList();
        }

        public List<Student> ReturnStudents()
        {
            return context.Students.Include(s => s.Department).ToList();
        }

        public void CreateEnrollment(CourseStudents courseStudent)
        {
            context.Add(courseStudent);
            context.SaveChanges();
        }

        public CourseStudents FindEnrollment(int id) =>
            context.CourseStudents.Find(id);

        public void EditEnrollment(CourseStudents courseStudent)
        {
            context.Update(courseStudent);
            context.SaveChanges();
        }

        public CourseStudents ReturnDetails(int id)
        {
            return context.CourseStudents
                .Include(cs => cs.Student)
                .ThenInclude(s => s.Department)
                .Include(cs => cs.Course)
                .ThenInclude(c => c.Department)
                .FirstOrDefault(cs => cs.Id == id);
        }

        public void DeleteEnrollment(int id)
        {
            var enrollment = context.CourseStudents.Find(id);
            if (enrollment != null)
            {
                context.CourseStudents.Remove(enrollment);
                context.SaveChanges();
            }
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

        public List<CourseStudents> GetEnrollmentsByStudentAndSearch(int studentId, string searchString)
        {
            var query = context.CourseStudents
                .Include(cs => cs.Student)
                .Include(cs => cs.Course)
                .Where(cs => cs.StdId == studentId);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(e =>
                    (e.Course != null && e.Course.Name.Contains(searchString))
                );
            }

            return query.ToList();
        }

        public bool IsAlreadyEnrolled(int studentId, int courseId)
        {
            return context.CourseStudents
                .Any(cs => cs.StdId == studentId && cs.CrsId == courseId);
        }
        public List<CourseStudents> GetEnrollmentsByInstructorId(int instructorId)
        {
            return context.CourseStudents
                .Include(cs => cs.Student)
                .Include(cs => cs.Course)
                    .ThenInclude(c => c.Department)
                .Include(cs => cs.Course)
                    .ThenInclude(c => c.Instructors)
                .Where(cs => cs.Course.Instructors.Any(i => i.Id == instructorId))
                .ToList();
        }

        public bool IsCourseTaughtByInstructor(int courseId, int instructorId)
        {
            return context.Courses
                .Include(c => c.Instructors)
                .Any(c => c.Id == courseId && c.Instructors.Any(i => i.Id == instructorId));
        }

        public List<CourseStudents> GetEnrollmentsByStudentId(int studentId)
        {
            return context.CourseStudents
                .Include(cs => cs.Student)
                .Include(cs => cs.Course)
                .ThenInclude(c => c.Department)
                .Where(cs => cs.StdId == studentId)
                .ToList();
        }

        public bool EnrollmentExists(int studentId, int courseId)
        {
            return context.CourseStudents
                .Any(cs => cs.StdId == studentId && cs.CrsId == courseId);
        }

        public string GetStudentName(int studentId)
        {
            var student = context.Students.Find(studentId);
            return student?.Name ?? "Unknown Student";
        }
    }
}