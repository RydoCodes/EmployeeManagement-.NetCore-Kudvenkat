using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingIdentity.Models
{
    // IdentityUser : An Identity class that we get from .NET Core Identity.
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
