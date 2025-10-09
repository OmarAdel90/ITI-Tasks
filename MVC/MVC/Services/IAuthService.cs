using MVC.Models;

namespace MVC.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password, HttpContext httpContext);
        Task LogoutAsync(HttpContext httpContext);
        string GetCurrentUserRole(HttpContext httpContext);
        int? GetCurrentStudentId(HttpContext httpContext);
        int? GetCurrentInstructorId(HttpContext httpContext);
        string GetCurrentUserId(HttpContext httpContext);
    }
}