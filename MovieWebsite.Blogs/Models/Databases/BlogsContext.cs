using System;
using System.Linq;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //PostgreSQL supports arrays, but i use MSSQL
            modelBuilder.Entity<Post>()
                .Property(e => e.RankUppers)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet());
            modelBuilder.Entity<Post>()
                .Property(e => e.RankDowners)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet());
        }
    }
}