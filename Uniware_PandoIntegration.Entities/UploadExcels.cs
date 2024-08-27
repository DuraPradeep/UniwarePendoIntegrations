using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class UploadExcels
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Instance { get; set; }
        public string Facility { get; set; }

    }
    public class MainClass
    {
        public List<UploadExcels> UploadExcels { get; set; }
        public string Enviornment { get; set; }
        public string Userid { get; set; }
    }
    public class UploadElements
    {
        public string code { get; set; }
        public string source { get; set; }
        public string Facility { get; set; }
    }
}
