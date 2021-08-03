using Microsoft.EntityFrameworkCore;

namespace MovieWebsite.Persons.Models.Databases
{
    public class PersonContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=personsdb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Database.EnsureCreated();

            base.OnModelCreating(modelBuilder);
        }
    }
}