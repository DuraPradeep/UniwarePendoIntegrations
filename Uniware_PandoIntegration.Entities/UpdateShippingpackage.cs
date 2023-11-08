using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class UpdateShippingpackage
    {
        public string shippingPackageCode { get; set; }
      
        //public string shippingProviderCode { get; set; }
        //public string trackingNumber { get; set; }
        //public string shippingPackageTypeCode { get; set; }
        //public int? actualWeight { get; set; }
        ////public List<ShippingBox> shippingBox { get; set; }
        //public ShippingBox shippingBox { get; set; }
        //public int? noOfBoxes { get; set; }
        public List<CustomFieldValue> customFieldValues { get; set; }
    }
    
    public class UpdateShippingpackageFacility
    {
        public string shippingPackageCode { get; set; }

        public string shippingProviderCode { get; set; }
        public string trackingNumber { get; set; }
        public string shippingPackageTypeCode { get; set; }
        public int? actualWeight { get; set; }
        //public List<ShippingBox> shippingBox { get; set; }
        public ShippingBox shippingBox { get; set; }
        public int? noOfBoxes { get; set; }
        public List<CustomFieldValue> customFieldValues { get; set; }
    }
    public class ShippingBox
    {
        
        public int? length { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
    }
    public class UpdateShippingpackagedb
    {
        public string shippingPackageCode { get; set; }
        public string id { get; set; }
        //public string shippingProviderCode { get; set; }
        //public string trackingNumber { get; set; }
        //public string shippingPackageTypeCode { get; set; }
        //public int? actualWeight { get; set; }
        ////public List<ShippingBox> shippingBox { get; set; }
        //public ShippingBox shippingBox { get; set; }
        //public int? noOfBoxes { get; set; }
        public List<CustomFieldValue> customFieldValues { get; set; }
        public string FacilityCode { get; set; }
    }
    public class ShippingBoxdb
    {
        public string Id { get; set; }
        public int? length { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
    }


}
