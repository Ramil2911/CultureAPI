using Microsoft.EntityFrameworkCore;

namespace MovieWebsite.Blogs.Models.Databases
{
    public class DraftsContext : DbContext
    {
        public DbSet<Post> Drafts { get; set; }
        
        public DraftsContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=blogdraftsdb;Trusted_Connection=True;");
        }
    }
}