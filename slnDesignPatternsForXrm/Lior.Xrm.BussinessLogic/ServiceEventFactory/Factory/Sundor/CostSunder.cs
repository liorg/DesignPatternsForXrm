using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    //TotalCostSunder
    public class TotalCostSunderResponse
    {
        public decimal TotalSundor { get; set; }
    }

    public class TotalCostSunderRequest : ISundor
    {
        public decimal StationCostsOffline { get; set; }
        public decimal CommercialCost { get; set; }

        public bool Anycostsundor { get; set; }
        public decimal TotalSundor { get; set; }

    }
    //Total CostArrivalFLT
    public class XrmCostSunder : XrmDecoratorV2<TotalCostSunderResponse>
    {
        public XrmCostSunder(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostSunderResponse response)
        {
            _new_incidentservice.new_TotalSundor = response.TotalSundor;

        }
    }

    public class CostSunder : ParticipantOnCalculator<TotalCostSunderRequest, TotalCostSunderResponse>
    {

        public CostSunder(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostSunderResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCostSunderResponse CalcTotal(TotalCostSunderRequest request)
        {
            var response = new TotalCostSunderResponse();
            
            response.TotalSundor = request.Anycostsundor ? request.CommercialCost + request.StationCostsOffline : 0;
            return response;
        }

        protected override TotalCostSunderRequest LoadRequest()
        {
            var result = new TotalCostSunderRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;
            var orginal = _openClosedCalculator.GetOriginalFlight();

            #region   Anycostsundor
            if (orginal != null && orginal.new_Airline != null && orginal.new_Airline.Value == 100000001)
            {
                if (target.Attributes.Contains("new_AnyCostSundor".ToLower()))
                   target.Attributes.Remove("new_AnyCostSundor".ToLower());
                
                target.Attributes.Add("new_anycostsundor", false);
            }

            if (preImage != null && preImage.new_AnyCostSundor.HasValue)
                result.Anycostsundor = preImage.new_AnyCostSundor.HasValue ? preImage.new_AnyCostSundor.Value : false;
            if (target.Attributes.Contains("new_AnyCostSundor".ToLower()))
                result.Anycostsundor = target.new_AnyCostSundor.HasValue ? target.new_AnyCostSundor.Value : false;


            #endregion

            #region TotalCost
            //-----
            if (preImage != null && preImage.new_TotalSundor.HasValue)
                result.TotalSundor = preImage.new_TotalSundor.HasValue ? preImage.new_TotalSundor.Value : 0;
            if (target.Attributes.Contains("new_TotalSundor".ToLower()))
                result.TotalSundor = target.new_TotalSundor.HasValue ? target.new_TotalSundor.Value : 0;
            //------

            if (preImage != null && preImage.new_StationCostsOffline.HasValue)
                result.StationCostsOffline = preImage.new_StationCostsOffline.HasValue ? preImage.new_StationCostsOffline.Value : 0;
            if (target.Attributes.Contains("new_StationCostsOffline".ToLower()))
                result.StationCostsOffline = target.new_StationCostsOffline.HasValue ? target.new_StationCostsOffline.Value : 0;

            if (preImage != null && preImage.new_CommercialCost.HasValue)
                result.CommercialCost = preImage.new_CommercialCost.HasValue ? preImage.new_CommercialCost.Value : 0;
            if (target.Attributes.Contains("new_CommercialCost".ToLower()))
                result.CommercialCost = target.new_CommercialCost.HasValue ? target.new_CommercialCost.Value : 0;


            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostSunderResponse response)
        {
            return response.TotalSundor;
        }
    }


}
