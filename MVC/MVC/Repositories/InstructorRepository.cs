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
        public List<Department> ReturnDepartments(){
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
        public void DeleteInstructor(int id)
        {
            var instructor = context.Instructors.Find(id);
            context.Remove(instructor);
            context.SaveChanges();
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
                    (i.Department != null && i.Department.Name.Contains(searchString))
                );
            }

            return query.ToList();
        }
    }
}
