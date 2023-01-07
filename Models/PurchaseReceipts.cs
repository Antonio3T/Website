using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Models
{
    public class PurchaseReceipts
    {
        [Key, Column(Order = 0)]
        [ForeignKey("Game")]
        [Display(Name = "Game")]
        [Required]
        public int GameID { get; set; }
        public Game Game { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("Profile")]
        [Display(Name = "Profile")]
        [Required]
        public int ProfileID { get; set; }
        public Profile Profile { get; set; }

        public float Value { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}