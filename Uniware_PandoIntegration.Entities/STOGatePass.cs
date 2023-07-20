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

  


}
