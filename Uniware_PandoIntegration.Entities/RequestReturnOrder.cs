using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class RequestReturnOrder
    {
        public string returnType { get; set; }
        public string statusCode { get; set; }
        public string createdTo { get; set; }
        public string createdFrom { get; set; }
    }
}
