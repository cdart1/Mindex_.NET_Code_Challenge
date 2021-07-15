using challenge.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace challenge.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Compensation> Compensations { get; set; }
        // Using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // This creates a new unique identifier instead of providing a primary key 
            // in the Compensation Class.
            modelBuilder.Entity<Compensation>().HasKey(x => new { x.Salary, x.EffectiveDate });

            modelBuilder.Entity<Compensation>()
                .HasOne(b => b.Employee)
                .WithOne()
                .HasForeignKey("Compensation");
        }
    }
}
