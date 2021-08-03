using System;
using Microsoft.EntityFrameworkCore;

namespace MovieWebsite.Accounts.Models.Databases
{
    public class AccountContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=accountsdb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Database.EnsureCreated();
            modelBuilder.Entity<Account>(x => x
                .HasData(new Account
                {
                    Login = "admin",
                    Password = "admin",
                    Mail = "ramil2911912@gmail.com",
                    RegisterTime = DateTime.Parse("29.11.04"),
                    LastOnlineTime = DateTime.Now,
                    AvatarId = 0,
                    Role = "admin"
                }));
            
            base.OnModelCreating(modelBuilder);
        }
    }
}