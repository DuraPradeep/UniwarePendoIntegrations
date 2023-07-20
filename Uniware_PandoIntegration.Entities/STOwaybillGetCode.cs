using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class STOwaybillGetCode
    {
        public bool successful { get; set; }
        public object message { get; set; }
        public List<object> errors { get; set; }
        public object warnings { get; set; }
        public List<Element> elements { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    

    public partial class Element
    {
        public int id { get; set; }
        //public string code { get; set; }
        //public string statusCode { get; set; }
        //public string type { get; set; }
        public object invoiceCode { get; set; }
        public object invoiceDisplayCode { get; set; }
        public object returnInvoiceCode { get; set; }
        public object returnInvoiceDisplayCode { get; set; }
        public string toPartyName { get; set; }
        public object gatePassOrderCode { get; set; }
        //public string reference { get; set; }
        //public string purpose { get; set; }
        //public string username { get; set; }
        //public long created { get; set; }
        //public long updated { get; set; }
        public List<GatePassItemDTO> gatePassItemDTOs { get; set; }
        //public List<CustomFieldValue> customFieldValues { get; set; }
    }
    public class STOlists
    {
        public List<GatePassItemDTO> gatePassItemDTOs { get; set; }
        public List<Element> elements { get; set; }
    }
    public class GatePassItemDTO
    {
        public string code { get; set; }
        public object gatePassItemId { get; set; }
        public string gatePassItemStatus { get; set; }
        public object itemCode { get; set; }
        public object itemStatus { get; set; }
        public string inventoryType { get; set; }
        public string itemTypeName { get; set; }
        public string itemTypeSKU { get; set; }
        public object itemTypeImageUrl { get; set; }
        public object itemTypePageUrl { get; set; }
        public object inflowReceiptCode { get; set; }
        public object itemCondition { get; set; }
        public object reason { get; set; }
        public int total { get; set; }
        public int unitPrice { get; set; }
        public int taxPercentage { get; set; }
        public int integratedGstPercentage { get; set; }
        public int unionTerritoryGstPercentage { get; set; }
        public int stateGstPercentage { get; set; }
        public int centralGstPercentage { get; set; }
        public int compensationCessPercentage { get; set; }
        public int quantity { get; set; }
        public int receivedQuantity { get; set; }
        public int pendingQuantity { get; set; }
        public string shelfCode { get; set; }
        public string hsnCode { get; set; }
        public object batchDTO { get; set; }
    }

    


}
