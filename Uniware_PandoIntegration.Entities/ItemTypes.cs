using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class ItemTypeDTO
    {
        public string Code { get; set; }
        public object tat { get; set; }
        public int? id { get; set; }
        public string skuCode { get; set; }
        public string categoryCode { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string scanIdentifier { get; set; }
        public int? length { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
        public double? weight { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public string brand { get; set; }
        public string ean { get; set; }
        public string upc { get; set; }
        public string isbn { get; set; }
        public object maxRetailPrice { get; set; }
        public object basePrice { get; set; }
        public object batchGroupCode { get; set; }
        public double? costPrice { get; set; }
        public object taxTypeCode { get; set; }
        public string gstTaxTypeCode { get; set; }
        public string hsnCode { get; set; }
        public object imageUrl { get; set; }
        public string productPageUrl { get; set; }
        public string type { get; set; }
        public string scanType { get; set; }
        public object determineExpiryFrom { get; set; }
        public object taxCalculationType { get; set; }
        public bool? requiresCustomization { get; set; }
        public string itemDetailFieldsText { get; set; }
        public bool? enabled { get; set; }
        public List<string> tags { get; set; }
        public object shelfLife { get; set; }
        public object expirable { get; set; }
        public List<object> customFieldValues { get; set; }
        public List<object> componentItemTypes { get; set; }
        public object grnExpiryTolerance { get; set; }
        public object dispatchExpiryTolerance { get; set; }
        public object returnExpiryTolerance { get; set; }
        public int? minOrderSize { get; set; }
        public object expiryDate { get; set; }
        public string skuType { get; set; }
        public string Source { get; set; }
    }

    public class SkuRoot
    {
        public bool successful { get; set; }
        public object message { get; set; }
        public List<object> errors { get; set; }
        public object warnings { get; set; }
        public ItemTypeDTO itemTypeDTO { get; set; }
    }
    


}
