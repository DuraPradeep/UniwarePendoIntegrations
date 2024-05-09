using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class TDashboardDetails
    {
        public string TrackingNumber { get; set; }
        public string DisplayOrder { get; set; }
        public string ShipmentID { get; set; }
        public string LatestStatus { get; set; }
        public string CourierName { get; set; }
        public string trackingLink { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string FacilityCode { get; set; }
        public string CustomerCity { get; set; }
        public string InvoiceDate { get; set; }
        public string MaterialCode { get; set; }
        public string Quantity { get; set; }
        public string UOM { get; set; }
        public string IndentID { get; set; }
        public string Pincode { get; set; }
        public string state { get; set; }
        public string Region { get; set; }
        public string OrderStatus { get; set; }

        //public List<TrackingDetails> trackingDetails { get; set; }

    }
    public class TrackingDetails
    {
        public string Count { get; set; }
        public string StatusName { get; set; }
    }
    public class DashboardsLists
    {
       public List<TDashboardDetails>dashboardDetails {  get; set; }
        public List<TrackingDetails>trackingDetails { get; set; }
    }
}
