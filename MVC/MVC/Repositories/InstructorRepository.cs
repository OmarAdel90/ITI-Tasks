using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Repositories
{
    public class InstructorRepository
    {
        private readonly AppDbContext context;

        public InstructorRepository(AppDbContext context) => this.context = context;

        public List<Course> ReturnCourses()
        {
            return context.Courses.Include(c => c.Department).ToList();
        }

        public List<Department> ReturnDepartments()
        {
            return context.Departments.ToList();
        }

        public void CreateInstructor(Instructor instructor)
        {
            context.Add(instructor);
            context.SaveChanges();
        }

        public Instructor FindInstructor(int id)
        {
            return context.Instructors.Find(id);
        }

        public void EditInstructor(Instructor instructor)
        {
            context.Update(instructor);
            context.SaveChanges();
        }

        public Instructor ReturnDetails(int id)
        {
            return context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .FirstOrDefault(i => i.Id == id);
        }

        public async Task<bool> DeleteInstructor(int instructorId)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var instructor = await context.Instructors
                    .FirstOrDefaultAsync(i => i.Id == instructorId);

                if (instructor == null)
                {
                    throw new InvalidOperationException("Instructor not found.");
                }

                // Step 1: Find and delete the associated ApplicationUser
                if (!string.IsNullOrEmpty(instructor.UserId))
                {
                    // Try to parse UserId as int (for ApplicationUser.Id)
                    if (int.TryParse(instructor.UserId, out int userId))
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

                // Step 2: Now delete the Instructor record
                context.Instructors.Remove(instructor);
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

        public List<Instructor> GetInstructorsBySearch(string searchString)
        {
            var query = context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(i =>
                    i.Name.Contains(searchString) ||
                    (i.Department != null && i.Department.Name.Contains(searchString)) ||
                    (i.Course != null && i.Course.Name.Contains(searchString))
                );
            }

            return query.ToList();
        }

        public Instructor GetInstructorByUserId(string userId)
        {
            return context.Instructors.FirstOrDefault(i => i.UserId == userId);
        }

        public List<Instructor> GetInstructorsByUserId(string userId)
        {
            return context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .Where(i => i.UserId == userId)
                .ToList();
        }
    }
}