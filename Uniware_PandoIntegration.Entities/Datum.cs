using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Data
    {
        public string name { get; set; }
        public string reference_number { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string region { get; set; }
        public string mobile_number { get; set; }
        public string email { get; set; }
        public string customer_type { get; set; }
        public string category { get; set; }
        public string cust_category { get; set; }
        public string delivery_number { get; set; }
        public string mrp_price { get; set; }
        public string material_code { get; set; }
        public string source_system { get; set; }
        public string material_taxable_amount { get; set; }
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
        public string line_item_no { get; set; }
        public string pickup_reference_number { get; set; }
        public string cust_refid { get; set; }
    }

    public class sendRoot
    {

        public List<Data> data { get; set; }
    }


}
