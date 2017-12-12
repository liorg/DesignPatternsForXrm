using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Plugin.EmailSend
{
    [DataContract]
    public class ConfigEmail
    {
        [DataMember]
        public bool IsReverse { get; set; }
        [DataMember]
        public string Api { get; set; }
        [DataMember]
        public string Url { get; set; }

        [DataMember]
        public string FromEmail { get; set; }

        [DataMember]
        public string TextChecked { get; set; }

        [DataMember]
        public string ReplyEmail { get; set; }


        [DataMember]
        public string ReplayLabel { get; set; }

        [DataMember]
        public string Log { get; set; }

    }
}
