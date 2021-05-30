using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackTest.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        //public ApplicationContext()
        //{

        //}

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .Property(e => e.RegistrationDate)
                .HasColumnType("date");

            modelBuilder
                .Entity<User>()
                .Property(e => e.LastActivityDate)
                .HasColumnType("date");
            //.HasConversion(
            //    //v => ((int)v).ToString(),
            //    //v => (TargetUserStatus)Enum.Parse(typeof(TargetUserStatus), v));
            //    v => v.Date);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=DateDB;Username=postgres;Password=a54g5x");
        }
    }
}
