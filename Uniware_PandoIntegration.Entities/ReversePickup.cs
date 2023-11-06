using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class ReversePickup
    {
        public string reversePickupCode { get; set; }
        public string pickupInstruction { get; set; }
        public string trackingLink { get; set; }
        public string shippingCourier { get; set; }
        public string trackingNumber { get; set; }
        public string shippingProviderCode { get; set; }
        public PickUpAddress pickUpAddress { get; set; }
        public Dimension dimension { get; set; }
        public List<CustomField> customFields { get; set; }
    }

    public class CustomField
    {
        public string name { get; set; }
        public string value { get; set; }

    }

    public class Dimension
    {
        public string boxLength { get; set; }
        public string boxWidth { get; set; }
        public string boxHeight { get; set; }
        public string boxWeight { get; set; }
    }

    public class PickUpAddress
    {
        public string id { get; set; }
        public string name { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string phone { get; set; }
        public string pincode { get; set; }
    }
    public class ReversePickupDb
    {
        public string reversePickupCode { get; set; }
        public string CId { get; set; }
        public string pickupInstruction { get; set; }
        public string trackingLink { get; set; }
        public string shippingCourier { get; set; }
        public string trackingNumber { get; set; }
        public string shippingProviderCode { get; set; }
        public PickUpAddress pickUpAddress { get; set; }
        public Dimension dimension { get; set; }
        public List<CustomField> customFields { get; set; }
        public string FaciityCode { get; set; }
    }

    public class CustomFieldDb
    {
        public string name { get; set; }
        public string value { get; set; }
        public string CId { get; set; }

    }

    public class DimensionDb
    {
        public string CId { get; set; }
        public string boxLength { get; set; }
        public string boxWidth { get; set; }
        public string boxHeight { get; set; }
        public string boxWeight { get; set; }
    }

    public class PickUpAddressDb
    {
        public string CId { get; set; }

        public string id { get; set; }
        public string name { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string phone { get; set; }
        public string pincode { get; set; }
    }
    public class ReversePickupResponse
    {
        public bool successful { get; set; }
        public object message { get; set; }
        public object errors { get; set; }
        public object warnings { get; set; }
    }
}
