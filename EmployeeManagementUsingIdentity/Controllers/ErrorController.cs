using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagementUsingIdentity.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> rydologger;

        public ErrorController(ILogger<ErrorController> rydologger)
        {
            this.rydologger = rydologger;
        }

        // This is route is set for invalid URLS : handled by middleware : app.UseStatusCodePagesWithReExecute("/ErrorController/{0}");
        [Route("/ErrorController/{statuscode}")]
        public IActionResult NotFound(int statuscode)
        {
            var statuscoderesult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch(statuscode)
            {
                case 404 :
                    ViewBag.ErrorMessage = "Sorry the Path cannot be found in RydoApp";
                    rydologger.LogWarning($"404 Error Occurred. This Path {statuscoderesult.OriginalPath} with Query String Parameters {statuscoderesult.OriginalQueryString} was not available");
                    break;
            }

            return View();
        }

        // This is route is set incase there is any error occured at statuscoderesult.path : handled by middleware :  app.UseExceptionHandler("/Error");
        [AllowAnonymous]
        [Route("ErrorController")]
        public IActionResult Error()
        {
            var statuscoderesult = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.ErrorMessage = "An Error Occured in RydoApp";
            rydologger.LogWarning($"Error occured at path {statuscoderesult.Path}"
                                + $"with Error details as {statuscoderesult.Error.Message} and stack trace {statuscoderesult.Error.StackTrace}");

            return View();
        }
    }
}