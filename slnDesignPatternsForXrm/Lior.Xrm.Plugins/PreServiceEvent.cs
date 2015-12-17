using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lior.Xrm.Plugins
{
    using System;
    using System.ServiceModel;
    using Microsoft.Xrm.Sdk;

    using Lior.Xrm.BusinessUnit.BL;

    using Lior.Xrm.Xrm;

    public class PreServiceEvent : Plugin
    {
        private string preImageAlias = "PreImage";

        public PreServiceEvent()
            : base(typeof(PreServiceEvent))
        {
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(20, "Create", "new_incidentservice", new Action<LocalPluginContext>(ExecutePreCreate)));
            base.RegisteredEvents.Add(new Tuple<int, string, string, Action<LocalPluginContext>>(20, "Update", "new_incidentservice", new Action<LocalPluginContext>(ExecutePreUpdate)));

            // Note : you can register for more events here if this plugin is not specific to an individual entity and message combination.
            // You may also need to update your RegisterFile.crmregister plug-in registration file to reflect any change.
        }
        protected void ExecutePreCreate(LocalPluginContext localContext)
        {
            if (localContext == null)
            {
                throw new ArgumentNullException("localContext");
            }
            IPluginExecutionContext context = localContext.PluginExecutionContext;
            Entity targetEntity = (Entity)context.InputParameters["Target"]; 
            IncidentServiceEventCalc calc = new IncidentServiceEventCalc(localContext.OrganizationService, true);
            new_incidentservice new_servicecostTarget = targetEntity.ToEntity<Xrm.new_incidentservice>();
            calc.CalcIncidentService(new_servicecostTarget, null);

        }

        protected void ExecutePreUpdate(LocalPluginContext localContext)
        {
            if (localContext == null)
                throw new ArgumentNullException("localContext");
            
            IPluginExecutionContext context = localContext.PluginExecutionContext;
            Entity targetEntity, preEntity;
            targetEntity = (Entity)context.InputParameters["Target"];
            preEntity = (Entity)context.PreEntityImages[preImageAlias];
            IncidentServiceEventCalc calc = new IncidentServiceEventCalc(localContext.OrganizationService, false);
            var new_servicecostTarget = targetEntity.ToEntity<Xrm.new_incidentservice>();

            new_incidentservice new_servicecostImage = preEntity.ToEntity<Xrm.new_incidentservice>();
            calc.CalcIncidentService(new_servicecostTarget, new_servicecostImage);

        }

    }
}
