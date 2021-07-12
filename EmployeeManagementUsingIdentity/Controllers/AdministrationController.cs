using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementUsingIdentity.ViewModelsIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementUsingIdentity.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> rydorolemanager;

        public AdministrationController(RoleManager<IdentityRole> rydorolemanager)
        {
            this.rydorolemanager = rydorolemanager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel rydomodel)
        {
            if(ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name= rydomodel.RoleName
                };

                IdentityResult result =  await rydorolemanager.CreateAsync(identityRole);

                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }
                
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = rydorolemanager.Roles;
            return View(roles);
        }
    }
}