using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class ReturnUpdateShipping
    {
        public string shippingPackageCode { get; set; }
        public List<CustomFieldValue> customFieldValues { get; set; }
    }
    public class ReturndCustomFieldValue
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string value { get; set; }
    }
}
