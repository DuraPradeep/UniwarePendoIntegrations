using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class ErrorCountEntitys
    {
        public List<CodesErrorDetails> SaleorderDetails { get; set; }
        public List<CodesErrorDetails> STOWaybill { get; set; }
        public List<CodesErrorDetails> STOAPI { get; set; }
        public List<EndpointErrorDetails> WaybillError { get; set; }
        public List<EndpointErrorDetails> UpdateShippingError { get; set; }
        public List<EndpointErrorDetails>AllocateShippingError { get; set; }
    }
}
