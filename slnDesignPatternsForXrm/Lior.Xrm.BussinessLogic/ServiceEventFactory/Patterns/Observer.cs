using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lior.Xrm.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;

using System.Web;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{

    public abstract class Observer
    {
        protected const string cm_nameOfSettings = "Service URL  for sending escalation";
        protected IOrganizationService _service;
        protected new_incidentserviceparameter _paramters;
        protected new_incidentservice _target;
        protected new_flightoccurrence _orginalFlightOccurrence;
        protected IEmailTemplate _emailTemplate;
        //http://mycrm/myOrg/main.aspx?etc=4&id=%7b899D4FCF-F4D3-E011-9D26-00155DBA3819%7d&pagetype=entityrecord
        protected string FormatUrl = "http://{0}/{1}/main.aspx?etn={2}&id={3}&pagetype=entityrecord";

        public Observer(IOrganizationService service, new_incidentservice target,
            new_incidentserviceparameter paramters, new_flightoccurrence orginalFlightOccurrence, IEmailTemplate configEmailTemplate)
        {
            _service = service;
            _target = target;
            _paramters = paramters;
            _orginalFlightOccurrence = orginalFlightOccurrence;
            _emailTemplate = configEmailTemplate;
        }

        public abstract bool CanSendEmail();

        public Email SendEmail(Guid fromUser)
        {
            Email email = new Email();
            email.TrackingToken = string.Empty;
            var fromParty = new ActivityParty
             {
                 PartyId = new EntityReference(SystemUser.EntityLogicalName, fromUser)
             };

            ActivityParty[] emailTo = null;
            ActivityParty toParty = null;
            var teamid = GetUsersTeam();
            if (teamid == null) throw new ArgumentNullException("teamid is null");
            var teamGuids = GetTeamMembers(teamid.Value);

            if (teamGuids != null && teamGuids.Count > 0)
            {
                emailTo = new ActivityParty[teamGuids.Count];
                for (int i = 0; i < teamGuids.Count; i++)
                {
                    Guid userGuid = teamGuids[i];
                    toParty = new ActivityParty
                    {
                        PartyId = new EntityReference(SystemUser.EntityLogicalName, userGuid)
                    };
                    emailTo[i] = toParty;
                }
            }
            if (emailTo != null)
                email.To = emailTo;

            email.From = new ActivityParty[] { fromParty };

            Guid getTamplateEmail = GetTamplateEmail();
            var emailTemplate = GetEmailTemplateDescription(getTamplateEmail, fromUser);
            email.Subject = GenerateTemplateTitle(emailTemplate.Item2, _orginalFlightOccurrence);
            email.Description = GenerateTemplateBody(emailTemplate.Item1, _orginalFlightOccurrence);
            //    email.Attributes.Add("RegardingObjectId".ToLower(), _target.ToEntityReference()); //= new Microsoft.Xrm.Client.CrmEntityReference(_target.LogicalName, _target.Id); //_target.ToEntityReference();
            email.RegardingObjectId = _target.ToEntityReference();
            Guid emailid = _service.Create(email);
            email.ActivityId = emailid;
            return email;
        }

        protected virtual Guid GetTamplateEmail()
        {
            return _emailTemplate.EmailTemplateId;
        }

        protected virtual string GenerateTemplateTitle(string title, new_flightoccurrence orginalFlightOccurrence)
        {
            var url = GetURLPath(orginalFlightOccurrence.ToEntityReference());
            title = title.Replace("!URL!", url);
            return title;
        }

        protected virtual string GenerateTemplateBody(string body, new_flightoccurrence orginalFlightOccurrence)
        {
            var urlCrm = GetURLPath(orginalFlightOccurrence.ToEntityReference());
            var eurl = urlCrm;//HttpUtility.UrlEncode(urlCrm);
            var url = "<a href='" + eurl + "' target='_blank'>flight</a>";
            body = body.Replace("!URL!", url);
            return body;
        }

        protected abstract Guid? GetUsersTeam();

        protected Tuple<string, string> GetEmailTemplateDescription(Guid templateGuid, Guid sysUser)
        {
            InstantiateTemplateRequest templateRequest = new InstantiateTemplateRequest();
            //object id is the system user since it is a general template
            templateRequest.ObjectId = sysUser;
            templateRequest.ObjectType = SystemUser.EntityLogicalName;
            var body = ""; var subject = "";
            templateRequest.TemplateId = templateGuid;

            InstantiateTemplateResponse instTempResp = (InstantiateTemplateResponse)_service.Execute(templateRequest);
            if (instTempResp.EntityCollection.Entities != null && instTempResp.EntityCollection.Entities.Count > 0)
            {
                body = (string)instTempResp.EntityCollection.Entities[0].Attributes["description"];
                subject = (string)instTempResp.EntityCollection.Entities[0].Attributes["subject"];
                return new Tuple<string, string>(body, subject);

            }
            return new Tuple<string, string>(body, subject);
        }

        protected string GetURLPath(EntityReference target)
        {
            //url = string.Format("http://elalcrm11/ELAL/main.aspx?etn={0}&pagetype=entityrecord&id={1}", target.LogicalName, target.Id);
            string serviceurlfromsettings = GetUrlFromSettingsByName(cm_nameOfSettings, _service);
            string url = string.Empty;
            url = string.Format("{0}?etn={1}&pagetype=entityrecord&id={2}", serviceurlfromsettings, target.LogicalName, target.Id);

            return url;
        }

        protected string GetUrlFromSettingsByName(string serviceUrl, IOrganizationService service)
        {
            string url = string.Empty;
            QueryExpression query = new QueryExpression("new_parameter");
            ColumnSet columns = new ColumnSet();
            columns.AddColumns("new_description",
                    "new_name",
                    "new_value");

            query.ColumnSet = columns;
            query.Criteria = new FilterExpression();
            query.Criteria.FilterOperator = LogicalOperator.And;
            ConditionExpression conditionIdByName = new ConditionExpression("new_name", ConditionOperator.Equal, serviceUrl);
            query.Criteria.Conditions.AddRange(conditionIdByName);
            EntityCollection col = service.RetrieveMultiple(query);
            if (col.Entities.Count > 0)
            {
                if (col.Entities[0].Contains("new_value"))
                {
                    return url = col.Entities[0].GetAttributeValue<string>("new_value");
                }

            }

            return url;
        }

        protected List<Guid> GetTeamMembers(Guid teamId)
        {
            List<Guid> users = new List<Guid>();
            var formatFectx = "<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='true'><entity name='systemuser'> <attribute name='systemuserid' /><link-entity name='teammembership' from='systemuserid' to='systemuserid' visible='false' intersect='true'><link-entity name='team' from='teamid' to='teamid' alias='ab'> <filter type='and'><condition attribute='teamid' operator='eq' value='{0}' /></filter> </link-entity></link-entity></entity></fetch>";
            var fetchXml = String.Format(formatFectx, teamId);
            EntityCollection col = this._service.RetrieveMultiple(new FetchExpression(fetchXml));
            foreach (var sysUser in col.Entities)
            {
                users.Add(sysUser.Id);
            }
            return users;
        }

    }
}

