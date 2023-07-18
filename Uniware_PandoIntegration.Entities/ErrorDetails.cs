using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class ErrorDetails
    {
        public string Code { get; set; }
        public string SkuCode { get; set; }
        public bool Status { get; set; }
        public string Reason { get; set; }
    }
}
