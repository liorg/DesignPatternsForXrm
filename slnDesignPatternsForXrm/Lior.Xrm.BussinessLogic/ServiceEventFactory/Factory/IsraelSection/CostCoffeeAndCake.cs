using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{

    //CostCoffeeAndCake
    public class TotalCostCoffeeAndCakeRequest : ISectionAnyIsraelStation
    {
        public decimal CoffeeAndCake { get; set; }
        public decimal ParamCoffeeAndCake { get; set; }
        public bool AnyCostIsraelStation { get; set; }
        public decimal TotalCostCoffeeAndCake { get; set; }
    }
    public class TotalCostCoffeeAndCakeResponse
    {
        public decimal TotalCostCoffeeAndCake { get; set; }
    }

    public class XrmCostCoffeeAndCake : XrmDecoratorV2<TotalCostCoffeeAndCakeResponse>
    {
        public XrmCostCoffeeAndCake(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostCoffeeAndCakeResponse response)
        {
            _new_incidentservice.new_TotalCostCoffeeAndCake= response.TotalCostCoffeeAndCake;

        }
    }

    public class CostCoffeeAndCake : ParticipantOnCalculator<TotalCostCoffeeAndCakeRequest, TotalCostCoffeeAndCakeResponse>
    {
       
        public CostCoffeeAndCake(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostCoffeeAndCakeResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }

        

        protected override TotalCostCoffeeAndCakeResponse CalcTotal(TotalCostCoffeeAndCakeRequest request)
        {
            TotalCostCoffeeAndCakeResponse response = new TotalCostCoffeeAndCakeResponse();
           response.TotalCostCoffeeAndCake = request.AnyCostIsraelStation ? request.ParamCoffeeAndCake * request.CoffeeAndCake : 0;
            return response;
        }

        protected override TotalCostCoffeeAndCakeRequest LoadRequest()
        {
            var result = new TotalCostCoffeeAndCakeRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region SectionAnyCostIsraelStation
            if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
                result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            if (target.Attributes.Contains("new_anycostisraelstation"))
                result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            #endregion

            #region TotalCostCoffeeAndCake
            if (preImage != null && preImage.new_TotalCostCoffeeAndCake.HasValue)
                result.TotalCostCoffeeAndCake = preImage.new_TotalCostCoffeeAndCake.HasValue ? preImage.new_TotalCostCoffeeAndCake.Value : 0;
            if (target.Attributes.Contains("new_totalcostcoffeeandcake"))
                result.TotalCostCoffeeAndCake = target.new_TotalCostCoffeeAndCake.HasValue ? target.new_TotalCostCoffeeAndCake.Value : 0;

            if (preImage != null && preImage.new_CoffeeAndCake.HasValue)
                result.CoffeeAndCake = preImage.new_CoffeeAndCake.HasValue ? preImage.new_CoffeeAndCake.Value : 0;
            if (target.Attributes.Contains("new_coffeeandcake"))
                result.CoffeeAndCake = target.new_CoffeeAndCake.HasValue ? target.new_CoffeeAndCake.Value : 0;


            if (parameters.Attributes.Contains("new_coffeeandcake"))
                result.ParamCoffeeAndCake = parameters.new_CoffeeAndCake.HasValue ? parameters.new_CoffeeAndCake.Value : 0;
           
            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostCoffeeAndCakeResponse response)
        {
            return response.TotalCostCoffeeAndCake;
        }
    }


}
