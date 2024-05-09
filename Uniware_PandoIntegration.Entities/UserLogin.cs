using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class UserLogin
    {
        public string LoginID { get; set; }
        public string UserName { get; set; }
        public string Password{ get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string PhoneNumber { get; set; }
        public string Environment { get; set; }

    }
}
