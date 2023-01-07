using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Models
{
    public class GameF
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

        [ForeignKey("Category")]
        [Required(ErrorMessage = "{0} required")]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        [ForeignKey("Platform")]
        [Required(ErrorMessage = "{0} required")]
        public int PlatformID { get; set; }
        public Platform Platform { get; set; }

        [ForeignKey("Publisher")]
        //[Required(ErrorMessage = "{0} required")]
        public int PublisherID { get; set; }
        public Publisher Publisher { get; set; }

        public virtual ICollection<PurchaseReceipts>? Receipt { get; set; }
    }
}