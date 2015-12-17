using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{

    public class TotalCostTaxiResponse
    {
        public decimal TotalCostTaxi { get; set; }
    }

    public class TotalCostTaxRequest : ISectionAnyIsraelStation
    {
        public decimal Taxi { get; set; }
        public decimal ParamTaxi { get; set; }
        public bool AnyCostIsraelStation { get; set; }

        public decimal TotalCostTaxi { get; set; }
    }

    public class XrmCostTaxi : XrmDecoratorV2<TotalCostTaxiResponse>
    {
        public XrmCostTaxi(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostTaxiResponse response)
        {
            _new_incidentservice.new_TotalCostTaxi = response.TotalCostTaxi;

        }
    }

    public class CostTaxi : ParticipantOnCalculator<TotalCostTaxRequest, TotalCostTaxiResponse>
    {
        public CostTaxi(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostTaxiResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }

        protected override TotalCostTaxiResponse CalcTotal(TotalCostTaxRequest request)
        {
            TotalCostTaxiResponse response = new TotalCostTaxiResponse();
            response.TotalCostTaxi = request.AnyCostIsraelStation ? request.ParamTaxi * request.Taxi : 0;
            return response;
        }

        protected override TotalCostTaxRequest LoadRequest()
        {
            var result = new TotalCostTaxRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region SectionAnyCostIsraelStation
            if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
                result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            if (target.Attributes.Contains("new_anycostisraelstation"))
                result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            #endregion

            #region TotalCostTaxi
            if (preImage != null && preImage.new_TotalCostTaxi.HasValue)
                result.TotalCostTaxi = preImage.new_TotalCostTaxi.HasValue ? preImage.new_TotalCostTaxi.Value : 0;
            if (target.Attributes.Contains("new_totalcosttaxi"))
                result.TotalCostTaxi = target.new_TotalCostTaxi.HasValue ? target.new_TotalCostTaxi.Value : 0;

            if (preImage != null && preImage.new_Taxi.HasValue)
                result.Taxi = preImage.new_Taxi.HasValue ? preImage.new_Taxi.Value : 0;
            if (target.Attributes.Contains("new_taxi"))
                result.Taxi = target.new_Taxi.HasValue ? target.new_Taxi.Value : 0;

            if (parameters.Attributes.Contains("new_taxi"))
                result.ParamTaxi = parameters.new_Taxi.HasValue ? parameters.new_Taxi.Value : 0;
            else
                result.ParamTaxi = 0;
            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostTaxiResponse response)
        {
            return response.TotalCostTaxi;
        }
    }



}
