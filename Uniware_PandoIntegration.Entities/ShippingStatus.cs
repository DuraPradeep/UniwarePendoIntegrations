using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class ShippingStatus
    {
        public string StatusName { get; set; }
        public string Instanc { get; set; }
    }
    public class ShippingStatusList
    {
        public List<ShippingStatus> ShippingStatus { get; set; }
        public string Enviornment { get; set; }
        public string UserId { get; set; }
    }
}
