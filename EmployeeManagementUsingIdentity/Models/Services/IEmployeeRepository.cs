using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingIdentity.Models.Services
{
    // This is the Repository Pattern
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int Id);
        IEnumerable<Employee> GetEmployee();
        Employee AddEmployee(Employee employee);
        Employee Update(Employee employee);
        void Delete(int id);
    }
}
