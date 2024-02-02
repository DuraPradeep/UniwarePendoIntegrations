using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class RegionMaster
    {
        public string State { get; set; }
        public string Region { get; set; }
    }
    public class RegionMasterMap
    {
        public List<RegionMaster> RegionMasters { get; set; }
        public string Enviornment { get; set; }
        public string Userid { get; set; }
    }
}
