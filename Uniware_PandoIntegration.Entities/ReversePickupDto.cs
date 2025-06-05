using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class ReversePickupDto
    {
        public string ReversePickupCode { get; set; }
        public string TrackingLink { get; set; }
        public string ShippingCourier { get; set; }
        public string ShippingProviderCode { get; set; } = "PANDOREV";
        public string TrackingNumber { get; set; }
    }
}
