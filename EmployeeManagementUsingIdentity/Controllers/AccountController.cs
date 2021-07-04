using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementUsingIdentity.ViewModelsIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementUsingIdentity.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> rydoUserManager;
        private readonly SignInManager<IdentityUser> rydoSignInManager;

        // Create a new user, using UserManager service provided by asp.net core identity.
        //Sign-in a user using SignInManager service provided by asp.net core identity.
        public AccountController(UserManager<IdentityUser> rydoUserManager, SignInManager<IdentityUser> rydoSignInManager)
        {
            this.rydoUserManager = rydoUserManager;
            this.rydoSignInManager = rydoSignInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel rydomodel)
        {
            if(ModelState.IsValid)
            {
                // We are creating a IdentityUser Type and binding rydomodel to it because rydouserManager takes IdentityUser Type as Generic Parameter.
                var rydouser = new IdentityUser
                {
                    UserName = rydomodel.Email,
                    Email = rydomodel.Email

                };

                var result = await rydoUserManager.CreateAsync(rydouser, rydomodel.Password);

                if(result.Succeeded)
                {
                    await rydoSignInManager.SignInAsync(rydouser, isPersistent: false);
                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(rydomodel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await rydoSignInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel rydomodel)
        {
            if(ModelState.IsValid)
            {
                var result =  await rydoSignInManager.PasswordSignInAsync(rydomodel.Email, rydomodel.Password, rydomodel.RememberMe, false);

                if(result.Succeeded)
                {
                    return RedirectToAction("index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(rydomodel);
        }
    }
}
// Register User   -rydoUserManager.CreateAsync & rydoSignInManager.SignInAsync
// Login User      -rydoSignInManager.PasswordSignInAsync
// Logout User     -rydoSignInManager.SignOutAsync()

// A Session Cookie is immediately lost after we close the browser window
// A Permanent Cookie is retained on the client machine even after the browser window is closed.