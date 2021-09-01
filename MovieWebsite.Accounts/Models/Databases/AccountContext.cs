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
                        Username = "admin",
                        PasswordHash = "cN7owHGTd23c6+H/TA/Ysn3hrrNXtwCdEM6js9ZmPpWM420E",
                        Mail = "ramil2911912@gmail.com",
                        RegisterTime = DateTime.Parse("29.11.04"),
                        LastOnlineTime = DateTime.Now,
                        AvatarGuid = Guid.Empty,
                        Role = "admin"
                    },
                    new Account
                    {
                        Id = 2,
                        Username = "user",
                        PasswordHash = "cN7owHGTd23c6+H/TA/Ysn3hrrNXtwCdEM6js9ZmPpWM420E",
                        Mail = "ramil2911912@gmail.com",
                        RegisterTime = DateTime.Parse("29.11.04"),
                        LastOnlineTime = DateTime.Now,
                        AvatarGuid = Guid.Empty,
                        Role = "user"
                    }
                }));
            
        }
    }
}