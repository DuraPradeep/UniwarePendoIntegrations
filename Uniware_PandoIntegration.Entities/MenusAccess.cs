using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class MenusAccess
    {
        public List<RoleMenuAccess> RoleMenuAccessesList { get; set; }
        public List<Menus> MenusList { get; set; }
        //public SelectList MenuLists { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int UserTypeId { get; set; }
        public int FilterDepartmentId { get; set; }
        public int FilterRoleId { get; set; }
        public string FilterState { get; set; }
        public string FilterZone { get; set; }
        public string MenuXml { get; set; }
        public string WorkflowName { get; set; }
        //public SelectList RoleList { get; set; }
        //public SelectList DepartmentList { get; set; }
        //public SelectList UserList { get; set; }
    }
}
