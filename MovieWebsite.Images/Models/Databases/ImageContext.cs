using Microsoft.EntityFrameworkCore;

namespace MovieWebsite.Images.Models.Databases
{
    public class ImageContext : DbContext
    {
        public DbSet<Image> Images { get; set; }

        public ImageContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=imagesdb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}