using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Plugin.EmailSend
{
    public static class MetadataEmails
    {
        static ConcurrentDictionary<string, List<string>> _attEmails=new ConcurrentDictionary<string, List<string>>();
        static object _syncRoot = new object();
        public static string[] GetEmails(IOrganizationService service,string entityName)
        {
            List<string> temp;
            if (_attEmails.TryGetValue(entityName, out temp))
            {
                if(temp!=null)
                return temp.ToArray();
            }
            else
            {
                lock (_syncRoot)
                {
                    temp = new List<string>();
                    RetrieveEntityRequest metaDataRequest = new RetrieveEntityRequest();
                    RetrieveEntityResponse metaDataResponse = new RetrieveEntityResponse();
                    metaDataRequest.EntityFilters = EntityFilters.Attributes;
                    metaDataRequest.LogicalName = entityName;
                    metaDataResponse = (RetrieveEntityResponse)service.Execute(metaDataRequest);
                    var entityData = metaDataResponse.EntityMetadata;
                    // entityData.Attributes.Where(aa=>aa.is)
                    foreach (var attr in entityData.Attributes)
                    {
                        if (attr is StringAttributeMetadata)
                        {
                            StringAttributeMetadata isemail = (StringAttributeMetadata)attr;
                            if (isemail.Format == StringFormat.Email)
                                temp.Add(isemail.SchemaName.ToLower());

                        }

                    }
                    _attEmails.TryAdd(entityName, temp);
                }
            }
            if (temp==null | !temp.Any())
                return null;
            return temp.ToArray();
        }
    }
}
