using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Model
{
    public interface IEmployeeRepository
    {
        Employee GetEmployee(int Id);

        IEnumerable<Employee> GetEmployee();

        Employee AddEmployee(Employee employee);
    }
}
