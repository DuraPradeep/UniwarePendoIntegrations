using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class DashboardStatusMasterEntity
    {
        public string TrackingStatus { get; set; }
        public string DashboardStatus { get; set; }
    }
    public class DashboardStatusMasterEntityMap
    {
        public List<DashboardStatusMasterEntity> dashboardStatusMasterEntities { get; set; }
        public string Enviornment { get; set; }
        public string Userid { get; set; }
    }
}
