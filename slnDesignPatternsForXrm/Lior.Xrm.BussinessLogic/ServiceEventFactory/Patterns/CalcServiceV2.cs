using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xrm.Sdk;
using Lior.Xrm.Xrm;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class CalcService : OpenClosedCalculator
    {
         public CalcService(IOrganizationService service, new_incidentservice target, new_incidentservice preImage, new_incidentserviceparameter paramters)
            : base(service, target, preImage, paramters)
        {
        }

        public override void Register(ParticipantOnCalculator calc)
        {
            _calcs.Add(calc);
        }



    }

}

