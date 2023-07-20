using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class WaybillSTOItemtypeDTO
    {
        public bool successful { get; set; }
        public object message { get; set; }
        public List<object> errors { get; set; }
        public object warnings { get; set; }
        public ItemTypeDTO itemTypeDTO { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
   

   


}
