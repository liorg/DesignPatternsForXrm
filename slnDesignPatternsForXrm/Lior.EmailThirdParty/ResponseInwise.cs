using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Plugin.EmailSend
{
    /*
     [
    {
        "email": "lior_gr@malam.com",
        "status": "queued",
        "reject_reason": "insufficient-funds",
        "transaction_id": "84dc2493e461455998bf5dc34f327a98",
        "code": null,
        "name": null
    }
]
     */
  
    [DataContract]
    public class InwiseItem
    {
        [DataMember]
        public string email { get; set; }

        [DataMember]
        public string status { get; set; }

        [DataMember]
        public string reject_reason { get; set; }

        [DataMember]
        public string transaction_id { get; set; }

        [DataMember]
        public string code { get; set; }

        [DataMember]
        public string name { get; set; }
    }
}
