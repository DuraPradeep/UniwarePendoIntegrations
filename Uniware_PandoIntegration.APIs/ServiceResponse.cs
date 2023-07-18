using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Uniware_PandoIntegration.APIs
{
    public class ServiceResponse<T>
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the errcode.
        /// </summary>
        [DataMember]
        public int Errcode { get; set; }

        /// <summary>
        /// Gets or sets the errdesc.
        /// </summary>
        [DataMember]
        public string Errdesc { get; set; }

        /// <summary>
        /// Gets or sets the object param.
        /// </summary>
        [DataMember]
        public T ObjectParam { get; set; }

        /// <summary>
        /// Gets or sets the additional param.
        /// </summary>
        [DataMember]
        public string AdditionalParam { get; set; }

        /// <summary>
        /// Gets or sets the is confirmation id.
        /// </summary>
        [DataMember]
        public int IsConfirmationId { get; set; }
        [DataMember]
        public bool IsSuccess { get; set; }
        [DataMember]
        public string Message { get; set; }

        //public ServiceResponse()
        //{
        //    Id = 0;
        //    Errdesc = string.Empty;
        //    Errcode = 0;
        //    ObjectParam =null;
        //    AdditionalParam = string.Empty;
        //}
    }
}
