using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace Uniware_PandoIntegration.Entities
{
    public class UserProfile
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int Roleid { get; set; }
        public string RoleName { get; set; }
        public SelectList RoleNameList { get; set; }
        public string Environment {  get; set; }
    }
}
