using EmployeeManagementUsingDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingDB.Model
{
    public class SQLEmployeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context;
        public SQLEmployeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public Employee AddEmployee(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee emp = context.Employees.Find(id);
            if(emp!=null)
            {
                context.Employees.Remove(emp);
                context.SaveChanges();
            }

            return emp;
        }

        public Employee GetEmployee(int Id)
        {
            return context.Employees.Find(Id);
        }

        public IEnumerable<Employee> GetEmployee()
        {
            return context.Employees;
        }

        public Employee Update(Employee employeechanges)
        {
            var employee=context.Employees.Attach(employeechanges);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return employeechanges;
        }
    }
}
