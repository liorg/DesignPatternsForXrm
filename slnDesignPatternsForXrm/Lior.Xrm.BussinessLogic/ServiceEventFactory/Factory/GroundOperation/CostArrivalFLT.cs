using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCostArrivalFLTResponse
    {
        public decimal TotalCostArrival { get; set; }
    }

    public class TotalCostArrivalFLTRequest : IGroundOperation
    {
        public decimal TotalHotelcostarrival { get; set; }
        public decimal TotalMealscostarrival { get; set; }
        public decimal TotalTransportationcostarrival { get; set; }
        public decimal TotalCostCompensationsDGExtratktDBCRF2 { get; set; }
        public decimal AdditionalCostsarrivalcost { get; set; }

        public bool Anycostgroundoperation { get; set; }
        public decimal TotalCostArrival { get; set; }
    }
    //Total CostArrivalFLT
    public class XrmCostArrivalFLT : XrmDecoratorV2<TotalCostArrivalFLTResponse>
    {
        public XrmCostArrivalFLT(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostArrivalFLTResponse response)
        {
            _new_incidentservice.new_TotalCostArrival = response.TotalCostArrival;

        }
    }

    public class CostArrivalFLT : ParticipantOnCalculator<TotalCostArrivalFLTRequest, TotalCostArrivalFLTResponse>
    {

        public CostArrivalFLT(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostArrivalFLTResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCostArrivalFLTResponse CalcTotal(TotalCostArrivalFLTRequest request)
        {
            var response = new TotalCostArrivalFLTResponse();
            response.TotalCostArrival = request.Anycostgroundoperation ? request.TotalCostCompensationsDGExtratktDBCRF2 +
                request.TotalHotelcostarrival + request.TotalMealscostarrival +
                request.TotalTransportationcostarrival + request.AdditionalCostsarrivalcost
                                    : 0;
            return response;
        }

        protected override TotalCostArrivalFLTRequest LoadRequest()
        {
            var result = new TotalCostArrivalFLTRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region   Anycostgroundoperation

            if (preImage != null && preImage.new_AnyCostGroundOperation.HasValue)
                result.Anycostgroundoperation = preImage.new_AnyCostGroundOperation.HasValue ? preImage.new_AnyCostGroundOperation.Value : false;
            if (target.Attributes.Contains("new_AnyCostGroundOperation".ToLower()))
                result.Anycostgroundoperation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostGroundOperation.Value : false;
            #endregion

            #region TotalCost
            //-----
            if (preImage != null && preImage.new_TotalCostArrival.HasValue)
                result.TotalCostArrival = preImage.new_TotalCostArrival.HasValue ? preImage.new_TotalCostArrival.Value : 0;
            if (target.Attributes.Contains("new_TotalCostArrival".ToLower()))
                result.TotalCostArrival = target.new_TotalCostArrival.HasValue ? target.new_TotalCostArrival.Value : 0;
            //------

            if (preImage != null && preImage.new_TotalHotelsCostArrival.HasValue)
                result.TotalHotelcostarrival = preImage.new_TotalHotelsCostArrival.HasValue ? preImage.new_TotalHotelsCostArrival.Value : 0;
            if (target.Attributes.Contains("new_TotalHotelsCostArrival".ToLower()))
                result.TotalHotelcostarrival = target.new_TotalHotelsCostArrival.HasValue ? target.new_TotalHotelsCostArrival.Value : 0;

            if (preImage != null && preImage.new_TotalMealsCostArrival.HasValue)
                result.TotalMealscostarrival = preImage.new_TotalHotelsCostArrival.HasValue ? preImage.new_TotalMealsCostArrival.Value : 0;
            if (target.Attributes.Contains("new_TotalMealsCostArrival".ToLower()))
                result.TotalMealscostarrival = target.new_TotalMealsCostArrival.HasValue ? target.new_TotalMealsCostArrival.Value : 0;

            if (preImage != null && preImage.new_TotalTransportationCostArrival.HasValue)
                result.TotalTransportationcostarrival = preImage.new_TotalTransportationCostArrival.HasValue ? preImage.new_TotalTransportationCostArrival.Value : 0;
            if (target.Attributes.Contains("new_TotalTransportationCostArrival".ToLower()))
                result.TotalTransportationcostarrival = target.new_TotalTransportationCostArrival.HasValue ? target.new_TotalTransportationCostArrival.Value : 0;

            if (preImage != null && preImage.new_AdditionalCostsArrivalCost.HasValue)
                result.AdditionalCostsarrivalcost = preImage.new_AdditionalCostsArrivalCost.HasValue ? preImage.new_AdditionalCostsArrivalCost.Value : 0;
            if (target.Attributes.Contains("new_AdditionalCostsArrivalCost".ToLower()))
                result.AdditionalCostsarrivalcost = target.new_AdditionalCostsArrivalCost.HasValue ? target.new_AdditionalCostsArrivalCost.Value : 0;


            if (preImage != null && preImage.new_TotalCostCompensationsDGExtratktDBCRF2.HasValue)
                result.TotalCostCompensationsDGExtratktDBCRF2 = preImage.new_TotalCostCompensationsDGExtratktDBCRF2.HasValue ? preImage.new_TotalCostCompensationsDGExtratktDBCRF2.Value : 0;
            if (target.Attributes.Contains("new_TotalCostCompensationsDGExtratktDBCRF2".ToLower()))
                result.TotalCostCompensationsDGExtratktDBCRF2 = target.new_TotalCostCompensationsDGExtratktDBCRF2.HasValue ? target.new_TotalCostCompensationsDGExtratktDBCRF2.Value : 0;


            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostArrivalFLTResponse response)
        {
            return response.TotalCostArrival;
        }
    }


}
