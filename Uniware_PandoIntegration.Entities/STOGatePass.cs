using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class STOGatePass
    {
        public bool successful { get; set; }
        public object message { get; set; }
        public List<object> errors { get; set; }
        public object warnings { get; set; }
        public object totalRecords { get; set; }
        public List<Element> elements { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public partial class CustomFieldValue
    {
        public string fieldName { get; set; }
        public string fieldValue { get; set; }
        public string valueType { get; set; }
        public string displayName { get; set; }
        public bool required { get; set; }
        public object possibleValues { get; set; }
    }

    public partial class Element
    {
        public string username { get; set; }
        //public string code { get; set; }
        public string type { get; set; }
        public string statusCode { get; set; }
        //public long created { get; set; }
        public string toParty { get; set; }
        public string reference { get; set; }
        public List<CustomFieldValue> customFieldValues { get; set; }
    }
    public class PostDataSTOWaybill
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
        public string clear { get; set; }
       
    }




}
