using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class FTLShipment
    {
        public string shipment_id { get; set; }
        public string transporter_ref_id { get; set; }
        public string transporter_name { get; set; }
        public string delivery_type { get; set; }
        public string shipment_status { get; set; }
        public List<Shipment> shipments { get; set; }
    }
    public partial class Shipment
    {
        public List<string> delivery_numbers { get; set; }
        public List<string> business_divisions { get; set; }
        public string ship_to_ref_id { get; set; }
        public string ship_to_type { get; set; }
        public string sold_to_ref_id { get; set; }
        public string sold_to_type { get; set; }
        public string pick_up_ref_id { get; set; }
        public string pick_up_type { get; set; }
        public string pod_available { get; set; }
        public List<object> pod_attachments { get; set; }
        public string lr_number { get; set; }
        public string consignment_number { get; set; }
        public string tracking_link { get; set; }
    }
    public class FTLShipmentResponse
    {
        public bool Status { get; set; }
        public string Reason { get; set; }
        public string Message { get; set; }
    }
}
