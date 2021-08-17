using Microsoft.EntityFrameworkCore;

namespace MovieWebsite.Blogs.Models.Databases
{
    public class BlogsContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }

        public BlogsContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=blogssdb;Trusted_Connection=True;");
            }
        }
    }
}