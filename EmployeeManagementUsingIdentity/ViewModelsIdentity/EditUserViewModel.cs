using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementUsingIdentity.ViewModelsIdentity
{
    public class EditUserViewModel
    {
        // We are using the constructor to initiaise the constructor properties so that we do not get NULL REFERENCE Exception Errors
        public EditUserViewModel()
        {
            Claims = new List<String>();
            Roles = new List<String>();
        }

        public string id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }

        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
