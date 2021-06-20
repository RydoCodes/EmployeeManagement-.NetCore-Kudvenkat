using EmployeeManagement.Model;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    public class AttributeRoutesinOnlyAtion : Controller
    {
        private IEmployeeRepository _employeerepository;

        public AttributeRoutesinOnlyAtion(IEmployeeRepository employeerepository)
        {
            _employeerepository = employeerepository;
        }
       // [Route("")]
        [Route("AttributeRoutesinOnlyAtion")]
        [Route("AttributeRoutesinOnlyAtion/Index")]
        public ViewResult Index()
        {
            //return _employeerepository.GetEmployee(1).Name;  

            return View("~/Views/Home/Index.cshtml", _employeerepository.GetEmployee());

        }
        [Route("AttributeRoutesinOnlyAtion/Details/{id?}")]
        public ViewResult Details(int? id)
        {
            Employee model = _employeerepository.GetEmployee(id??1);

            HomeDetailsViewModel vm = new HomeDetailsViewModel();
            vm.Employee = model;
            vm.PageTitle = "Rydo App";

            return View("~/Views/Home/Details.cshtml", vm);
        }
    }
}
