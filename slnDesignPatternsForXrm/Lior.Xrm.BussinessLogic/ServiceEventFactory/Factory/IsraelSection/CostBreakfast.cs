using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    //CostBreakfast

    public class TotalCostBreakfastRequest : ISectionAnyIsraelStation
    {
        public decimal Breakfast { get; set; }
        public decimal ParamBreakfast { get; set; }
         public decimal TotalCostBreakfast { get; set; }
        public bool AnyCostIsraelStation
        {
            get;
            set;
        }
    }
    public class TotalCostBreakfastResponse
    {
        public decimal TotalCostBreakfast { get; set; }
    }


    public class XrmCostBreakfast: XrmDecoratorV2<TotalCostBreakfastResponse>
    {
        public XrmCostBreakfast(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostBreakfastResponse response)
        {
            _new_incidentservice.new_TotalCostBreakfast= response.TotalCostBreakfast;

        }
    }

    public class CostBreakfast : ParticipantOnCalculator<TotalCostBreakfastRequest, TotalCostBreakfastResponse>
    {

        public CostBreakfast(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostBreakfastResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }

        

        protected override TotalCostBreakfastResponse CalcTotal(TotalCostBreakfastRequest request)
        {
            TotalCostBreakfastResponse response = new TotalCostBreakfastResponse();
            response.TotalCostBreakfast = request.AnyCostIsraelStation ? request.ParamBreakfast * request.Breakfast : 0;
            return response;
        }

        protected override TotalCostBreakfastRequest LoadRequest()
        {
            var result = new TotalCostBreakfastRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region SectionAnyCostIsraelStation
            if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
                result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            if (target.Attributes.Contains("new_anycostisraelstation"))
                result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            #endregion

           #region TotalCostBreakfast
            if (preImage != null && preImage.new_TotalCostBreakfast.HasValue)
                result.TotalCostBreakfast = preImage.new_TotalCostBreakfast.HasValue ? preImage.new_TotalCostBreakfast.Value : 0;
            if (target.Attributes.Contains("new_totalcostbreakfast"))
                result.TotalCostBreakfast = target.new_TotalCostBreakfast.HasValue ? target.new_TotalCostBreakfast.Value : 0;

            if (preImage != null && preImage.new_Breakfast.HasValue)
                result.Breakfast = preImage.new_Breakfast.HasValue ? preImage.new_Breakfast.Value : 0;
            if (target.Attributes.Contains("new_breakfast"))
                result.Breakfast = target.new_Breakfast.HasValue ? target.new_Breakfast.Value : 0;

            if (parameters.Attributes.Contains("new_breakfast"))
                result.ParamBreakfast = parameters.new_Breakfast.HasValue ? parameters.new_Breakfast.Value : 0;
           
            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostBreakfastResponse response)
        {
            return response.TotalCostBreakfast;
        }
    }


}
