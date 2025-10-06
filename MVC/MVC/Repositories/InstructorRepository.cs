using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Repositories
{
    public class InstructorRepository
    {
        private readonly AppDbContext context;
        public InstructorRepository(AppDbContext context) => this.context = context;

        public List<Instructor> ReturnInstructors()
        {
            var instructors = context.Instructors
                .Include(i => i.Department)
                .Include(i => i.Course)
                .ToList();
            return instructors;
        }
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
    }
}
