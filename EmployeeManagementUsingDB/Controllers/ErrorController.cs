using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingDB.Controllers
{
    public class ErrorController : Controller
    {
        [Route("/Error/{statuscode}")]
        public IActionResult HttpStatusCodeHandler(int statuscode)
        {
            switch (statuscode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Sorry The Resource cannot be found";
                    break;
            }

            return View("NotFound");
        }   
    }
}
