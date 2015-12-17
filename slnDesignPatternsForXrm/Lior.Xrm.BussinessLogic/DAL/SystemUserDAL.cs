using Lior.Xrm.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lior.Xrm.BussinessLogic.DAL
{
    public class ServiceEventDAL
    {
        private IOrganizationService _service;

        public ServiceEventDAL(IOrganizationService service)
        {
            this._service = service;
        }


        public IEnumerable<new_incidentservice> GetAll()
        {
            int fetchCount = 1000;
            // Initialize the page number.
            int pageNumber = 1;
            // Specify the current paging cookie. For retrieving the first page, 
            // pagingCookie should be null.
            List<new_incidentservice> new_incidentservices = new List<new_incidentservice>();
            string pagingCookie = null;

            var fetchXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                    <entity name='new_incidentservice'>
                   <all-attributes /> 
                    <filter type='and'>
                      <condition attribute='statecode' operator='eq' value='0' />
                    </filter>
                  </entity>
                 </fetch>";
            while (true)
            {
                string xml = CreateXml(fetchXml, pagingCookie, pageNumber, fetchCount);
                RetrieveMultipleRequest fetchRequest1 = new RetrieveMultipleRequest
                {
                    Query = new FetchExpression(xml)
                };

                EntityCollection returnCollection = ((RetrieveMultipleResponse)_service.Execute(fetchRequest1)).EntityCollection;

                foreach (var c in returnCollection.Entities)
                {
                    var ins = c.ToEntity<new_incidentservice>();
                    new_incidentservices.Add(ins);
                }

                // Check for morerecords, if it returns 1.
                if (returnCollection.MoreRecords)
                    pageNumber++;// Increment the page number to retrieve the next page.
                else
                    break; // If no more records in the result nodes, exit the loop.

            }
            return new_incidentservices;
        }

        /// <summary>
        /// Fetch Helper Xml paging
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="cookie"></param>
        /// <param name="page"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        string CreateXml(string xml, string cookie, int page, int count)
        {
            StringReader stringReader = new StringReader(xml);
            XmlTextReader reader = new XmlTextReader(stringReader);

            // Load document
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            return CreateXml(doc, cookie, page, count);
        }

        /// <summary>
        /// Fetch Helper Xml paging
        /// </summary>
        /// <param name="job"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        string CreateXml(XmlDocument doc, string cookie, int page, int count)
        {
            XmlAttributeCollection attrs = doc.DocumentElement.Attributes;

            if (cookie != null)
            {
                XmlAttribute pagingAttr = doc.CreateAttribute("paging-cookie");
                pagingAttr.Value = cookie;
                attrs.Append(pagingAttr);
            }

            XmlAttribute pageAttr = doc.CreateAttribute("page");
            pageAttr.Value = System.Convert.ToString(page);
            attrs.Append(pageAttr);

            XmlAttribute countAttr = doc.CreateAttribute("count");
            countAttr.Value = System.Convert.ToString(count);
            attrs.Append(countAttr);

            StringBuilder sb = new StringBuilder(1024);
            StringWriter stringWriter = new StringWriter(sb);

            XmlTextWriter writer = new XmlTextWriter(stringWriter);
            doc.WriteTo(writer);
            writer.Close();

            return sb.ToString();
        }



    }
}
