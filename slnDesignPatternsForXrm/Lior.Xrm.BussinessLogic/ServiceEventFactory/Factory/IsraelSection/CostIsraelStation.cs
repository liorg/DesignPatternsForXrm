using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    //CostIsraelStation
    public class CostIsraelStationResponse
    {
        public decimal TOTALisraelstation { get; set; }
    }

    public class CostIsraelStationRequest : ISectionAnyIsraelStation
    {
        public decimal TOTALMeals { get; set; }
        public decimal TOTALtransportation { get; set; }
        public decimal TOTALhotels { get; set; }
        public decimal TotalCostAdditionalCost { get; set; }


        public bool AnyCostIsraelStation { get; set; }
        public decimal TotalIsraelStation { get; set; }
    }

    public class XrmCostIsraelStation : XrmDecoratorV2<CostIsraelStationResponse>
    {
        public XrmCostIsraelStation(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(CostIsraelStationResponse response)
        {
            _new_incidentservice.new_TOTALisraelstation = response.TOTALisraelstation;

        }
    }

    public class CostIsraelStation : ParticipantOnCalculator<CostIsraelStationRequest, CostIsraelStationResponse>
    {

        public CostIsraelStation(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<CostIsraelStationResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override CostIsraelStationResponse CalcTotal(CostIsraelStationRequest request)
        {
            CostIsraelStationResponse response = new CostIsraelStationResponse();
            var totalhotels = _depencies.ContainsKey("TOTALhotels") ? _depencies["TOTALhotels"].GetTotal() : 0;
            var totalTransportation = _depencies.ContainsKey("TOTALtransportation") ? _depencies["TOTALtransportation"].GetTotal() : 0;


            response.TOTALisraelstation = request.AnyCostIsraelStation ? totalhotels + totalTransportation +
                                                        request.TOTALMeals + request.TotalCostAdditionalCost : 0;
            return response;
        }

        protected override CostIsraelStationRequest LoadRequest()
        {
            var result = new CostIsraelStationRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region SectionAnyCostIsraelStation
            if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
                result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            if (target.Attributes.Contains("new_anycostisraelstation"))
                result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            #endregion

            #region TotalCost
            if (preImage != null && preImage.new_TOTALisraelstation.HasValue)
                result.TotalIsraelStation = preImage.new_TOTALisraelstation.HasValue ? preImage.new_TOTALisraelstation.Value : 0;
            if (target.Attributes.Contains("new_TOTALisraelstation".ToLower()))
                result.TotalIsraelStation = target.new_TOTALisraelstation.HasValue ? target.new_TOTALisraelstation.Value : 0;

            if (preImage != null && preImage.new_TOTALmeals.HasValue)
                result.TOTALMeals = preImage.new_TOTALmeals.HasValue ? preImage.new_TOTALmeals.Value : 0;
            if (target.Attributes.Contains("new_TOTALmeals".ToLower()))
                result.TOTALMeals= target.new_TOTALmeals.HasValue ? target.new_TOTALmeals.Value : 0;

            if (preImage != null && preImage.new_TOTALtransportation.HasValue)
                result.TOTALtransportation = preImage.new_TOTALtransportation.HasValue ? preImage.new_TOTALtransportation.Value : 0;
            if (target.Attributes.Contains("new_TOTALtransportation".ToLower()))
                result.TOTALtransportation = target.new_TOTALtransportation.HasValue ? target.new_TOTALtransportation.Value : 0;

            if (preImage != null && preImage.new_TOTALhotels.HasValue)
                result.TOTALhotels = preImage.new_TOTALhotels.HasValue ? preImage.new_TOTALhotels.Value : 0;
            if (target.Attributes.Contains("new_TOTALhotels".ToLower()))
                result.TOTALhotels = target.new_TOTALhotels.HasValue ? target.new_TOTALhotels.Value : 0;

            if (preImage != null && preImage.new_TotalCostAdditionalCost.HasValue)
                result.TotalCostAdditionalCost = preImage.new_TotalCostAdditionalCost.HasValue ? preImage.new_TotalCostAdditionalCost.Value : 0;
            if (target.Attributes.Contains("new_TotalCostAdditionalCost".ToLower()))
                result.TotalCostAdditionalCost = target.new_TotalCostAdditionalCost.HasValue ? target.new_TotalCostAdditionalCost.Value : 0;


            #endregion


            return result;
        }



        public override decimal GetTotal(CostIsraelStationResponse response)
        {
            return response.TOTALisraelstation;
        }
    }


}
