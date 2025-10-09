using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Repositories
{
    public class StudentRepository
    {
        private readonly AppDbContext context;

        public StudentRepository(AppDbContext context) => this.context = context;

        public List<Student> GetStudentsBySearch(string searchString)
        {
            var query = context.Students
                .Include(s => s.Department)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(s =>
                    s.Name.Contains(searchString) ||
                    s.Address.Contains(searchString) ||
                    s.Department.Name.Contains(searchString)
                );
            }

            return query.ToList();
        }

        public Student ReturnDetails(int id)
        {
            return context.Students
                .Include(s => s.Department)
                .Include(s => s.CourseStudents)
                    .ThenInclude(cs => cs.Course)
                .FirstOrDefault(s => s.Id == id);
        }

        public List<Department> ReturnDepartments()
        {
            return context.Departments.ToList();
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

        public async Task<bool> DeleteStudent(int studentId)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var student = await context.Students
                    .Include(s => s.CourseStudents)
                    .FirstOrDefaultAsync(s => s.Id == studentId);

                if (student == null)
                {
                    throw new InvalidOperationException("Student not found.");
                }

                // Check for active enrollments
                var enrollmentCount = student.CourseStudents?.Count ?? 0;

                if (enrollmentCount > 0)
                {
                    throw new InvalidOperationException(
                        $"Cannot delete student '{student.Name}' as they have {enrollmentCount} active enrollment(s). " +
                        "Please remove all course enrollments before deleting the student."
                    );
                }

                // Step 1: Find and delete the associated ApplicationUser
                if (!string.IsNullOrEmpty(student.UserId))
                {
                    // Try to parse UserId as int (for ApplicationUser.Id)
                    if (int.TryParse(student.UserId, out int userId))
                    {
                        var user = await context.ApplicationUsers
                            .FirstOrDefaultAsync(u => u.Id == userId);

                        if (user != null)
                        {
                            // Delete the ApplicationUser account
                            context.ApplicationUsers.Remove(user);
                            await context.SaveChangesAsync();
                        }
                    }
                }

                // Step 2: Now delete the Student record
                context.Students.Remove(student);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch (InvalidOperationException)
            {
                await transaction.RollbackAsync();
                throw; // Re-throw to preserve the friendly error message
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public Student GetStudentByUserId(string userId)
        {
            return context.Students
                .Include(s => s.Department)
                .FirstOrDefault(s => s.UserId == userId);
        }

        public List<Student> GetAllStudents()
        {
            return context.Students
                .Include(s => s.Department)
                .ToList();
        }
    }
}