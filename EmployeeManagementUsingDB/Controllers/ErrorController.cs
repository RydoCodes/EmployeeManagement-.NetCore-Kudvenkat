using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingDB.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("/ErrorInformationonViewPage/{statuscode}")]
        public IActionResult HttpStatusCodeHandlerusingViewPage(int statuscode)
        {
            var statuscodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statuscode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry The Resource cannot be found";
                    ViewBag.Path = statuscodeResult.OriginalPath;
                    ViewBag.QS = statuscodeResult.OriginalQueryString;
                    break;
            }

            return View("NotFound");
        }

        [Route("/ErrorInformationusingILogger/{statuscode}")]
        public IActionResult HttpStatusCodeHandlerusingILogger(int statuscode)
        {
            var statuscodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statuscode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry The Resource cannot be found";
                    logger.LogWarning($"404 Error Occured. Path = {statuscodeResult.OriginalPath}" + $"and Query String = {statuscodeResult.OriginalQueryString}" );
                    break;
            }

            return View("NotFound");
        }


        [Route("ErrorInformationonViewPage")]
        [AllowAnonymous]
        public IActionResult ErrorInformationonViewPage()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.ExceptionPath = exceptionDetails.Path;
            ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            ViewBag.StackTrace = exceptionDetails.Error.StackTrace;

            return View("Error");
        }

        [AllowAnonymous]
        [Route("ErrorInformationusingILogger")]
        public IActionResult ErrorInformationusingILogger()
        {
            var exceptiondetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            logger.LogError($"The path {exceptiondetails.Path} thew an exception" + 
                $"{exceptiondetails.Error}");

            return View("Error");
        }

    }
}
