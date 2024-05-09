using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class TrackingMaster
    {
        public string PandoStatus { get; set; }
        public string UniwareStatus { get; set; }
        public string CourierName { get; set; }
    }
    public class TrackingMasterMapping
    {
        public List<TrackingMaster> TrackingMasters { get; set; }
        public string Enviornment{ get; set; }
        public string Userid { get; set; }
    }
}
