using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Plugin.EmailSend
{
    public class AgentInwiseEmailSubmit : IAgentSend
    {
        public void Send(ITracingService log, ConfigEmail config, EmailModel emailModel)
        {
            using (MemoryStream stream1 = new MemoryStream())
            {
                string response = "", json="";
                try
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(EmailModel));
                    ser.WriteObject(stream1, emailModel);
                    stream1.Position = 0;
                    StreamReader sr = new StreamReader(stream1);
                    json = sr.ReadToEnd();
                   
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(config.Url);
                    request.Method = "POST";
                    request.Headers.Add("x-api-key", config.Api);
                    request.ContentType = "application/json";
                    request.Accept = "application/json";
              //      request.KeepAlive = true;
                    request.ContentLength = Encoding.UTF8.GetByteCount(json);
                //    request.SendChunked = true;
                    UTF8Encoding encoding = new UTF8Encoding();
                    request.AllowWriteStreamBuffering = true;
                    byte[] bytes = encoding.GetBytes(json);
                    using (Stream writeStream = request.GetRequestStream())
                    {
                          writeStream.Write(bytes, 0, bytes.Length);
                    }
                  
                    using (WebResponse webResponse = request.GetResponse())
                    using (Stream webStream = webResponse.GetResponseStream())
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        response = responseReader.ReadToEnd();
                        Console.Out.WriteLine(response);
                        responseReader.Close();
                    }
                    log.Trace("inwise::request={0} ,resposne={1}", json,response);
                }

                catch (Exception e1)
                {
                 
                    throw new InvalidPluginExecutionException(e1.Message);
                }
            }
        }
        
    }
}


