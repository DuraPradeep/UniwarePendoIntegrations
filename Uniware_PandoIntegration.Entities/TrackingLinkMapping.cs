using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class TrackingLinkMapping
    {
        public string CourierName { get; set; }
        public string TrackingLink { get; set; }
    }
    public class TrackingLinkMappingMap
    {
       public List<TrackingLinkMapping> TrackingLinkMappings{ get; set; }
        public string Enviornment { get; set; }
        public string Userid { get; set; }

    }
}
