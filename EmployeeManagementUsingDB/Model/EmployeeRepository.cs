using EmployeeManagementUsingDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingDB.Model
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private IList<Employee> _employeeList;

        public EmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() { Id = 1, Name = "Mary", Department = Dept.HR, Email = "mary@pragimtech.com" },
                new Employee() { Id = 2, Name = "John", Department = Dept.IT, Email = "john@pragimtech.com" },
                new Employee() { Id = 3, Name = "Sam", Department = Dept.IT, Email = "sam@pragimtech.com" },
            };
        }

        public Employee AddEmployee(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee deletedemployee = _employeeList.FirstOrDefault(e => e.Id == id);
            if(deletedemployee!=null)
            {
                _employeeList.Remove(deletedemployee);
            }
            return deletedemployee;
        }

        public Employee GetEmployee(int id)
        {
            Employee emp = _employeeList.FirstOrDefault(emp => emp.Id == id);
            return emp;

        }

        public IEnumerable<Employee> GetEmployee()
        {
            return _employeeList;
        }

        public Employee Update(Employee changedemployee)
        {
            Employee emp = _employeeList.FirstOrDefault(e => e.Id == changedemployee.Id);
            if (emp != null)
            {
                emp.Name = changedemployee.Name;
                emp.Email = changedemployee.Email;
                emp.Department = changedemployee.Department;
            }

            return emp;
        }
    }
}
