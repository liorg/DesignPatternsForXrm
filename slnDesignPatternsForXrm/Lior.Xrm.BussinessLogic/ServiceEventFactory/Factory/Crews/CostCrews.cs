using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;
using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    //TotalCostCrews assignment
    public class TotalCostCrewsResponse
    {
        public decimal TotalPlacingTeams { get; set; }
    }

    public class TotalCostCrewsRequest : ICrew
    {
        public decimal AnAirCrewCost { get; set; }
        public decimal Flight_AttendantCost { get; set; }
        public bool Anycostplacingteams { get; set; }
        public decimal TotalPlacingTeams { get; set; }
    }

   
    public class XrmCostCrews : XrmDecoratorV2<TotalCostCrewsResponse>
    {
        public XrmCostCrews(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostCrewsResponse response)
        {
            _new_incidentservice.new_Totalplacingteam = response.TotalPlacingTeams;

        }
    }

    public class CostCrews : ParticipantOnCalculator<TotalCostCrewsRequest, TotalCostCrewsResponse>
    {

        public CostCrews(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostCrewsResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCostCrewsResponse CalcTotal(TotalCostCrewsRequest request)
        {
            var response = new TotalCostCrewsResponse();
            response.TotalPlacingTeams = request.Anycostplacingteams ? request.AnAirCrewCost + request.Flight_AttendantCost : 0;
            return response;
        }

        protected override TotalCostCrewsRequest LoadRequest()
        {
            var result = new TotalCostCrewsRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region   Anycostplacingteams

            if (preImage != null && preImage.new_AnyCostPlacingTeams.HasValue)
                result.Anycostplacingteams = preImage.new_AnyCostPlacingTeams.HasValue ? preImage.new_AnyCostPlacingTeams.Value : false;
            if (target.Attributes.Contains("new_AnyCostPlacingTeams".ToLower()))
                result.Anycostplacingteams = target.new_AnyCostPlacingTeams.HasValue ? target.new_AnyCostPlacingTeams.Value : false;
            #endregion

            #region TotalCost
            //-----
            if (preImage != null && preImage.new_Totalplacingteam.HasValue)
                result.TotalPlacingTeams = preImage.new_Totalplacingteam.HasValue ? preImage.new_Totalplacingteam.Value : 0;
            if (target.Attributes.Contains("new_Totalplacingteam".ToLower()))
                result.TotalPlacingTeams = target.new_Totalplacingteam.HasValue ? target.new_Totalplacingteam.Value : 0;
            //------

            if (preImage != null && preImage.new_AnAirCrewCost.HasValue)
                result.AnAirCrewCost = preImage.new_AnAirCrewCost.HasValue ? preImage.new_AnAirCrewCost.Value : 0;
            if (target.Attributes.Contains("new_AnAirCrewCost".ToLower()))
                result.AnAirCrewCost = target.new_AnAirCrewCost.HasValue ? target.new_AnAirCrewCost.Value : 0;

            if (preImage != null && preImage.new_FlightAttendant.HasValue)
                result.Flight_AttendantCost = preImage.new_FlightAttendant.HasValue ? preImage.new_FlightAttendant.Value : 0;
            if (target.Attributes.Contains("new_FlightAttendant".ToLower()))
                result.Flight_AttendantCost = target.new_FlightAttendant.HasValue ? target.new_FlightAttendant.Value : 0;


            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostCrewsResponse response)
        {
            return response.TotalPlacingTeams;
        }
    }


}
