using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class CancelResponse
    {
        public string status { get; set; }
        public string waybill { get; set; }
        public string errorMessage { get; set; }
    }
    public class calcelwaybill
    {
        public string waybill { get; set; }
    }
    public class CancelData
    {
        public string indent_no { get; set; }
        public string material_invoice_number { get; set; }
        public string material_code { get; set; }
        public string line_item_no { get; set; }

    }
    
}
