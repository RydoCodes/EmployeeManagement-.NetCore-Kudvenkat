using EmployeeManagementUsingDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingDB.Model
{
    public class SQLEmployeRepository : IEmployeeRepository
    {
        private readonly AppDbContext rydocontext;
        public SQLEmployeRepository(AppDbContext rydocontext)
        {
            this.rydocontext = rydocontext;
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
