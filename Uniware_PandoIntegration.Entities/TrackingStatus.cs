using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class TrackingStatus
    {
        public string providerCode { get; set; }
        public string trackingNumber { get; set; }
        public string trackingStatus { get; set; }
        public string statusDate { get; set; }
        public string shipmentTrackingStatusName { get; set; }
    }
    public class TrackingStatusDb
    {
        public string Id { get; set; }
        public string providerCode { get; set; }
        public string trackingNumber { get; set; }
        public string trackingStatus { get; set; }
        public string statusDate { get; set; }
        public string shipmentTrackingStatusName { get; set; }
        public string facilitycode { get; set; }
        public string Instance { get; set;}
    }
    public class TrackingResponse
    {
        public bool successful { get; set; }
        public object message { get; set; }
        public object errors { get; set; }
        public object warnings { get; set; }
    }
}
