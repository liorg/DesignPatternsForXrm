using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Plugin.EmailSend
{
    //https://www.inwise.co.il/%D7%9E%D7%A4%D7%AA%D7%97%D7%99%D7%9D/%D7%93%D7%95%D7%92%D7%9E%D7%90%D7%95%D7%AA-%D7%A7%D7%95%D7%93-rest-api/
    //https://api.inwise.com/rest/v1/docs/index?_ga=2.47792679.140714012.1512898225-1418904879.1512645273#!/transactional_emails/transactionalEmails_sendTemplate

        [DataContract]
    public class EmailModel
    {
        [DataMember]
        public string api_key { get; set; }
          
        [DataMember]
        public Message message { get; set; }

    }
    [DataContract]
    public class Message
    {
          
        [DataMember]
        public string from_email { get; set; }
          
        [DataMember]
        public string html { get; set; }
          
        [DataMember]
        public string subject { get; set; }

        [DataMember]
        public List<to> to { get; set; }


    }
    [DataContract]
    public class to
    { 
        //   ""email"": "YYY@inwise.com",

        //""name"": ""YYY"",

        //""type"": ""to"" // Could be to/cc/bcc
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string type { get; set; }

        [DataMember]
        public string name { get; set; }
       

    }

}
