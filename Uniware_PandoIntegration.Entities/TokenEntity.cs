using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.Entities
{
    public class TokenEntity
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
    //public class Root
    //{
    //    public string username { get; set; }
    //    public string password { get; set; }
    //}

}
