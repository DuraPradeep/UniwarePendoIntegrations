using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class ApiStatus
    {
        public bool SaleOrder_search { get; set; }
        public bool Saleorder_Get { get; set; }
        public bool itemType_get { get; set; }
        public bool Post_delivery_picklist { get; set; }
        public int Failed_Count { get; set; }
        public bool Status { get; set; }
        public bool Trigger { get; set; }
        public string Code { get; set; }
        public String SkuCode { get; set; }
    }
}
