using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Models
{
    public class FavoriteCategories
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Category")]
        [Display(Name = "Category")]
        [Required]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Profile")]
        [Display(Name = "Profile")]
        [Required]
        public int ProfileID { get; set; }
        public Profile Profile { get; set; }
    }
}