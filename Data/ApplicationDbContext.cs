using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Website.Models;

namespace Website.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PurchaseReceipts>().HasKey(p => new { p.GameID, p.ProfileID });
            builder.Entity<FavoriteCategories>().HasKey(p => new { p.CategoryID, p.ProfileID });

            base.OnModelCreating(builder);
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Game> Games { get; set; }
        //

        public DbSet<Website.Models.Category> Category { get; set; }

        public DbSet<Website.Models.Platform> Platform { get; set; }

        public DbSet<Website.Models.Publisher> Publisher { get; set; }

        public DbSet<Website.Models.Game> Game { get; set; }

        public DbSet<Website.Models.PurchaseReceipts> PurchaseReceipts { get; set; }

        public DbSet<Website.Models.FavoriteCategories> FavoriteCategories { get; set; }
    }
}