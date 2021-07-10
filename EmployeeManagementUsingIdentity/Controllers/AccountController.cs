using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagementUsingIdentity.ViewModelsIdentity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementUsingIdentity.Controllers
{
    // Authorise Attribute Set at Global Level.
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // We want below actionmethod to respond to only GET and POST Request
        // If we do not specify anything then it will respond to all types of request.
       // [HttpGet] [HttpPost] // Way 1

        [AcceptVerbs("Get","Post")] // Way 2
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email) // ASP NET core remote validation
        {
            var user = await rydoUserManager.FindByEmailAsync(email);
            if(user==null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in user. Contact RydoGear");
            }
        }

        [AllowAnonymous]
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
                    return RedirectToAction("Index", "Home");
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel rydomodel, string returnURL)
        {
            if(ModelState.IsValid)
            {
                var result =  await rydoSignInManager.PasswordSignInAsync(rydomodel.Email, rydomodel.Password, rydomodel.RememberMe, false);

                if(result.Succeeded)
                {
                    //Way 1
                    if(!string.IsNullOrEmpty(returnURL) && Url.IsLocalUrl(returnURL))
                    {
                        return Redirect(returnURL); // This can redirect to non local URLS if you remove the check : Url.IsLocalUrl(returnURL)
                    }
                    // Way 2
                    //if (!string.IsNullOrEmpty(returnURL))
                    //{
                    //    return LocalRedirect(returnURL);
                    //}
                    else
                    {
                        return RedirectToAction("index", "Home"); // If the return URL is an external URL then we will be moved to the index page.
                    }
                    
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