using Microsoft.EntityFrameworkCore;

namespace MovieWebsite.Characters.Models.Databases
{
    public class CharacterContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }

        public CharacterContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=charactersdb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>(x => x
                .HasData(new Character
                {
                    PosterId = 1,
                    Description = "Some cool guy",
                    FullName = "Vasya Prtkin",
                    OriginalFullName = "Вася Петькин"
                }));
            
        }
    }
}