using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class WaybillSend
    {
        public string indent_no { get; set; }
        public string delivery_number { get; set; }
        public string mrp_price { get; set; }
        public string material_code { get; set; }
        public string actual_source { get; set; }
        public string source_system { get; set; }
        public string gate_ref_id { get; set; }
        public string division { get; set; }
        public string quantity { get; set; }
        public string quantity_unit { get; set; }
        public string weight { get; set; }
        public string weight_unit { get; set; }
        public string volume { get; set; }
        public string volume_unit { get; set; }
        public string ship_to { get; set; }
        public string sold_to { get; set; }
        public string type { get; set; }
        public string invoice_number { get; set; }
        public string invoice_amount { get; set; }
        public string category { get; set; }
        public string invoice_date { get; set; }
        public string line_item_no { get; set; }
        public string eway_bill_number { get; set; }
        public string eway_bill_date { get; set; }
        public string action_by { get; set; }
        public string action_type { get; set; }
        public string clear { get; set; }
    }
}
