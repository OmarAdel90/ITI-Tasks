using System.Data;

namespace MVC.Models
{
    public static class RoleCodes
    {
        public const string Student = "111";
        public const string Instructor = "222";
        public const string HR = "333";

        public static string GetRoleByCode(string code)
        {
            return code switch
            {
                Student => Roles.Student,
                Instructor => Roles.Instructor,
                HR => Roles.HR,
                _ => null
            };
        }

        public static bool IsValidCode(string code)
        {
            return code == Student || code == Instructor || code == HR ;
        }
    }
}