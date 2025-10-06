using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Repositories
{
    public class DepartmentRepository
    {
        private readonly AppDbContext context;
        public DepartmentRepository(AppDbContext context) => this.context = context;

        public List<Department> ReturnDepartments()
        {
            return context.Departments.ToList();
        }
        public Department ReturnDetails(int id)
        {
            return context.Departments.Find(id);
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

            // Check dependencies in repository
            var courseCount = context.Courses.Count(c => c.DeptId == id);

            if (courseCount > 0)
            {
                var message = $"Cannot delete '{department.Name}' because it has '{courseCount}' courses";
                throw new InvalidOperationException(message);
            }
            context.Departments.Remove(department);
            context.SaveChanges();
        }
    }
}
