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
    public class AgentEmailSubmit
    {
        const string URL = "http://runnerdevice.co.il/api/TestEmail";
        // string URL = "http://api.inwise.com/rest/v1/transactional/emails/sendTemplate";
       //string URL = "http://runnerdevice.co.il/api/TestEmail";

        public void Send(EmailModel emailModel)
        {
            using (MemoryStream stream1 = new MemoryStream())
            {
                try
                {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(EmailModel));
                    ser.WriteObject(stream1, emailModel);
                    stream1.Position = 0;
                    StreamReader sr = new StreamReader(stream1);
                    // var json = Encoding.UTF8.GetString(stream1.ToArray());
                    var json = sr.ReadToEnd();
                   
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                    request.Method = "POST";
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
                        string response = responseReader.ReadToEnd();
                        Console.Out.WriteLine(response);
                        responseReader.Close();
                    }
                }

                catch (Exception e1)
                {
                    throw new InvalidPluginExecutionException(e1.Message);
                }
            }
        }
        
        public void Ping(EmailModel emailModel)
        {
            try
            {
                string URL = "http://runnerdevice.co.il/api/TestEmail";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "GET";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream())
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    Console.Out.WriteLine(response);
                    responseReader.Close();
                }
            }

            catch (Exception e1)
            {
                throw new InvalidPluginExecutionException(e1.Message);
            }


        }

    }
}


/*
  public void Sendc(EmailModel emailModel)
        {
            using (MemoryStream stream1 = new MemoryStream())
            {

                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(EmailModel));
                ser.WriteObject(stream1, emailModel);
                stream1.Position = 0;
                StreamReader sr = new StreamReader(stream1);
                var json = sr.ReadToEnd();
                // string URL = "http://api.inwise.com/rest/v1/transactional/emails/sendTemplate";
                string URL = "http://runnerdevice.co.il/api/TestEmail";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";
                UTF8Encoding encoding = new UTF8Encoding(false);
                byte[] bytes = encoding.GetBytes(json);
                request.ContentLength = bytes.Length;
                request.ContentLength = json.Length;


                using (StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), Encoding.UTF8))
                {
                    //requestWriter.BaseStream.Write(bytes, 0, bytes.Length);
                    //sw.Flush();
                    // sw.Close();
                    requestWriter.Write(json);
                }
                try
                {
                    using (WebResponse webResponse = request.GetResponse())
                    using (Stream webStream = webResponse.GetResponseStream())
                    using (StreamReader responseReader = new StreamReader(webStream))
                    {
                        string response = responseReader.ReadToEnd();
                        Console.Out.WriteLine(response);
                        responseReader.Close();
                    }
                }

                catch (Exception e1)
                {
                    throw new InvalidPluginExecutionException(e1.Message);
                }
            }
        }


 */
