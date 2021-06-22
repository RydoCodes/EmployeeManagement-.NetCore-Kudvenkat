using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementUsingDB.Model;
using EmployeeManagementUsingDB.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        public ViewResult Details(int? id)
        {
            Employee rydoemployee = _employeeRepository.GetEmployee(id?? 4); // If the id passed is empty then by default employee with id 1 is retrieved.

            if(rydoemployee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                employee = rydoemployee,
                PageTitle = "Employee Details page"
                
            };

            return View(homeDetailsViewModel);
        }

        public ViewResult Create()
        {
            return View();
        }

        public ViewResult Edit(int id)
        {
            Employee emp = _employeeRepository.GetEmployee(id);

            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel()
            {
                Id = emp.Id,
                Name = emp.Name,
                Email = emp.Email,
                Department = emp.Department,
                ExistingPhotoPath = emp.PhotoPat
            };

            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);
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


        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            Employee rydoemp = _employeeRepository.GetEmployee(model.Id);
            rydoemp.Name = model.Name;
            rydoemp.Department = model.Department;
            rydoemp.Email = model.Email;

            if (ModelState.IsValid)
            {
                if(model.Photos!=null)
                {
                    // Meaning Employee already has chosen a photo while creation.
                    if(model.ExistingPhotoPath!=null)
                    { 
                        var filepath = Path.Combine(_environment.WebRootPath,"Images",model.ExistingPhotoPath);
                        System.IO.File.Delete(filepath);
                    }
                    rydoemp.PhotoPat = ProcessUploadedFile(model);
                }

                _employeeRepository.Update(rydoemp);
                return RedirectToAction("Index", new { id = rydoemp.Id });
            }

            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null & model.Photos.Count > 0)
            {
                foreach (IFormFile photo in model.Photos)
                {
                    string path = Path.Combine(_environment.WebRootPath, "Images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string filepath = Path.Combine(path, uniqueFileName);

                    using (var filestream = new FileStream(filepath, FileMode.Create))
                    {
                        photo.CopyTo(filestream);
                    }
                }

            }

            return uniqueFileName;
        }
    }
}