using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
   
    //CostNightWithBreakfast
    public class TotalCostNightWithBreakfastRequest : ISectionAnyIsraelStation
    {
        public decimal NightWithBreakfast { get; set; }
        public decimal ParamNightWithBreakfast { get; set; }  
        public bool AnyCostIsraelStation { get; set; }
        public decimal TotalCostNightWithBreakfast { get; set; }
    }

    public class TotalCostNightWithBreakfastResponse
    {
        public decimal TotalCostNightWithBreakfast { get; set; }
    }


    public class XrmCostNightWithBreakfast: XrmDecoratorV2<TotalCostNightWithBreakfastResponse>
    {
        public XrmCostNightWithBreakfast(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostNightWithBreakfastResponse response)
        {
            _new_incidentservice.new_TotalCostNightWithBreakfast= response.TotalCostNightWithBreakfast;

        }
    }

    public class CostNightWithBreakfast : ParticipantOnCalculator<TotalCostNightWithBreakfastRequest, TotalCostNightWithBreakfastResponse>
    {
       
        public CostNightWithBreakfast(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostNightWithBreakfastResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }

        

        protected override TotalCostNightWithBreakfastResponse CalcTotal(TotalCostNightWithBreakfastRequest request)
        {
            TotalCostNightWithBreakfastResponse response = new TotalCostNightWithBreakfastResponse();
            response.TotalCostNightWithBreakfast = request.AnyCostIsraelStation ? request.ParamNightWithBreakfast * request.NightWithBreakfast : 0;
            return response;
        }

        protected override TotalCostNightWithBreakfastRequest LoadRequest()
        {
            var result = new TotalCostNightWithBreakfastRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region SectionAnyCostIsraelStation
            if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
                result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            if (target.Attributes.Contains("new_anycostisraelstation"))
                result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            #endregion

            #region TotalCostNightWithBreakfast
            if (preImage != null && preImage.new_TotalCostNightWithBreakfast.HasValue)
                result.TotalCostNightWithBreakfast = preImage.new_TotalCostNightWithBreakfast.HasValue ? preImage.new_TotalCostNightWithBreakfast.Value : 0;
            if (target.Attributes.Contains("new_totalcostnightwithbreakfast"))
                result.TotalCostNightWithBreakfast = target.new_TotalCostNightWithBreakfast.HasValue ? target.new_TotalCostNightWithBreakfast.Value : 0;

            if (preImage != null && preImage.new_NightWithBreakfast.HasValue)
                result.NightWithBreakfast = preImage.new_NightWithBreakfast.HasValue ? preImage.new_NightWithBreakfast.Value : 0;
            if (target.Attributes.Contains("new_nightwithbreakfast"))
                result.NightWithBreakfast = target.new_NightWithBreakfast.HasValue ? target.new_NightWithBreakfast.Value : 0;

            if (parameters.Attributes.Contains("new_nightwithbreakfast"))
                result.ParamNightWithBreakfast = parameters.new_NightWithBreakfast.HasValue ? parameters.new_NightWithBreakfast.Value : 0;

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostNightWithBreakfastResponse response)
        {
            return response.TotalCostNightWithBreakfast;
        }
    }


}
