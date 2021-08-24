using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EmployeeManagementUsingIdentity.Models;
using EmployeeManagementUsingIdentity.ViewModelsIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementUsingIdentity.Controllers
{
    //[Authorize(Roles = "Admin")] // Role Based Authorization
    [Authorize(Policy = "RydoAdminRolePolicy")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> rydorolemanager;
        private readonly UserManager<ApplicationUser> rydousermanager;
        private readonly ILogger<AdministrationController> rydologger;

        public AdministrationController(RoleManager<IdentityRole> rydorolemanager, UserManager<ApplicationUser> rydousermanager, ILogger<AdministrationController> rydologger)
        {
            this.rydorolemanager = rydorolemanager;
            this.rydousermanager = rydousermanager;
            this.rydologger = rydologger;
        }

        #region Users


        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = rydousermanager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var rydouser = await rydousermanager.FindByIdAsync(id);

            if (rydouser == null)
            {
                ViewBag.ErrorMessage = $"User with {id} not Found";
                return View("Not Found");
            }


            var rydouserClaims = await rydousermanager.GetClaimsAsync(rydouser); // This returns us the list of all claims of the user that is passed as parameter.
            var rydouserRoles = await rydousermanager.GetRolesAsync(rydouser); // returns list of all roles associated with this user.

            EditUserViewModel rydoevm = new EditUserViewModel()
            {
                id = rydouser.Id,
                UserName = rydouser.UserName,
                City = rydouser.City,
                Email = rydouser.Email,
                Claims = rydouserClaims.Select(c => c.Type +":"+ c.Value).ToList(), // You wantt to get a List of value property of class Claims.
                Roles = rydouserRoles

            };

            return View(rydoevm);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel rydomodel)
        {
            var rydouser = await rydousermanager.FindByIdAsync(rydomodel.id);

            if (rydouser == null)
            {
                ViewBag.ErrorMessage = $"User with {rydomodel.id} not Found";
                return View("Not Found");
            }
            else
            {
                rydouser.Email = rydomodel.Email;
                rydouser.City = rydomodel.City;
                rydouser.UserName = rydomodel.UserName;

                var result = await rydousermanager.UpdateAsync(rydouser);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(rydomodel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var rydouser = await rydousermanager.FindByIdAsync(id);

            if (rydouser == null)
            {
                ViewBag.ErrorMessage = $"User with {id} not Found";
                return View("Not Found");
            }

            var result = await rydousermanager.DeleteAsync(rydouser);

            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUsers");
            }

        }

        #endregion

        #region Roles

        [Route("~/")]
        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = rydorolemanager.Roles;
            return View(roles);
        }



        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel rydomodel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = rydomodel.RoleName
                };

                IdentityResult result = await rydorolemanager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "RydoEditRoleClaimPolicy")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await rydorolemanager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {id} cannot be found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
                Users = new List<string>()
            };

            foreach (var user in rydousermanager.Users)
            {
                if (await rydousermanager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "RydoEditRoleClaimPolicy")]
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
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListRoles");
            }
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRoleRydoClaimPolicy")] 
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await rydorolemanager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await rydorolemanager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListRoles");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View("ListRoles");
                }
                catch (DbUpdateException ex)
                {
                    rydologger.LogError($"Error deleting role{ex.ToString()}");

                    ViewBag.ErrorTitle = $"{role.Name} is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role. If you want to delete this role, please remove the users from the role and then try to delete";
                    return View("~/Views/Error/Error.cshtml");
                }
            }
        }

        #endregion

        #region Edit & Manage UsersInRole
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.RoleId = roleId;

            var role = await rydorolemanager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} is not found";
                return View("NotFound");
            }

            var rydomodelList = new List<UserRoleViewModel>();

            foreach (var user in rydousermanager.Users)
            {
                var userroleviewmodel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    Username = user.UserName
                };

                if (await rydousermanager.IsInRoleAsync(user, role.Name))
                {
                    userroleviewmodel.IsSelected = true;
                }
                else
                {
                    userroleviewmodel.IsSelected = false;
                }

                rydomodelList.Add(userroleviewmodel);
            }

            return View(rydomodelList);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> lstrydomodel, string roleId)
        {
            var role = await rydorolemanager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} is not found";
                return View("NotFound");
            }

            for (int i = 0; i < lstrydomodel.Count; i++)
            {
                var user = await rydousermanager.FindByIdAsync(lstrydomodel[i].UserId);

                IdentityResult result = null;

                if (lstrydomodel[i].IsSelected && !(await rydousermanager.IsInRoleAsync(user, role.Name))) // if you selected a user and user is not in role then add that user to role
                {
                    result = await rydousermanager.AddToRoleAsync(user, role.Name);
                }
                else if (!lstrydomodel[i].IsSelected && (await rydousermanager.IsInRoleAsync(user, role.Name)))
                {
                    result = await rydousermanager.RemoveFromRoleAsync(user, role.Name);
                }

            }


            return RedirectToAction("EditRole", new { Id = roleId });
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {

            ViewBag.userId = userId;

            var rydouser = await rydousermanager.FindByIdAsync(userId);

            if (rydouser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            var rydomodel = new List<UserRolesViewModel>();

            foreach (var rydorole in rydorolemanager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel()
                {
                    RoleId = rydorole.Id,
                    RoleName = rydorole.Name
                };

                userRolesViewModel.IsSelected = await rydousermanager.IsInRoleAsync(rydouser, rydorole.Name) ? true : false;

                rydomodel.Add(userRolesViewModel);
            }

            return View(rydomodel);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> rydomodel, string userId)
        {
            var rydouser = await rydousermanager.FindByIdAsync(userId);

            if (rydouser == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            // Get all Roles related to that user
            var roles = await rydousermanager.GetRolesAsync(rydouser);

            // Remove all roles from that user
            var result = await rydousermanager.RemoveFromRolesAsync(rydouser, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot remove existing roles");
                return View(rydomodel);
            }
            // Re Adding those roles which are now selected

            var newroles = rydomodel.Where(rx => rx.IsSelected).Select(ry => ry.RoleName);

            result = await rydousermanager.AddToRolesAsync(rydouser, newroles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "cannot add selected roles to the user");
                return View(rydomodel);
            }

            return RedirectToAction("EditUser", new { Id = userId });
        }
        #endregion

        #region Manager UserClaims

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var rydouser = await rydousermanager.FindByIdAsync(userId);

            if(rydouser==null)
            {
                ViewBag.ErrorMessage = $"User with UserId {userId} cannot be found";
                return View("NotFound");
            }

            var existingUserClaims = await rydousermanager.GetClaimsAsync(rydouser);

            var rydomodel = new UserClaimsViewModel
            {
                UserId=userId
            };

            foreach(Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim userclaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                if(existingUserClaims.Any(rx=>rx.Type== claim.Type && rx.Value=="true"))
                {
                    userclaim.IsSelected = true;
                }

                rydomodel.Claims.Add(userclaim);
            }

            return View(rydomodel);

        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel rydomodel)
        {
            var rydouser = await rydousermanager.FindByIdAsync(rydomodel.UserId);

            if (rydouser == null)
            {
                ViewBag.ErrorMessage = $"User with UserId {rydomodel.UserId} cannot be found";
                return View("NotFound");
            }

            var rydoclaims = await rydousermanager.GetClaimsAsync(rydouser);
            var result = await rydousermanager.RemoveClaimsAsync(rydouser, rydoclaims);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot Remove Existing Claims");
            }

            IEnumerable<UserClaim> userclaims = rydomodel.Claims;
            IEnumerable<Claim> claimstoadd = userclaims.Select(s => new Claim(s.ClaimType, s.IsSelected ? "true":"false" ));

            result = await rydousermanager.AddClaimsAsync(rydouser, claimstoadd);

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Cannot add selected Claims to user");
                return View(rydomodel);
            }

            return RedirectToAction("EditUser", new { Id = rydomodel.UserId });

        }

        #endregion

    }
}