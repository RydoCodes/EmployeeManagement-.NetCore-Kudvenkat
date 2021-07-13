using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementUsingIdentity.Models;
using EmployeeManagementUsingIdentity.ViewModelsIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementUsingIdentity.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> rydorolemanager;
        private readonly UserManager<ApplicationUser> rydousermanager;

        public AdministrationController(RoleManager<IdentityRole> rydorolemanager, UserManager<ApplicationUser> rydousermanager)
        {
            this.rydorolemanager = rydorolemanager;
            this.rydousermanager = rydousermanager;
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

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await rydorolemanager.FindByIdAsync(id);

            if(role==null)
            {
                ViewBag.ErrorMessage = $"Role with Id {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName= role.Name
            };

            foreach(var user in rydousermanager.Users)
            {
                if(await rydousermanager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel rydomodel)
        {
            var role = await rydorolemanager.FindByIdAsync(rydomodel.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {rydomodel.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = rydomodel.RoleName;
                var result = await rydorolemanager.UpdateAsync(role);
                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(rydomodel);
            }
        }
    }
}