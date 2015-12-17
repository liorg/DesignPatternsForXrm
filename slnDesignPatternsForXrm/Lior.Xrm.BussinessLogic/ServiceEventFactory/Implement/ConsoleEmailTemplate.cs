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


    public class ConsoleEmailTemplate : IEmailTemplate
    {
        Guid _emailTemp;
        public ConsoleEmailTemplate(IOrganizationService service)
        {
            var configuration =// new ConfigurationFactory().GetConfiguration(service);
            _emailTemp = Guid.NewGuid(); //demo // Guid.Parse(configuration.EmailTemplate.ServiceEventConsoleEmailTemplate);

        }


        public Guid EmailTemplateId
        {
            get { return _emailTemp; }
        }
    }
    }

