using EmployeeManagementUsingDB.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingDB.Model
{
    public class SQLEmployeRepository : IEmployeeRepository
    {
        private readonly AppDbContext rydocontext;
        private readonly ILogger<SQLEmployeRepository> logger;

        public SQLEmployeRepository(AppDbContext rydocontext,ILogger<SQLEmployeRepository> logger)
        {
            this.rydocontext = rydocontext;
            this.logger = logger;
        }

        public Employee AddEmployee(Employee employee)
        {
            rydocontext.Employees.Add(employee);
            rydocontext.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee emp = rydocontext.Employees.Find(id);
            if(emp!=null)
            {
                rydocontext.Employees.Remove(emp);
                rydocontext.SaveChanges();
            }

            return emp;
        }

        public Employee GetEmployee(int Id)
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log");

            return rydocontext.Employees.Find(Id);
        }

        public IEnumerable<Employee> GetEmployee()
        {
            return rydocontext.Employees;
        }

        public Employee Update(Employee employeechanges)
        {
            var employee= rydocontext.Employees.Attach(employeechanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            rydocontext.SaveChanges();
            return employeechanges;
        }
    }
}
