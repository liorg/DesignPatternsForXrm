using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    
    //CostNightWithDinner
    public class TotalCostNightWithDinnerRequest : ISectionAnyIsraelStation
    {
        public decimal NightWithDinner { get; set; }
        public decimal ParamNightWithDinner { get; set; }
        public bool AnyCostIsraelStation { get; set; }
        public decimal TotalCostNightWithDinner { get; set; }
    }
    public class TotalCostNightWithDinnerResponse
    {
        public decimal TotalCostNightWithDinner { get; set; }

    }


    public class XrmCostNightWithDinner : XrmDecoratorV2<TotalCostNightWithDinnerResponse>
    {
        public XrmCostNightWithDinner(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostNightWithDinnerResponse response)
        {
            _new_incidentservice.new_TotalCostNightWithDinner = response.TotalCostNightWithDinner;

        }
    }

    public class CostNightWithDinner : ParticipantOnCalculator<TotalCostNightWithDinnerRequest, TotalCostNightWithDinnerResponse>
    {

        public CostNightWithDinner(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostNightWithDinnerResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCostNightWithDinnerResponse CalcTotal(TotalCostNightWithDinnerRequest request)
        {
            TotalCostNightWithDinnerResponse response = new TotalCostNightWithDinnerResponse();
            response.TotalCostNightWithDinner = request.AnyCostIsraelStation ? request.ParamNightWithDinner * request.NightWithDinner : 0;
            return response;
        }

        protected override TotalCostNightWithDinnerRequest LoadRequest()
        {
            var result = new TotalCostNightWithDinnerRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region SectionAnyCostIsraelStation
            if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
                result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            if (target.Attributes.Contains("new_anycostisraelstation"))
                result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            #endregion

            #region TotalCostNightWithDinner
            if (preImage != null && preImage.new_TotalCostNightWithDinner.HasValue)
                result.TotalCostNightWithDinner = preImage.new_TotalCostNightWithDinner.HasValue ? preImage.new_TotalCostNightWithDinner.Value : 0;
            if (target.Attributes.Contains("new_totalcostnightwithdinner"))
                result.TotalCostNightWithDinner = target.new_TotalCostNightWithDinner.HasValue ? target.new_TotalCostNightWithDinner.Value : 0;

            if (preImage != null && preImage.new_NightWithDinner.HasValue)
                result.NightWithDinner = preImage.new_NightWithDinner.HasValue ? preImage.new_NightWithDinner.Value : 0;
            if (target.Attributes.Contains("new_nightwithdinner"))
                result.NightWithDinner = target.new_NightWithDinner.HasValue ? target.new_NightWithDinner.Value : 0;

            if (parameters.Attributes.Contains("new_nightwithdinner"))
                result.ParamNightWithDinner = parameters.new_NightWithDinner.HasValue ? parameters.new_NightWithDinner.Value : 0;

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostNightWithDinnerResponse response)
        {
            return response.TotalCostNightWithDinner;
        }
    }


}
