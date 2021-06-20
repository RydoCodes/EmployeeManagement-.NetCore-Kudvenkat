using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Model;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;


// Attribute Routing Concepts below
namespace EmployeeManagement.Controllers
{
    [Route("AttributeRouteincontrollerandactipnController")]
    public class AttributeRouteincontrollerandactipnController : Controller
    {
        private IEmployeeRepository _employeerepository;

        public AttributeRouteincontrollerandactipnController(IEmployeeRepository employeerepository)
        {
            _employeerepository = employeerepository;
        }
        //[Route("~/")]
        [Route("")]
        [Route("Index")]
        public ViewResult Index()
        {
            //return _employeerepository.GetEmployee(1).Name;  

            return View("~/Views/Home/Index.cshtml", _employeerepository.GetEmployee());

        }
        [Route("Details/{id?}")]
        public ViewResult Details(int? id) 
        {
            Employee model = _employeerepository.GetEmployee(id ?? 1);

            HomeDetailsViewModel vm = new HomeDetailsViewModel();
            vm.Employee = model;
            vm.PageTitle = "Rydo App";

            return View("~/Views/Home/Details.cshtml", vm);
        }
    }
}