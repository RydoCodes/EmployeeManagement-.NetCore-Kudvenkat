using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementUsingIdentity.Models;
using EmployeeManagementUsingIdentity.Models.Services;
using EmployeeManagementUsingIdentity.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementUsingIdentity.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository rydorepo;
        private readonly IWebHostEnvironment rydoenv;
        private readonly ILogger<HomeController> rydolog;

        public HomeController(IEmployeeRepository rydorepo, IWebHostEnvironment rydoenv,ILogger<HomeController> rydolog)
        {
            this.rydorepo = rydorepo;
            this.rydoenv = rydoenv;
            this.rydolog = rydolog;
        }

        [Route("~/")]
        public IActionResult Index()
        {
            IEnumerable<Employee> rydoemplist = rydorepo.GetEmployee();
            return View(rydoemplist);
        }

        public IActionResult Details(int? Id)
        {
            throw new Exception();
            Employee emp = rydorepo.GetEmployee(Id ?? 1);
            return View(emp);
        }

        public IActionResult Delete(int? Id)
        {
            rydorepo.Delete(Id ?? 1);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Employee emp = rydorepo.GetEmployee(Id);

            EmployeeEditViewModel evm = new EmployeeEditViewModel
            {
                Id = emp.Id,
                Name=emp.Name,
                Email=emp.Email,
                Department =emp.Department,
                ExistingPhotoPath = emp.PhotoPath
            };

            return View(evm);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel rydomodel)
        {
            Employee rydoemp = rydorepo.GetEmployee(rydomodel.Id);

            rydoemp.Name = rydomodel.Name;
            rydoemp.Email = rydomodel.Email;
            rydoemp.Department = rydomodel.Department;

            if (ModelState.IsValid)
            {
                if(rydomodel.ExistingPhotoPath!=null)
                {
                    var path = Path.Combine(rydoenv.WebRootPath, "Images", rydomodel.ExistingPhotoPath);
                    System.IO.File.Delete(path);

                    string newpath = ProcessFileNameandStorage(rydomodel);
                    rydoemp.PhotoPath = newpath;
                }
            }

            Employee rydoeeditemp = rydorepo.Update(rydoemp);

            return RedirectToAction("Details", new { Id = rydoeeditemp.Id });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel employee)
        {

            if(ModelState.IsValid)
            {

                string filename = ProcessFileNameandStorage(employee);

                Employee emp = new Employee()
                {
                    Name = employee.Name,
                    Email = employee.Email,
                    Department = employee.Department,
                    PhotoPath = filename

                };

                rydorepo.AddEmployee(emp);
                return RedirectToAction("Details", new { id= emp.Id });
            }

            return View();


        }

        private string ProcessFileNameandStorage(EmployeeCreateViewModel model)
        {
            string uniquefilename = null;

            if(model.Photos!=null && model.Photos.Count>0)
            {
                foreach(IFormFile photo in model.Photos)
                {
                    string concretepath = Path.Combine(rydoenv.WebRootPath, "Images"); // C:\Users\vaibhav.maurya\Downloads\Projects\kudvenkat\.Net Core\EmployeeManagement\EmployeeManagementUsingIdentity\wwwroot\Images
                    uniquefilename = Guid.NewGuid().ToString() + "_" + photo.FileName; // Example 4ec13865-dc44-47a6-a75f-65c35be826a5_vaibhav.png
                    string filepath = Path.Combine(concretepath, uniquefilename); // C:\Users\vaibhav.maurya\Downloads\Projects\kudvenkat\.Net Core\EmployeeManagement\EmployeeManagementUsingIdentity\wwwroot\Images\e8bb2557-62a5-4e29-9a1f-e38b113e6b6e_Sunny1.jpg

                    using (var filestream = new FileStream(filepath, FileMode.Create))
                    {
                        photo.CopyTo(filestream);
                    }
                }
            }

            return uniquefilename;
        }
    }
}