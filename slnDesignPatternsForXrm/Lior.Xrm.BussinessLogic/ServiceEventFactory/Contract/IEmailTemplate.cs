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


    public interface IEmailTemplate
    {
        Guid EmailTemplateId { get; }
    }
    
}
