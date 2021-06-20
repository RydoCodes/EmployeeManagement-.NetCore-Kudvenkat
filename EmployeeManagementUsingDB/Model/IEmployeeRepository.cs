
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingDB.Model
{

    // This is the Repository Pattern
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int Id);

        IEnumerable<Employee> GetEmployee();

        Employee AddEmployee(Employee employee);

        Employee Update(Employee employee);

        Employee Delete(int id);


    }
}
