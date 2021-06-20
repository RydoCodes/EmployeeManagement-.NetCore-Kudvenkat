using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingDB.Model
{
    public static class ModelBuilderExtensions
    {
        public static void RydoSeed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Mary",
                    Department = Dept.IT,
                    Email = "mark@rydotech.com"
                },
                new Employee
                {
                    Id = 2,
                    Name = "John",
                    Department = Dept.HR,
                    Email = "John@rydotech.com"
                }
                );
        }
    }
}
