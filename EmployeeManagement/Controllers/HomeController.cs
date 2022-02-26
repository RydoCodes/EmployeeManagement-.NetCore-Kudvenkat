using EmployeeManagement.Model;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeerepository;

        public HomeController(IEmployeeRepository employeerepository)
        {
            _employeerepository = employeerepository;
        }

        [Route("~/")]
        public RedirectToRouteResult Test()
        {
            return RedirectToRoute("default");
        }

        public ViewResult Index()
        {

            return View(_employeerepository.GetEmployee());

        }

        [Route("{id?}")]
        public ViewResult Details(int? id)
        {
            Employee model = _employeerepository.GetEmployee(id ?? 1);

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

        public ViewResult DetailsViewData()
        {
            Employee model = _employeerepository.GetEmployee(1);
            ViewData["Employee"]= model;
            ViewData["Title"] = "Employee Details";
            return View(model);

        }
        public ViewResult DetailsViewBag()
        {
            Employee model = _employeerepository.GetEmployee(1);
            ViewBag.Employee = model;
            ViewBag.Title = "Employee Details";
            return View(model);
        }

        public ObjectResult Details1()
        {
            return new ObjectResult(_employeerepository.GetEmployee(1));   // Use ObjectResult when you want .net core to have abl for content negotiation
        }
        public ViewResult Details3()
        {
            return View("RydoView", _employeerepository.GetEmployee(1)); // Looks for a RydoView.cshtml inside Home Controller.
        }
        public ViewResult Details4()
        {
            return View("RydoViews/RydoPornHub.cshtml"); // Look for a view based on ABSOLUTE PATH
        }
        public ViewResult Details5()
        {
            return View("Views/Home/Test.cshtml"); // Look for a view based on ABSOLUTE PATH / Another way to show default View.
        }
        public ViewResult Details6()
        {
            return View("../Test/Update"); // Look for a view based on Relative PATH
        }

    }
}
