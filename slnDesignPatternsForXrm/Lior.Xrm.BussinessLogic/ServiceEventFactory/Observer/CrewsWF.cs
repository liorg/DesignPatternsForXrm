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

    public class CrewsWF : Observer
    {
        public CrewsWF(IOrganizationService service, new_incidentservice target,
            new_incidentserviceparameter paramters,
            new_flightoccurrence orginalFlightOccurrence,
            IEmailTemplate iEmailTemplate)
            : base(service, target, paramters, orginalFlightOccurrence, iEmailTemplate)
        {

        }
        public override bool CanSendEmail()
        {
            if (_target.new_AnyCostPlacingTeams != null && _target.new_AnyCostPlacingTeams.Value)
            {
                return true;
            }
            return false;
        }


        protected override Guid? GetUsersTeam()
        {
            if (_paramters.new_PlacingTeamsTeam == null) return null;
            return _paramters.new_PlacingTeamsTeam.Id;
        }
    }
}
