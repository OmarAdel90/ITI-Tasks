using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Repositories
{
    public class DepartmentRepository
    {
        private readonly AppDbContext context;
        public DepartmentRepository(AppDbContext context) => this.context = context;

        public Department ReturnDetails(int id)
        {
            return context.Departments
                .Include(d => d.Courses)
                .Include(d => d.Students)
                .FirstOrDefault(d => d.Id == id);
        }

        public void CreateDepartment(Department department)
        {
            context.Add(department);
            context.SaveChanges();
        }

        public void EditDepartment(Department department)
        {
            context.Update(department);
            context.SaveChanges();
        }

        public void DeleteDepartment(int id)
        {
            var department = context.Departments.Find(id);

            if (department == null)
            {
                throw new InvalidOperationException("Department not found.");
            }

            // Check dependencies in repository
            var courseCount = context.Courses.Count(c => c.DeptId == id);
            var studentCount = context.Students.Count(s => s.DeptId == id);

            if (courseCount > 0 || studentCount > 0)
            {
                var message = $"Cannot delete '{department.Name}' because it has ";
                if (courseCount > 0 && studentCount > 0)
                    message += $"{courseCount} courses and {studentCount} students.";
                else if (courseCount > 0)
                    message += $"{courseCount} courses.";
                else
                    message += $"{studentCount} students.";

                throw new InvalidOperationException(message);
            }

            context.Departments.Remove(department);
            context.SaveChanges();
        }

        public List<Department> GetDepartmentsBySearch(string searchString)
        {
            var query = context.Departments
                .Include(d => d.Courses)
                .Include(d => d.Students)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(d =>
                    d.Name.Contains(searchString) ||
                    d.ManagerName.Contains(searchString)
                );
            }

            return query.ToList();
        }
    }
}