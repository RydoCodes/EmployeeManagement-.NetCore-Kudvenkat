using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingIdentity.Models
{
    public static class ModelBuilderExtensions
    {
        public static void RydoSeed(this ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Employee>().HasData(
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
