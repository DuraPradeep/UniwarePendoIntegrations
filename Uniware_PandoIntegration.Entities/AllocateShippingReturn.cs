using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class AllocateShippingReturn
    {
        public string shippingPackageCode { get; set; }
        public string shippingLabelMandatory { get; set; }
        public string shippingProviderCode { get; set; }
        public string shippingCourier { get; set; }
        public string trackingNumber { get; set; }
        //public string trackingLink { get; set; }
        public string tracking_link_url { get; set; }

    }
    public class ReturnAllocateShippingDb
    {
        public string shippingPackageCode { get; set; }
        public string shippingLabelMandatory { get; set; }
        public string shippingProviderCode { get; set; }
        public string shippingCourier { get; set; }
        public string trackingNumber { get; set; }
        public string FacilityCode { get; set; }
        public string trackingLink { get; set; }
    }
    public class UniwarePostDto
    {
        public string ReversePickupCode { get; set; }
        public string TrackingLink { get; set; }
        public string ShippingCourier { get; set; }
        public string ShippingProviderCode { get; set; }
        public string TrackingNumber { get; set; }
    }
}
