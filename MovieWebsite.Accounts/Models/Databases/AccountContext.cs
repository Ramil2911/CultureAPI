using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MovieWebsite.Accounts.Models.Databases
{
    public class AccountContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AccountContext()
        {
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=accountsdb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(x => x
                .HasData(new List<Account>()
                {
                    new Account
                    {
                        Id = 1,
                        Login = "admin",
                        Password = "admin",
                        Mail = "ramil2911912@gmail.com",
                        RegisterTime = DateTime.Parse("29.11.04"),
                        LastOnlineTime = DateTime.Now,
                        AvatarId = 0,
                        Role = "admin"
                    },
                    new Account
                    {
                        Id = 2,
                        Login = "user",
                        Password = "user",
                        Mail = "ramil2911912@gmail.com",
                        RegisterTime = DateTime.Parse("29.11.04"),
                        LastOnlineTime = DateTime.Now,
                        AvatarId = 0,
                        Role = "user"
                    }
                }));
            
        }
    }
}