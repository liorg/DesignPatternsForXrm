using System;
using System.Activities;
using System.Collections.ObjectModel;

using Microsoft.Crm.Sdk.Messages;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using Lior.Xrm.Workflow.BL;


namespace Lior.Xrm.Workflow
{
    public class DocsFindMissions : CodeActivity
    {
        [Input("Subject")]
        public InArgument<string> Subject { get; set; }
        
        protected override void Execute(CodeActivityContext executionContext)
        {
            //Create the tracing service
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();
            //Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
            Entity entity = null;
            var inputs = context.InputParameters;
            if (context.InputParameters.Contains("target"))
                entity = (Entity)context.InputParameters["target"];

            if (entity == null)
                return;
            DocLogic logic = new DocLogic(service);
            LoadDocParametersFactory loadDocParametersFactory = null;
            var subject = Subject.Get(executionContext);
            switch (entity.LogicalName)
            {
                case "opportunity":
                    loadDocParametersFactory = new OpportunityParametersFactory(service, entity, subject);
                    break;
                case "new_order":
                    loadDocParametersFactory = new OrderParametersFactory(service, entity, subject);
                    break;
                default:
                    loadDocParametersFactory = null;
                    break;
            }
            logic.Excute(loadDocParametersFactory);
        }
    }
}