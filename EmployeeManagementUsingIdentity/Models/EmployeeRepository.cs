using EmployeeManagementUsingIdentity.Models.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingIdentity.Models
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext rydodbcontext;
        private readonly ILogger<IEmployeeRepository> rydologger;

        public EmployeeRepository(AppDbContext rydodbcontext,ILogger<IEmployeeRepository> rydologger)
        {
            this.rydodbcontext = rydodbcontext;
            this.rydologger = rydologger;
        }
        public Employee AddEmployee(Employee employee)
        {
            rydodbcontext.Add(employee);
            rydodbcontext.SaveChanges();
            return employee;

        }

        public void Delete(int id)
        {
            Employee deletedemployee = rydodbcontext.Employees.Find(id);
            if(deletedemployee!=null)
            {
                rydodbcontext.Remove(deletedemployee);
                rydodbcontext.SaveChanges();
            }
        }

        public Employee GetEmployee(int Id) 
        {
            Employee rydoemployee = rydodbcontext.Employees.Find(Id);
            return rydoemployee;
        }

        public IEnumerable<Employee> GetEmployee()
        {
            return rydodbcontext.Employees;
        }

        public Employee Update(Employee employeechanges)
        {
            var employee = rydodbcontext.Employees.Attach(employeechanges);
            employee.State = EntityState.Modified;
            rydodbcontext.SaveChanges();
            return employeechanges;
        }
    }
}
