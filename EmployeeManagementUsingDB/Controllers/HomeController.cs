using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementUsingDB.Model;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementUsingDB.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [Route("~/")]
        public IActionResult Index()
        {
            IEnumerable<Employee> lstEmployees = _employeeRepository.GetEmployee();
            return View(lstEmployees);
        }

        public ViewResult Details(int id)
        {
            Employee model = _employeeRepository.GetEmployee(id);
            return View(model);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                Employee newemployee = _employeeRepository.AddEmployee(employee);
            }

            return View();

        }
    }
}