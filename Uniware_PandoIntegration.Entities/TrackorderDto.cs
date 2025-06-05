using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class TrackOrderDto
    {
        public string OrderID { get; set; }
        public string OrderDate { get; set; }
        public int ItemCount { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
    public class OrderItemDto
    {
        public string ProductName { get; set; }
        public string TrackingStatus { get; set; }
        public string TrackingLink { get; set; }
    }
}
