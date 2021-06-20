using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Model;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;

//namespace EmployeeManagement.Controllers
//{
//    [Route("[controller]")]
//    public class TokenControllerActionController : Controller
//    {
//        private IEmployeeRepository _employeerepository;

//        public TokenControllerActionController(IEmployeeRepository employeerepository)
//        {
//            _employeerepository = employeerepository;
//        }
//        [Route("~/")]
//        [Route("")]
//        [Route("[action]")]
//        public ViewResult Index()
//        {
//            //return _employeerepository.GetEmployee(1).Name;  

//            return View("~/Views/Home/Index.cshtml", _employeerepository.GetEmployee());

//        }
//        [Route("[action]/{id?}")]
//        public ViewResult Details(int? id)
//        {
//            Employee model = _employeerepository.GetEmployee(id ?? 1);

//            HomeDetailsViewModel vm = new HomeDetailsViewModel();
//            vm.Employee = model;
//            vm.PageTitle = "Rydo App";

//            return View("~/Views/Home/Details.cshtml", vm);
//        }
//    }
//}


namespace EmployeeManagement.Controllers
{
    [Route("[controller]/[action]")]
    public class TokenControllerActionController : Controller
    {
        private IEmployeeRepository _employeerepository;

        public TokenControllerActionController(IEmployeeRepository employeerepository)
        {
            _employeerepository = employeerepository;
        }
        [Route("~/TokenControllerAction")]
        //[Route("~/")]
        public ViewResult Index()
        {
            //return _employeerepository.GetEmployee(1).Name;  

            return View("~/Views/Home/Index.cshtml", _employeerepository.GetEmployee());

        }
        [Route("{id?}")]
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
