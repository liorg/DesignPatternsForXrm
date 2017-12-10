using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Plugin.EmailSend
{
    public class EmailCrm
    {
        const string ValidTEXT = "lior_gr@malam.com";
        const string API_KEY = "97f39c85e2894311b7ecc097491279bb";
       
        IOrganizationService _service;
        EmailModel _emailModel;
        public EmailCrm(IOrganizationService service, EmailModel emailModel)
        {
            _service = service;
            _emailModel = emailModel;
            _emailModel.api_key = API_KEY;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailid"></param>
        /// <param name="emailModel"></param>
        /// <param name="fieldName"></param>
        /// <returns>IssueSend</returns>
        public bool CrmFieldSetEmailAgent(Guid emailid)
        {
            bool issueEmailFlag = true;
            bool issueEmail = true;
            var email = _service.Retrieve("email", emailid, new ColumnSet("to", "cc", "bcc", "subject", "from", "description"));
            _emailModel.message.html = email.GetAttributeValue<string>("description");
            _emailModel.message.subject = email.GetAttributeValue<string>("subject");

            issueEmail= GetFrom(email);
            if (issueEmail)
                return issueEmailFlag;

            _emailModel.message.to = new List<to>();

            issueEmail = Get(email, "to");
            if (!issueEmail & issueEmailFlag)
                issueEmailFlag = false;
            issueEmail = Get(email, "bcc");
            if (!issueEmail & issueEmailFlag)
                issueEmailFlag = false;
            issueEmail = Get(email, "cc");
            if (!issueEmail & issueEmailFlag)
                issueEmailFlag = false;

            return issueEmailFlag;
        }

        bool GetFrom(Entity email)
        {
            var from = email.GetAttributeValue<EntityCollection>("from");

            if (from != null & from.Entities != null & from.Entities.Any())
            {
                var emailsend = from.Entities.FirstOrDefault();
                var fromPartyId = emailsend.GetAttributeValue<EntityReference>("partyid");
                string[] fromEmailFieldsNames = null;
                var entityFrom = GetEntityByParyId(fromPartyId, out fromEmailFieldsNames);

                if (entityFrom == null)
                    return true;
                foreach (var fieldsName in fromEmailFieldsNames)
                {
                    var emailaddress = entityFrom.GetAttributeValue<string>(fieldsName);
                    if (!String.IsNullOrWhiteSpace(emailaddress))
                    {
                        _emailModel.message.from_email = emailaddress;
                        return false;
                    }
                }
                return true;
            }
            else
                return true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="fieldName"></param>
        /// <returns>IssueSend</returns>
        bool Get(Entity email, string fieldName)
        {
            bool issueSend = true;
            var to = email.GetAttributeValue<EntityCollection>(fieldName);
            if (to != null && to.Entities != null & to.Entities.Any())
            {
                foreach (var itemTo in to.Entities)
                {
                    var donotemail = itemTo.GetAttributeValue<bool?>("donotemail");
                    if (donotemail.HasValue && donotemail.Value)
                        continue;

                    var partyid = itemTo.GetAttributeValue<EntityReference>("partyid");
                    if (partyid == null)
                        continue;
                    string[] emailFieldsNames = null;


                    var entityTo = GetEntityByParyId(partyid, out emailFieldsNames);
                    if (entityTo == null)
                        continue;
                    foreach (var fieldsName in emailFieldsNames)
                    {
                        var emailaddress = entityTo.GetAttributeValue<string>(fieldsName);
                        if (!String.IsNullOrWhiteSpace(emailaddress))
                        {
                            if (emailaddress.Contains(ValidTEXT))
                                issueSend = false;
                            _emailModel.message.to.Add(new to
                            {
                                email = emailaddress,
                                type = fieldName
                            });

                        }
                    }
                }
            }
            return issueSend;
        }

        Entity GetEntityByParyId(EntityReference partyid, out string[] emailFieldsNames)
        {

            emailFieldsNames = MetadataEmails.GetEmails(_service, partyid.LogicalName);
            if (emailFieldsNames == null)
                return null;
            var entityFrom = _service.Retrieve(partyid.LogicalName, partyid.Id, new ColumnSet(emailFieldsNames));
            return entityFrom;
        }


    }
}
