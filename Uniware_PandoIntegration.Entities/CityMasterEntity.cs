using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class CityMasterEntity
    {
        public string ReferenceName { get; set; }
        public string ActualName { get; set; }
    }
    public class CityMasterEntitymap
    {
        public List<CityMasterEntity> cityMasterEntities { get; set; }
        public string Enviornment { get; set; }
        public string Userid { get; set; }
    }
}
