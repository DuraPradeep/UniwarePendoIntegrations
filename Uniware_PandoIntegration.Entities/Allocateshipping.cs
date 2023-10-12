using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class Allocateshipping
    {
        public string shippingPackageCode { get; set; }
        public string shippingLabelMandatory { get; set; }
        public string shippingProviderCode { get; set; }
        public string shippingCourier { get; set; }
        public string trackingNumber { get; set; }
        //public string generateUniwareShippingLabel { get; set; }
    }
    public class AllocateshippingDb
    {
        public string shippingPackageCode { get; set; }
        public string shippingLabelMandatory { get; set; }
        public string shippingProviderCode { get; set; }
        public string shippingCourier { get; set; }
        public string trackingNumber { get; set; }
        public string generateUniwareShippingLabel { get; set; }
        public string FacilityCode { get; set; }
    }
}
