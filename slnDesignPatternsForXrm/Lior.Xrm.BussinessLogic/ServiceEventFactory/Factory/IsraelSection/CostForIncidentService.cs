using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
   
    public class TotalCostForIncidentServiceRequest //: ISectionAnyIsraelStation
    {
        public decimal TotalCallCenter { get; set; }
        public decimal TotalCashier { get; set; }
        public decimal TotalGroundOperation { get; set; }
        public decimal TotalFoodandprovisions { get; set; }
        public int EstimatedCost { get; set; }
        public decimal CostDifferenceround { get; set; }
        public decimal Totalplacingteam { get; set; }
        public decimal TotalCargo { get; set; }
        public decimal TotalSundor { get; set; }
        public decimal TotalCostForIncidentService { get; set; }

       // public bool AnyCostIsraelStation  { get; set; }

        
    }

    public class TotalCostForIncidentServiceResponse
    {
        public decimal TotalCostForIncidentService { get; set; }
    }

    public class XrmCostForIncidentService : XrmDecoratorV2<TotalCostForIncidentServiceResponse>
    {
        public XrmCostForIncidentService(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostForIncidentServiceResponse response)
        {
            _new_incidentservice.new_TotalCostForIncidentService = response.TotalCostForIncidentService;

        }
    }

    public class CostForIncidentService : ParticipantOnCalculator<TotalCostForIncidentServiceRequest, TotalCostForIncidentServiceResponse>
    {


        public CostForIncidentService(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostForIncidentServiceResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }

        

        protected override TotalCostForIncidentServiceResponse CalcTotal(TotalCostForIncidentServiceRequest request)
        {

            TotalCostForIncidentServiceResponse response = new TotalCostForIncidentServiceResponse();

            decimal sumOftotalIsraelStation = _depencies.ContainsKey("costIsraelStation") ? _depencies["costIsraelStation"].GetTotal() : 0;
            decimal sumOftotalCallCenter = _depencies.ContainsKey("costTotalCallCenter") ? _depencies["costTotalCallCenter"].GetTotal() : 0;
            decimal sumOfcostTotalCostCashier = _depencies.ContainsKey("costTotalCostCashier") ? _depencies["costTotalCostCashier"].GetTotal() : 0;
            decimal sumOfcostGroundOperations = _depencies.ContainsKey("costGroundOperations") ? _depencies["costGroundOperations"].GetTotal() : 0;
            decimal sumOfcostCrews = _depencies.ContainsKey("costCrews") ? _depencies["costCrews"].GetTotal() : 0;
            decimal sumOfcostSunder = _depencies.ContainsKey("costSunder") ? _depencies["costSunder"].GetTotal() : 0;
            response.TotalCostForIncidentService = sumOftotalIsraelStation+sumOftotalCallCenter + sumOfcostTotalCostCashier + sumOfcostGroundOperations +
                request.TotalFoodandprovisions + request.EstimatedCost
                + request.CostDifferenceround + sumOfcostCrews + request.TotalCargo + sumOfcostSunder;
            // response.TotalCostForIncidentService = request.TotalCallCenter + request.TotalCashier + request.TotalGroundOperation + request.TotalFoodandprovisions + request.EstimatedCost +
            //request.CostDifferenceround + request.Totalplacingteam + request.TotalCargo + request.TotalSundor;
            return response;
        }

        protected override TotalCostForIncidentServiceRequest LoadRequest()
        {
            var result = new TotalCostForIncidentServiceRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            //#region SectionAnyCostIsraelStation
            //if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
            //    result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            //if (target.Attributes.Contains("new_anycostisraelstation"))
            //    result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            //#endregion

           
            #region TotalCostForIncidentService
            if (preImage != null && preImage.new_TotalCallCenter.HasValue)
                result.TotalCallCenter = preImage.new_TotalCallCenter.HasValue ? preImage.new_TotalCallCenter.Value : 0;
            if (target.Attributes.Contains("new_totalcallcenter"))
                result.TotalCallCenter = target.new_TotalCallCenter.HasValue ? target.new_TotalCallCenter.Value : 0;

            if (preImage != null && preImage.new_TotalCashier.HasValue)
                result.TotalCashier = preImage.new_TotalCashier.HasValue ? preImage.new_TotalCashier.Value : 0;
            if (target.Attributes.Contains("new_totalcashier"))
                result.TotalCashier = target.new_TotalCashier.HasValue ? target.new_TotalCashier.Value : 0;

            if (preImage != null && preImage.new_Totalgroundoperation.HasValue)
                result.TotalGroundOperation = preImage.new_Totalgroundoperation.HasValue ? preImage.new_Totalgroundoperation.Value : 0;
            if (target.Attributes.Contains("new_totalgroundoperation"))
                result.TotalGroundOperation = target.new_Totalgroundoperation.HasValue ? target.new_Totalgroundoperation.Value : 0;

            if (preImage != null && preImage.new_TotalFoodandprovisions.HasValue)
                result.TotalFoodandprovisions = preImage.new_TotalFoodandprovisions.HasValue ? preImage.new_TotalFoodandprovisions.Value : 0;
            if (target.Attributes.Contains("new_totalfoodandprovisions"))
                result.TotalFoodandprovisions = target.new_TotalFoodandprovisions.HasValue ? target.new_TotalFoodandprovisions.Value : 0;

            if (preImage != null && preImage.new_EstimatedCost.HasValue)
                result.EstimatedCost = preImage.new_EstimatedCost.HasValue ? preImage.new_EstimatedCost.Value : 0;
            if (target.Attributes.Contains("new_estimatedcost"))
                result.EstimatedCost = target.new_EstimatedCost.HasValue ? target.new_EstimatedCost.Value : 0;

            if (preImage != null && preImage.new_CostDifferenceround.HasValue)
                result.CostDifferenceround = preImage.new_CostDifferenceround.HasValue ? preImage.new_CostDifferenceround.Value : 0;
            if (target.Attributes.Contains("new_costdifferenceround"))
                result.CostDifferenceround = target.new_CostDifferenceround.HasValue ? target.new_CostDifferenceround.Value : 0;

            if (preImage != null && preImage.new_Totalplacingteam.HasValue)
                result.Totalplacingteam = preImage.new_Totalplacingteam.HasValue ? preImage.new_Totalplacingteam.Value : 0;
            if (target.Attributes.Contains("new_totalplacingteam"))
                result.Totalplacingteam = target.new_Totalplacingteam.HasValue ? target.new_Totalplacingteam.Value : 0;

            if (preImage != null && preImage.new_TotalCargo.HasValue)
                result.TotalCargo = preImage.new_TotalCargo.HasValue ? preImage.new_TotalCargo.Value : 0;
            if (target.Attributes.Contains("new_totalcargo"))
                result.TotalCargo = target.new_TotalCargo.HasValue ? target.new_TotalCargo.Value : 0;

            if (preImage != null && preImage.new_TotalSundor.HasValue)
                result.TotalSundor = preImage.new_TotalSundor.HasValue ? preImage.new_TotalSundor.Value : 0;
            if (target.Attributes.Contains("new_totalsundor"))
                result.TotalSundor = target.new_TotalSundor.HasValue ? target.new_TotalSundor.Value : 0;

            if (preImage != null && preImage.new_TotalCostForIncidentService.HasValue)
                result.TotalCostForIncidentService = preImage.new_TotalCostForIncidentService.HasValue ? preImage.new_TotalCostForIncidentService.Value : 0;
            if (target.Attributes.Contains("new_totalcostforIncidentservice"))
                result.TotalCostForIncidentService = target.new_TotalCostForIncidentService.HasValue ? target.new_TotalCostForIncidentService.Value : 0;

            #endregion

            return result;
        }



        public override decimal GetTotal(TotalCostForIncidentServiceResponse response)
        {
            return response.TotalCostForIncidentService;
        }
    }


}
