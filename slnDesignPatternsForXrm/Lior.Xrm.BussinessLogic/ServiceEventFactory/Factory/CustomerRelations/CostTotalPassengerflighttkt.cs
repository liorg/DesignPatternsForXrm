
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalPassengerflighttktResponse
    {
        //new_totalpassengerflighttkt
        public int TotalPassengerflighttkt { get; set; }
    }

    public class TotalPassengerflighttktRequest : ICustomerRelations
    {
        public bool AnycostCustomerRelation { get; set; }
        public int? TotalPassengerflighttkt { get; set; }


    }

    public class XrmCostTotalPassengerflighttkt : XrmDecoratorV2<TotalPassengerflighttktResponse>
    {
        public XrmCostTotalPassengerflighttkt(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalPassengerflighttktResponse response)
        {
            _new_incidentservice.new_TotalPassengerFlightTKT = response.TotalPassengerflighttkt;

        }
    }

    public class CostTotalPassengerflighttkt  : ParticipantOnCalculator<TotalPassengerflighttktRequest, TotalPassengerflighttktResponse>
    {

        public CostTotalPassengerflighttkt(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalPassengerflighttktResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }


        protected override TotalPassengerflighttktResponse CalcTotal(TotalPassengerflighttktRequest request)
        {
            var response = new TotalPassengerflighttktResponse();
            response.TotalPassengerflighttkt = 0;

            if (!request.AnycostCustomerRelation)
                return response;

            if (request.TotalPassengerflighttkt.HasValue)
            {
                response.TotalPassengerflighttkt = request.TotalPassengerflighttkt.Value;
                return response;
            }
            var openClose = this.OpenClosedCalculator;
            var orginal = openClose.GetOriginalFlight();

            if (orginal != null)
              response.TotalPassengerflighttkt = orginal.new_TotalPassengerFlight.GetValueOrDefault();
                
            return response;

        }

        protected override TotalPassengerflighttktRequest LoadRequest()
        {
            var result = new TotalPassengerflighttktRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region   AnycostCustomerRelation

            if (preImage != null && preImage.new_AnyCostCustomerRelation.HasValue)
                result.AnycostCustomerRelation = preImage.new_AnyCostCustomerRelation.HasValue ? preImage.new_AnyCostCustomerRelation.Value : false;
            if (target.Attributes.Contains("new_AnyCostCustomerRelation".ToLower()))
                result.AnycostCustomerRelation = target.new_AnyCostCustomerRelation.HasValue ? target.new_AnyCostCustomerRelation.Value : false;
            #endregion

            #region TotalCost
            //if (preImage != null && preImage.new_TotalPassengerFlightTKT.HasValue)
            //    result.TotalPassengerflighttkt = preImage.new_TotalPassengerFlightTKT.HasValue ? preImage.new_TotalPassengerFlightTKT.Value : 0;
            if (target.Attributes.Contains("new_TotalPassengerFlightTKT".ToLower()))
                result.TotalPassengerflighttkt = target.new_TotalPassengerFlightTKT.HasValue ? target.new_TotalPassengerFlightTKT.Value : 0;

            #endregion

            return result;
        }

        public override decimal GetTotal(TotalPassengerflighttktResponse response)
        {
            return response.TotalPassengerflighttkt;
        }
    }


}
