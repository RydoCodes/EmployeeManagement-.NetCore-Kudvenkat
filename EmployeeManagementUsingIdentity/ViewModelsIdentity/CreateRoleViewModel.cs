using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingIdentity.ViewModelsIdentity
{
    public class CreateRoleViewModel
    {
        [Required]
        [MaxLength(8,ErrorMessage ="RydoGear does not allow you enter more than 8 characters here")]
        public string RoleName { get; set; }
    }
}
