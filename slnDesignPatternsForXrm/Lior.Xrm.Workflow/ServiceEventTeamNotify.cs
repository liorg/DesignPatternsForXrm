// <copyright file="NewClaimCreateMailToLowsuit.cs" company="">
// Copyright (c) 2012 All Rights Reserved
// </copyright>
// <author></author>
// <date>12/18/2012 3:37:20 PM</date>
// <summary>Implements the NewClaimCreateMailToLowsuit Workflow Activity.</summary>
namespace Lior.Xrm.Workflow
{
    using System;
    using System.Activities;
    using System.ServiceModel;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    using Microsoft.Crm.Sdk.Messages;
    using Lior.Xrm.Xrm;
    using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory;

    public sealed class ServiceEventTeamNotify : CodeActivity
    {
        [RequiredArgument]
        [Input("Template for Send")]
        [ReferenceTarget("template")]
        public InArgument<EntityReference> EmailTemplate { get; set; }

        [RequiredArgument]
        [Input("User From Send")]
        [ReferenceTarget("systemuser")]
        public InArgument<EntityReference> FromUser { get; set; }
        /// <summary>
        /// Executes the workflow activity.
        /// </summary>
        /// <param name="executionContext">The execution context.</param>
        protected override void Execute(CodeActivityContext executionContext)
        {
            // Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            if (tracingService == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve tracing service.");
            }

            tracingService.Trace("Entered NewClaimCreateMailToLowsuit.Execute(), Activity Instance Id: {0}, Workflow Instance Id: {1}",
                executionContext.ActivityInstanceId,
                executionContext.WorkflowInstanceId);

            // Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();

            if (context == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve workflow context.");
            }

            tracingService.Trace("NewClaimCreateMailToLowsuit.Execute(), Correlation Id: {0}, Initiating User: {1}",
                context.CorrelationId,
                context.InitiatingUserId);

            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                // TODO: Implement your custom Workflow business logic.
                Entity target = (Entity)context.InputParameters["Target"];
                new_incidentservice new_incidentservice = target.ToEntity<new_incidentservice>();

                SubjectEmailSender email = new SubjectEmailSender(service, new_incidentservice);

                HelperEvent helper = new HelperEvent(service, new_incidentservice);
                var parameters = helper.GetParamters();
                var originalFlight = helper.GetOriginalFlight();
                EntityReference emailTemplate = EmailTemplate.Get<EntityReference>(executionContext);
                EntityReference user = FromUser.Get<EntityReference>(executionContext);
                WFEmailTemplate wfemailTemp = new WFEmailTemplate(emailTemplate);

                var israelWF = new IsraelWF(service, new_incidentservice, parameters, originalFlight, wfemailTemp);
                var callCenterWF = new CallCenterWF(service, new_incidentservice, parameters, originalFlight, wfemailTemp);
                var cashierWF = new CashierWF(service, new_incidentservice, parameters, originalFlight, wfemailTemp);
                var groupWF = new GroupWF(service, new_incidentservice, parameters, originalFlight, wfemailTemp);
                var foodWF = new FoodWF(service, new_incidentservice, parameters, originalFlight, wfemailTemp);
                var customerRelationWF = new CustomerRelationWF(service, new_incidentservice, parameters, originalFlight, wfemailTemp);
                var scheduleWF = new ScheduleWF(service, new_incidentservice, parameters, originalFlight, wfemailTemp);
                var cargoWF = new CargoWF(service, new_incidentservice, parameters, originalFlight, wfemailTemp);
                var crewsWF = new CrewsWF(service, new_incidentservice, parameters, originalFlight, wfemailTemp);
                var sundorWF = new SundorWF(service, new_incidentservice, parameters, originalFlight, wfemailTemp);

                email.Attach(israelWF);
                email.Attach(callCenterWF);
                email.Attach(cashierWF);
                email.Attach(groupWF);
                email.Attach(foodWF);
                email.Attach(customerRelationWF);
                email.Attach(scheduleWF);
                email.Attach(cargoWF);
                email.Attach(crewsWF);
                email.Attach(sundorWF);

                email.SendEmailsToTeams(user.Id);

            }


            catch (FaultException<OrganizationServiceFault> e)
            {
                tracingService.Trace("Exception: {0}", e.ToString());

                // Handle the exception.
                throw;
            }

            tracingService.Trace("Exiting NewClaimCreateMailToLowsuit.Execute(), Correlation Id: {0}", context.CorrelationId);
        }
    }
}