using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Lior.Xrm.JobsProvider.DataModel
{
    public static class EXSR2
    {
        public static string RemoveDuplicateAliasJ(this string xmlData)
        {
            try
            {

                XElement data = XElement.Parse(xmlData);
                var aliasDuplicate = (from row in data.Element("entity").Elements("link-entity")
                                      group row by new { row.Attribute("alias").Value } into grp
                                      select new { rp = grp.Count() > 1, alias = grp.Key }
                      ).ToList();
                foreach (var item in aliasDuplicate)
                {
                    XElement rowToRemove = (from rowRemove in data.Element("entity").Elements("link-entity")
                                            where rowRemove.Attribute("alias") != null && rowRemove.Attribute("alias").Value == item.alias.Value && item.rp == true
                                            select rowRemove
                              ).FirstOrDefault();
                    if (rowToRemove != null)
                    {
                        (from rowRemove in data.Element("entity").Elements("link-entity")
                         where rowRemove.Attribute("alias") != null && rowRemove.Attribute("alias").Value == item.alias.Value && item.rp == true
                         select rowRemove
                                  ).Remove();

                        data.Element("entity").Add(rowToRemove);
                    }
                }
                return data.ToString();
            }
            catch
            {
                return xmlData;
            }

        }
    }
    public class LargeResultSetsFetchXML
    {
        Action<Exception, string> _logError;

        public LargeResultSetsFetchXML(Action<Exception, string> logError = null)
        {
            _logError = logError;
        }

        public IEnumerable<EntityReference> Get(IOrganizationService service, string fetchXml, int fetchCount)
        {
            int pageNumber = 1;
            EntityCollection returnCollection = null;
            while (true)
            {
                // Build fetchXml string with the placeholders.
                var xml = CreateXml(fetchXml, pageNumber, fetchCount);
                xml = xml.RemoveDuplicateAliasJ();
                // Excute the fetch query and get the xml result.
                var fetchRequest1 = new RetrieveMultipleRequest
                {
                    Query = new FetchExpression(xml)
                };
                try
                {
                    returnCollection = ((RetrieveMultipleResponse)service.Execute(fetchRequest1)).EntityCollection;
                }
                catch (FaultException<OrganizationServiceFault> ee)
                {
                    // You can handle an exception here or pass it back to the calling method.
                    Log(ee, "FetchExpression:" + xml);
                    break;
                }
                if (returnCollection != null)
                {
                    foreach (Entity entityMember in returnCollection.Entities)
                        yield return entityMember.ToEntityReference();

                    // Check for morerecords, if it returns 1.
                    if (returnCollection.MoreRecords)
                        pageNumber++;// Increment the page number to retrieve the next page.

                    else
                        break; // If no more records in the result nodes, exit the loop.
                }
            }
        }
        void Log(Exception e, string s)
        {
            if (_logError != null)
                _logError(e, s);
        }

        string CreateXml(string xml, int page, int count)
        {
            var stringReader = new StringReader(xml);
            var reader = new XmlTextReader(stringReader);
            var doc = new XmlDocument();
            doc.Load(reader);
            return CreateXml(doc, page, count);
        }

        string CreateXml(XmlDocument doc, int page, int count)
        {
            if (doc.DocumentElement != null)
            {
                var attrs = doc.DocumentElement.Attributes;

                var pageAttr = doc.CreateAttribute("page");
                pageAttr.Value = Convert.ToString(page);
                attrs.Append(pageAttr);

                var countAttr = doc.CreateAttribute("count");
                countAttr.Value = Convert.ToString(count);
                attrs.Append(countAttr);
            }

            var sb = new StringBuilder(1024);
            var stringWriter = new StringWriter(sb);

            var writer = new XmlTextWriter(stringWriter);
            doc.WriteTo(writer);
            writer.Close();

            return sb.ToString();
        }
    }
}
