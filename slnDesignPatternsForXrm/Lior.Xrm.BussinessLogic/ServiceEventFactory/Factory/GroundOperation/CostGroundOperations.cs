using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    //TotalCostGroundOperations
    public class TotalCostGroundOperationsResponse
    {
        public decimal TotalGroundOperations { get; set; }
    }

    public class TotalCostGroundOperationsRequest : IGroundOperation
    {

        public bool Anycostgroundoperation { get; set; }
        public decimal TotalGroundOperations { get; set; }
    }
    //Total CostArrivalFLT
    public class XrmCostGroundOperations : XrmDecoratorV2<TotalCostGroundOperationsResponse>
    {
        public XrmCostGroundOperations(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostGroundOperationsResponse response)
        {
            _new_incidentservice.new_Totalgroundoperation = response.TotalGroundOperations;

        }
    }

    public class CostGroundOperations : ParticipantOnCalculator<TotalCostGroundOperationsRequest, TotalCostGroundOperationsResponse>
    {

        public CostGroundOperations(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostGroundOperationsResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCostGroundOperationsResponse CalcTotal(TotalCostGroundOperationsRequest request)
        {
            var response = new TotalCostGroundOperationsResponse();
            var costTotalCostArrival = _depencies.ContainsKey("TotalCostArrival") ? _depencies["TotalCostArrival"].GetTotal() : 0;
            var costTotalCostDepartureCost = _depencies.ContainsKey("TotalCostDepartureCost") ? _depencies["TotalCostDepartureCost"].GetTotal() : 0;

            response.TotalGroundOperations = request.Anycostgroundoperation ? costTotalCostArrival + costTotalCostDepartureCost : 0;
            return response;
        }

        protected override TotalCostGroundOperationsRequest LoadRequest()
        {
            var result = new TotalCostGroundOperationsRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region   Anycostgroundoperation

            if (preImage != null && preImage.new_AnyCostGroundOperation.HasValue)
                result.Anycostgroundoperation = preImage.new_AnyCostGroundOperation.HasValue ? preImage.new_AnyCostGroundOperation.Value : false;
            if (target.Attributes.Contains("new_AnyCostGroundOperation".ToLower()))
                result.Anycostgroundoperation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostGroundOperation.Value : false;
            #endregion

           
            return result;
        }



        public override decimal GetTotal(TotalCostGroundOperationsResponse response)
        {
            return response.TotalGroundOperations;
        }
    }


}
