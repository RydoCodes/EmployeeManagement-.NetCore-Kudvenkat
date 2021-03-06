using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagementUsingIdentity.RydoUtilities;

namespace EmployeeManagementUsingIdentity.ViewModelsIdentity
{
    public class RegisterViewModel
    {

        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailInUse",controller:"Account")] // ASP NET core remote validation
        [ValidEmailDomain(allowedDomain: "rydogear.com", ErrorMessage ="You should be part of rydogear.com :)")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password",
            ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}
