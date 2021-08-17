using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MovieWebsite.Shared;

namespace MovieWebsite.Movies.Models.Databases
{
    public class MovieContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }

        public MovieContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=moviesdb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var enumValueComparer = new ValueComparer<HashSet<Genre>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToHashSet());
            var intValueComparer = new ValueComparer<HashSet<int>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToHashSet());
            
            modelBuilder.Entity<Movie>()
                .Property(e => e.Genres)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Enum.Parse<Genre>).ToHashSet())
                .Metadata.SetValueComparer(enumValueComparer);
            modelBuilder.Entity<Movie>()
                .Property(e => e.ActorIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet())
                .Metadata.SetValueComparer(intValueComparer);
            modelBuilder.Entity<Movie>()
                .Property(e => e.CharacterIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet())
                .Metadata.SetValueComparer(intValueComparer);
            modelBuilder.Entity<Movie>()
                .Property(e => e.DirectorIds)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet())
                .Metadata.SetValueComparer(intValueComparer);
        }
    }
}