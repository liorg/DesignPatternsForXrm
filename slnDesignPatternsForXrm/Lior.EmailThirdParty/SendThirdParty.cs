using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;

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
    public class SendThirdParty : Plugin
    {
       
        public SendThirdParty()
            : base(typeof(SendThirdParty))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(20, "Send", "email", new Action<LocalPluginContext>(EmailHandlerPreSend)));
        }

        protected void EmailHandlerPreSend(LocalPluginContext localContext)
        {
            EmailModel emailModel = new EmailModel();
            emailModel.message = new Message();
            if (localContext == null)
                throw new ArgumentNullException("localContext");

            // Obtain the execution context from the service provider.
            IPluginExecutionContext context = localContext.PluginExecutionContext;
            IOrganizationService service = localContext.OrganizationService;

            //Check if target exists.
            var isTargetExistsAndIsEnity = context.InputParameters.Contains("EmailId") && context.InputParameters["EmailId"] is Guid;

            if (!isTargetExistsAndIsEnity)
                throw new InvalidPluginExecutionException("Pre update plugin step must contain an entity target");

            Guid emailid = (Guid)context.InputParameters["EmailId"];
            
            EmailCrm emailCrm = new EmailCrm(service, emailModel);
            bool issueEmail = emailCrm.CrmFieldSetEmailAgent(emailid);

            if (!issueEmail)
            {
                AgentEmailSubmit agent = new AgentEmailSubmit();
                agent.Send(emailModel);
                context.InputParameters["IssueSend"] = issueEmail;//true if the email should be sent; otherwise, false, just record it as sent.
            }
        }
    }
}