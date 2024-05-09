using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class SaleOrderSearch
    {
        public string code { get; set; }
       public string displayOrderCode { get; set; }
        public string channel { get; set; }
        public string source { get; set; }
        public string displayOrderDateTime { get; set; }
        public string status { get; set; }
        public string created { get; set; }
        public string notificationEmail { get; set; }
        public string updated { get; set; }
        public string notificationMobile { get; set; }
        public string fulfillmentTat { get; set; }
        public string customerGSTIN { get; set; }
        public string channelProcessingTime { get; set; }            
   
    }
    public class Rootsearch
    {
        public SaleOrderSearch[] SaleOrderSearch { get; set; }
    }
    public class item
    {
        public string quentity { get; set; }

    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public partial class Element
    {
        public string code { get; set; }
        public string displayOrderCode { get; set; }
        public string channel { get; set; }
        public string source { get; set; }
        public object displayOrderDateTime { get; set; }
        public string status { get; set; }
        public object created { get; set; }
        public object updated { get; set; }
        public object fulfillmentTat { get; set; }
        public string notificationEmail { get; set; }
        public string notificationMobile { get; set; }
        public string customerGSTIN { get; set; }
        public object channelProcessingTime { get; set; }
    }

    public class Root
    {
        
        public object totalRecords { get; set; }
        public List<Element> elements { get; set; }
    }

    public class Salesorder
    {
        public string code { get; set; }
        public string Instance { get; set; }
    }
    public class SKucode
    {
        public string SkuCode { get; set; }
        public string code { get; set; }
    }
    public class skucode
    {
        public string skuCode { get; set; }
    }
}
