using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class ReqSalesOrderSearch
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string dateType { get; set; }
    }
}
