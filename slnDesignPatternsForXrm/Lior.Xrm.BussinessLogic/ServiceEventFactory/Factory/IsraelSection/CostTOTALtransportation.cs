using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;
using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;


namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{

    public class TOTALtransportationResponse
    {
        public decimal TOTALtransportation { get; set; }
    }

    public class TOTALtransportationRequest : ISectionAnyIsraelStation
    {
      public   decimal TotalCostBus { get; set; }
      public  decimal TotalCostTaxi { get; set; }

      public  decimal AdditionalCoststransportation { get; set; }

         public decimal TOTALtransportation { get; set; }

         public bool AnyCostIsraelStation
         {
              get; set; 
         }
    }

    public class XrmCostTOTALtransportation : XrmDecoratorV2<TOTALtransportationResponse>
    {
        public XrmCostTOTALtransportation(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TOTALtransportationResponse response)
        {
            _new_incidentservice.new_TOTALtransportation = response.TOTALtransportation;

        }
    }

    public class CostTOTALtransportation : ParticipantOnCalculator<TOTALtransportationRequest, TOTALtransportationResponse>
    {

        public CostTOTALtransportation(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TOTALtransportationResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }

        protected override TOTALtransportationResponse CalcTotal(TOTALtransportationRequest request)
        {
            decimal sumOfcostTotalBus = _depencies.ContainsKey("TotalBus") ? _depencies["TotalBus"].GetTotal() : 0;
            decimal sumOfcostTotalTaxi = _depencies.ContainsKey("TotalTaxi") ? _depencies["TotalTaxi"].GetTotal() : 0;
            TOTALtransportationResponse response = new TOTALtransportationResponse();
            response.TOTALtransportation = request.AnyCostIsraelStation ? request.AdditionalCoststransportation + sumOfcostTotalBus + sumOfcostTotalTaxi : 0;//request.TotalCostBus + request.TotalCostTaxi : 0;
            return response;
        }

        protected override TOTALtransportationRequest LoadRequest()
        {
            var result = new TOTALtransportationRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region SectionAnyCostIsraelStation
            if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
                result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            if (target.Attributes.Contains("new_anycostisraelstation"))
                result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            #endregion

            
              #region TOTALtransportation
            //if (preImage != null && preImage.new_TOTALtransportation.HasValue)
            //    result.TOTALtransportation = preImage.new_TOTALtransportation.HasValue ? preImage.new_TOTALtransportation.Value : 0;
            //if (target.Attributes.Contains("new_totaltransportation"))
            //    result.TOTALtransportation = target.new_TOTALtransportation.HasValue ? target.new_TOTALtransportation.Value : 0;

            if (preImage != null && preImage.new_AdditionalCoststransportation.HasValue)
                result.AdditionalCoststransportation = preImage.new_AdditionalCoststransportation.HasValue ? preImage.new_AdditionalCoststransportation.Value : 0;
            
            if (target.Attributes.Contains("new_AdditionalCoststransportation".ToLower()))
                result.AdditionalCoststransportation = target.new_AdditionalCoststransportation.HasValue ? target.new_AdditionalCoststransportation.Value : 0;

            #endregion

            return result;
        }



        public override decimal GetTotal(TOTALtransportationResponse response)
        {
            return response.TOTALtransportation;
        }
    }


}
