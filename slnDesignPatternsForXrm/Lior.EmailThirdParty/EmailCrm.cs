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
        const string HTML = "html";
        const string UTF8 = "utf-8";
        IOrganizationService _service;
        EmailModel _emailModel; ConfigEmail _configemail;
        public EmailCrm(IOrganizationService service, EmailModel emailModel, ConfigEmail configemail)
        {
            _service = service;
            _emailModel = emailModel;
            _emailModel.message.content_type = HTML;
            _emailModel.message.charset = UTF8;
            _emailModel.message.from_email = configemail.FromEmail;
            _configemail = configemail;
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




            issueEmail = GetFrom(email);
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

                var emailFieldsNames = MetadataEmails.GetEmails(_service, fromPartyId.LogicalName);
                if (emailFieldsNames == null)
                    return true;
               
                var entityFrom = _service.Retrieve(fromPartyId.LogicalName, fromPartyId.Id, new ColumnSet(emailFieldsNames));

                if (entityFrom == null)
                    return true;
                foreach (var fieldsName in emailFieldsNames)
                {
                    var emailaddress = entityFrom.GetAttributeValue<string>(fieldsName);
                    if (!String.IsNullOrWhiteSpace(emailaddress) && emailaddress==_configemail.ReplyEmail)
                    {
                        _emailModel.message.from_name = _configemail.ReplayLabel; 
                        _emailModel.message.reply_to = emailaddress;
                        return false;
                    }
                }
                return true;
            }
            else
                return true;
        }

        void SetAttachments(Entity email)
        {
            var fecthXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='activitymimeattachment'>
                                <attribute name='filename' />  
                                <attribute name='body' /> 
                                <attribute name='mimetype' /> 
                    <link-entity name='email' alias='ab' to='objectid' from='activityid'>
                    <filter type='and'><condition attribute='activityid' operator='eq'  value='" + email.Id.ToString() + "' /></filter></link-entity></entity></fetch>";

            EntityCollection result = _service.RetrieveMultiple(new FetchExpression(fecthXml));
            if (result != null && result.Entities != null && result.Entities.Any())
            {
                _emailModel.message.attachments = new List<attachment>();
                foreach (var c in result.Entities)
                {
                    var mimetype = c.Attributes["mimetype"].ToString();
                    var filename = c.Attributes["filename"].ToString();
                    var documentbody = c.Attributes["body"].ToString();
                    _emailModel.message.attachments.Add(new attachment
                    {
                        content = documentbody,
                        name = filename,
                        type = mimetype
                    });
                }
            }
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
                            if (_configemail.IsReverse)
                            {
                                if (emailaddress.Contains(_configemail.TextChecked))// if (!emailaddress.Contains(ValidTEXT))
                                    issueSend = false;
                            }
                            else
                            {
                                if (!emailaddress.Contains(_configemail.TextChecked))// if (!emailaddress.Contains(ValidTEXT))
                                    issueSend = false;
                            }
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
