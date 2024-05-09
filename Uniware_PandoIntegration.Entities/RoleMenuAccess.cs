using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class RoleMenuAccess
    {
        public long MenuAccessId { get; set; }
        public short Role { get; set; }
        public int MenuId { get; set; }
        public bool Deleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedIP { get; set; }
        public string flag { get; set; }
        public long UserID { get; set; }
        public short Permission { get; set; }
        public bool CreateId { get; set; }
        public bool ViewId { get; set; }
        public bool UpdateId { get; set; }
        public bool DeleteId { get; set; }
        public bool PrintId { get; set; }
        public bool Approval { get; set; }
        public long HostId { get; set; }
        public string MenuXml { get; set; }
    }
}
