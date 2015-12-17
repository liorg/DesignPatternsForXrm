using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{


    //CostHotMeal
    public class TotalCostHotMealRequest : ISectionAnyIsraelStation
    {
        public decimal HotMeal { get; set; }
        public decimal ParamHotMeal { get; set; }
        public bool AnyCostIsraelStation { get; set; }
        public decimal TotalCostHotMeal { get; set; }
    }
    public class TotalCostHotMealResponse
    {
        public decimal TotalCostHotMeal { get; set; }
    }

    public class XrmCostHotMeal : XrmDecoratorV2<TotalCostHotMealResponse>
    {
        public XrmCostHotMeal(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostHotMealResponse response)
        {
            _new_incidentservice.new_TotalCostHotMeal = response.TotalCostHotMeal;

        }
    }

    public class CostHotMeal : ParticipantOnCalculator<TotalCostHotMealRequest, TotalCostHotMealResponse>
    {

        public CostHotMeal(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostHotMealResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCostHotMealResponse CalcTotal(TotalCostHotMealRequest request)
        {
            TotalCostHotMealResponse response = new TotalCostHotMealResponse();
            response.TotalCostHotMeal = request.AnyCostIsraelStation ? request.ParamHotMeal * request.HotMeal : 0;
            return response;
        }

        protected override TotalCostHotMealRequest LoadRequest()
        {
            var result = new TotalCostHotMealRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region SectionAnyCostIsraelStation
            if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
                result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            if (target.Attributes.Contains("new_anycostisraelstation"))
                result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            #endregion

            #region TotalCostHotMeal
            if (preImage != null && preImage.new_TotalCostHotMeal.HasValue)
                result.TotalCostHotMeal = preImage.new_TotalCostHotMeal.HasValue ? preImage.new_TotalCostHotMeal.Value : 0;
            if (target.Attributes.Contains("new_totalcosthotmeal"))
                result.TotalCostHotMeal = target.new_TotalCostHotMeal.HasValue ? target.new_TotalCostHotMeal.Value : 0;

            if (preImage != null && preImage.new_HotMeal.HasValue)
                result.HotMeal = preImage.new_HotMeal.HasValue ? preImage.new_HotMeal.Value : 0;
            if (target.Attributes.Contains("new_hotmeal"))
                result.HotMeal = target.new_HotMeal.HasValue ? target.new_HotMeal.Value : 0;

            if (parameters.Attributes.Contains("new_hotmeal"))
                result.ParamHotMeal = parameters.new_HotMeal.HasValue ? parameters.new_HotMeal.Value : 0;

            #endregion

            return result;
        }



        public override decimal GetTotal(TotalCostHotMealResponse response)
        {
            return response.TotalCostHotMeal;
        }
    }


}
