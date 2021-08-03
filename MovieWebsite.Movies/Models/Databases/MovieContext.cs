using Microsoft.EntityFrameworkCore;

namespace MovieWebsite.Movies.Models.Databases
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=moviesdb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Database.EnsureCreated();

            base.OnModelCreating(modelBuilder);
        }
    }
}