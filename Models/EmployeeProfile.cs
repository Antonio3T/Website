using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class EmployeeProfile
    {
        public int Id { get; set; }

        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}