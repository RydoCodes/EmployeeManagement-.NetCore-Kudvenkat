using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementUsingDB.Model;
using EmployeeManagementUsingDB.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementUsingDB.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment _environment;

        public HomeController(IEmployeeRepository employeeRepository,IWebHostEnvironment environment)
        {
            _employeeRepository = employeeRepository;
            _environment = environment;
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

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                employee = model,
                PageTitle= "Employee Details page"
                
            };

            return View(homeDetailsViewModel);
        }

        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                //Saving Photo to Images Folder
                if(model.Photo!=null)
                {
                    string path = Path.Combine(_environment.WebRootPath, "Images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;

                    string filepath = Path.Combine(path,uniqueFileName);

                    model.Photo.CopyTo(new FileStream(filepath,FileMode.Create));
                }

                Employee newemployee = new Employee()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPat = uniqueFileName
                };

                _employeeRepository.AddEmployee(newemployee);
                return RedirectToAction("Details", new { id = newemployee.Id });
            }

            return View();

        }
    }
}