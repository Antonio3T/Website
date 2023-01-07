using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class Publisher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} required")]
        public string Name { get; set; }

        public ICollection<Game>? Games { get; set; }
    }
}