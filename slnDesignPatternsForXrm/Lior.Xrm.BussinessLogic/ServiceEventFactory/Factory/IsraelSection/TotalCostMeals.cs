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
    public class TotalCostMealsRequest : ISectionAnyIsraelStation
    {

        public decimal Additionalcostsmeals { get; set; }//new_Additionalcostsmeals
        public bool AnyCostIsraelStation { get; set; }
      
    }
    public class TotalCostMealsResponse
    {
        public decimal TOTALmeals { get; set; }//new_TOTALmeals
    }

    public class XrmTotalCostMeals : XrmDecoratorV2<TotalCostMealsResponse>
    {
        public XrmTotalCostMeals(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostMealsResponse response)
        {
            _new_incidentservice.new_TOTALmeals = response.TOTALmeals;

        }
    }

    public class TotalCostMeals : ParticipantOnCalculator<TotalCostMealsRequest, TotalCostMealsResponse>
    {

        public TotalCostMeals(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostMealsResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCostMealsResponse CalcTotal(TotalCostMealsRequest request)
        {
            TotalCostMealsResponse response = new TotalCostMealsResponse();
            decimal sumOfcostBreakfast = _depencies.ContainsKey("CostBreakfast") ? _depencies["CostBreakfast"].GetTotal() : 0;
            decimal sumOfcostCoffeeAndCake = _depencies.ContainsKey("CostCoffeeAndCake") ? _depencies["CostCoffeeAndCake"].GetTotal() : 0;
            decimal sumOfcostHotMeal = _depencies.ContainsKey("CostHotMeal") ? _depencies["CostHotMeal"].GetTotal() : 0;
            if (request.AnyCostIsraelStation)
                response.TOTALmeals = request.Additionalcostsmeals + sumOfcostBreakfast + sumOfcostCoffeeAndCake + sumOfcostHotMeal;
            

           return response;
        }

        protected override TotalCostMealsRequest LoadRequest()
        {
            var result = new TotalCostMealsRequest();
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

            if (preImage != null && preImage.new_Additionalcostsmeals.HasValue)
                result.Additionalcostsmeals = preImage.new_Additionalcostsmeals.HasValue ? preImage.new_Additionalcostsmeals.Value : 0;
            if (target.Attributes.Contains("new_Additionalcostsmeals".ToLower()))
                result.Additionalcostsmeals = target.new_Additionalcostsmeals.HasValue ? target.new_Additionalcostsmeals.Value : 0;

        

            #endregion

            return result;
        }



        public override decimal GetTotal(TotalCostMealsResponse response)
        {
            return response.TOTALmeals;
        }
    }


}
