using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class TruckDetails
    {
        public string Details { get; set; }
        public string Instance { get; set; }
    }
    public class TruckdetailsMap
    {
        public List<TruckDetails> TruckDetails { get; set; }
        public string Enviornment { get; set; }
    }
}
