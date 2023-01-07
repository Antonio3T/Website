using System.ComponentModel.DataAnnotations;

namespace Website.Models
{
    public class Game
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [MaxLength(50, ErrorMessage = "{0} has a max length of {1} characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} required")]
        public float Price { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [MaxLength(50, ErrorMessage = "{0} has a max length of {1} characters")]
        [RegularExpression(@"^.+\.([jJ][pP][gG])$")]
        public string Picture { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [DataType(DataType.Date)]
        public string ReleaseDate { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [MaxLength(1000, ErrorMessage = "{0} has a max length of {1} characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [MaxLength(20, ErrorMessage = "{0} has a max length of {1} characters")]
        public string Category { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [MaxLength(100, ErrorMessage = "{0} has a max length of {1} characters")]
        public string Platform { get; set; }

        [Required(ErrorMessage = "{0} required")]
        [MaxLength(50, ErrorMessage = "{0} has a max length of {1} characters")]
        public string Publisher { get; set; }
    }
}