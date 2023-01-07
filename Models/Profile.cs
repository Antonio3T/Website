using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class Profile
    {
        public int Id { get; set; }

        public string Username { get; set; }

        //[EmailAddress]
        //public string Email { get; set; }

        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required]
        public string Role { get; set; }
    }
}