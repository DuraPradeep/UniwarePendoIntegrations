using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class Menus
    {
        public int MenuID { get; set; }
        public string MenuTitle { get; set; }
        public string Classname { get; set; }
        public string FontColor { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string OnBeginFun { get; set; }
        public string OnFailureFun { get; set; }
        public string RouteValue { get; set; }
        public string OnSuccessFun { get; set; }
        public int ParentMenu { get; set; }
        public bool Deleted { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedIP { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedIP { get; set; }
        public string Flag { get; set; }
        public string IconImages { get; set; }
        public string ImageSVGs { get; set; }
        public string OnCompleteFun { get; set; }
        public int OrderNo { get; set; }
        public int ModuleID { get; set; }
        public short Permission { get; set; }
        public bool CreateId { get; set; }
        public bool ViewId { get; set; }
        public bool UpdateId { get; set; }
        public bool DeleteId { get; set; }
        public bool PrintId { get; set; }
        public bool Approval { get; set; }
        public long HostId { get; set; }
        public bool IsSelected { get; set; }
    }
}
