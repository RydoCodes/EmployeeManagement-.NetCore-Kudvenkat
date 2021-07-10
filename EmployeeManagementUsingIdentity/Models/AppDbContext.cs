using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingIdentity.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser> // By Default IdentityDbContext uses IdentityUser Class. If we want to use ApplicationUser class then
    {                                                              // use this code.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Not including this causes error while adding migration : The entity type 'IdentityUserLogin<string>' requires a primary key to be defined
            modelBuilder.RydoSeed();
        }
    }
}
