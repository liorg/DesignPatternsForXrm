using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    //CostNightWithLunch
    public class TotalCostNightWithLunchRequest : ISectionAnyIsraelStation
    {
        public decimal NightWithLunch { get; set; }
        public decimal ParamNightWithLunch { get; set; } 
        public bool AnyCostIsraelStation { get; set; }
        public decimal TotalCostNightWithLunch { get; set; }
    }

    public class TotalCostNightWithLunchResponse
    {
        public decimal TotalCostNightWithLunch { get; set; }
    }
    public class XrmCostNightWithLunch : XrmDecoratorV2<TotalCostNightWithLunchResponse>
    {
        public XrmCostNightWithLunch(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostNightWithLunchResponse response)
        {
            _new_incidentservice.new_TotalCostNightWithLunch= response.TotalCostNightWithLunch;

        }
    }

    public class CostNightWithLunch : ParticipantOnCalculator<TotalCostNightWithLunchRequest, TotalCostNightWithLunchResponse>
    {
       
        public CostNightWithLunch(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostNightWithLunchResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }

        

        protected override TotalCostNightWithLunchResponse CalcTotal(TotalCostNightWithLunchRequest request)
        {
            TotalCostNightWithLunchResponse response = new TotalCostNightWithLunchResponse();
           response.TotalCostNightWithLunch = request.AnyCostIsraelStation ? request.ParamNightWithLunch * request.NightWithLunch : 0;
            return response;
        }

        protected override TotalCostNightWithLunchRequest LoadRequest()
        {
            var result = new TotalCostNightWithLunchRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region SectionAnyCostIsraelStation
            if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
                result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            if (target.Attributes.Contains("new_anycostisraelstation"))
                result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            #endregion

            #region TotalCostNightWithLunch
            if (preImage != null && preImage.new_TotalCostNightWithLunch.HasValue)
                result.TotalCostNightWithLunch = preImage.new_TotalCostNightWithLunch.HasValue ? preImage.new_TotalCostNightWithLunch.Value : 0;
            if (target.Attributes.Contains("new_totalcostnightwithlunch"))
                result.TotalCostNightWithLunch = target.new_TotalCostNightWithLunch.HasValue ? target.new_TotalCostNightWithLunch.Value : 0;

            if (preImage != null && preImage.new_NightWithLunch.HasValue)
                result.NightWithLunch = preImage.new_NightWithLunch.HasValue ? preImage.new_NightWithLunch.Value : 0;
            if (target.Attributes.Contains("new_nightwithlunch"))
                result.NightWithLunch = target.new_NightWithLunch.HasValue ? target.new_NightWithLunch.Value : 0;

            if (parameters.Attributes.Contains("new_nightwithlunch"))
                result.ParamNightWithLunch = parameters.new_NightWithLunch.HasValue ? parameters.new_NightWithLunch.Value : 0;

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostNightWithLunchResponse response)
        {
            return response.TotalCostNightWithLunch;
        }
    }


}
