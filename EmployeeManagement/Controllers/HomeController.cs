using EmployeeManagement.Model;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeerepository;

        public HomeController(IEmployeeRepository employeerepository)
        {
            _employeerepository = employeerepository;
        }
        public ViewResult Index()
        {
            //return _employeerepository.GetEmployee(1).Name;  

            return View(_employeerepository.GetEmployee());

        }

        public ViewResult Details(int id)
        {
            Employee model = _employeerepository.GetEmployee(id);

            HomeDetailsViewModel vm = new HomeDetailsViewModel();
            vm.Employee = model;
            vm.PageTitle = "Rydo Employees";

            return View(vm);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if(ModelState.IsValid)
            {
                Employee newemployee = _employeerepository.AddEmployee(employee);
               // return RedirectToAction("Details", new { id = employee.Id });
            }

            return View();
           
        }

        //public ViewResult Details()
        //{
        //    Employee model = _employeerepository.GetEmployee(1);



        //    // ViewData["Employee"]= model;
        //    //ViewData["Title"] = "Employee Details";

        //    //ViewBag.Employee = model;
        //    //ViewBag.Title = "Employee Details";

        //    //ViewBag.Title = "Employee Details";
        //    //return View(model);

        //    HomeDetailsViewModel vm = new HomeDetailsViewModel();
        //    vm.Employee = model;
        //    vm.PageTitle = "Rydo App";

        //    return View(vm);
        //}

        //public ViewResult Details()
        //{
        //    //return new ObjectResult(_employeerepository.GetEmployee(1));   // Use ObjectResult when you want .net core to have abl for content negotiation

        //    //return View(_employeerepository.GetEmployee(1)); // Normal Behaviour of .Net Core to look for the cshtml page related to this code

        //    //return View("RydoView", _employeerepository.GetEmployee(1)); // Looks for a RydoView.cshtml inside Home Controller.

        //    //return View("RydoViews/RydoPornHub.cshtml"); // Look for a view based on ABSOLUTE PATH

        //   // return View("Views/Home/Test.cshtml"); // Look for a view based on ABSOLUTE PATH / Another way to show default View.

        //     return View("../Test/Update"); // Look for a view based on Relative PATH 

        //}
    }
}
