using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    //TotalCostDepartureFLT
    public class TotalCostDepartureFLTResponse
    {
        public decimal TotalCostDepartureCost { get; set; }
    }

    public class TotalCostDepartureFLTRequest : IGroundOperation
    {
        public decimal TotalHotelcostDeparture { get; set; }
        public decimal TotalMealscostDeparture { get; set; }
        public decimal TotalTransportationcostDeparture { get; set; }
        public decimal TotalCostCompensationsDGExtratktDBCRFNGround { get; set; }
        public decimal AdditionalCostsDeparture { get; set; }

        public bool Anycostgroundoperation { get; set; }
        public decimal TotalCostDepartureCost { get; set; }
    }
    //Total CostArrivalFLT
    public class XrmCostDepartureFLT : XrmDecoratorV2<TotalCostDepartureFLTResponse>
    {
        public XrmCostDepartureFLT(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostDepartureFLTResponse response)
        {
            _new_incidentservice.new_TotalDepartureCosts = response.TotalCostDepartureCost;

        }
    }

    public class CostDepartureFLT : ParticipantOnCalculator<TotalCostDepartureFLTRequest, TotalCostDepartureFLTResponse>
    {

        public CostDepartureFLT(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostDepartureFLTResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCostDepartureFLTResponse CalcTotal(TotalCostDepartureFLTRequest request)
        {
            var response = new TotalCostDepartureFLTResponse();
            response.TotalCostDepartureCost = request.Anycostgroundoperation ? request.TotalCostCompensationsDGExtratktDBCRFNGround +
                request.TotalHotelcostDeparture + request.TotalMealscostDeparture +
                request.TotalTransportationcostDeparture + request.AdditionalCostsDeparture
                                    : 0;
            return response;
        }

        protected override TotalCostDepartureFLTRequest LoadRequest()
        {
            var result = new TotalCostDepartureFLTRequest();
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
            if (preImage != null && preImage.new_TotalDepartureCosts.HasValue)
                result.TotalCostDepartureCost = preImage.new_TotalDepartureCosts.HasValue ? preImage.new_TotalDepartureCosts.Value : 0;
            if (target.Attributes.Contains("new_TotalDepartureCosts".ToLower()))
                result.TotalCostDepartureCost = target.new_TotalDepartureCosts.HasValue ? target.new_TotalDepartureCosts.Value : 0;
            //------

            if (preImage != null && preImage.new_TotalHotel2.HasValue)
                result.TotalHotelcostDeparture = preImage.new_TotalHotel2.HasValue ? preImage.new_TotalHotel2.Value : 0;
            if (target.Attributes.Contains("new_TotalHotel2".ToLower()))
                result.TotalHotelcostDeparture = target.new_TotalHotel2.HasValue ? target.new_TotalHotel2.Value : 0;

            if (preImage != null && preImage.new_TotalMeals2.HasValue)
                result.TotalMealscostDeparture = preImage.new_TotalMeals2.HasValue ? preImage.new_TotalMeals2.Value : 0;
            if (target.Attributes.Contains("new_TotalMeals2".ToLower()))
                result.TotalMealscostDeparture = target.new_TotalMeals2.HasValue ? target.new_TotalMeals2.Value : 0;

            if (preImage != null && preImage.new_TotalTransportation2.HasValue)
                result.TotalTransportationcostDeparture = preImage.new_TotalTransportation2.HasValue ? preImage.new_TotalTransportation2.Value : 0;
            if (target.Attributes.Contains("new_TotalTransportation2".ToLower()))
                result.TotalTransportationcostDeparture = target.new_TotalTransportation2.HasValue ? target.new_TotalTransportation2.Value : 0;

            if (preImage != null && preImage.new_AdditionalCostsDeparture.HasValue)
                result.AdditionalCostsDeparture = preImage.new_AdditionalCostsDeparture.HasValue ? preImage.new_AdditionalCostsDeparture.Value : 0;
            if (target.Attributes.Contains("new_AdditionalCostsDeparture".ToLower()))
                result.AdditionalCostsDeparture = target.new_AdditionalCostsDeparture.HasValue ? target.new_AdditionalCostsDeparture.Value : 0;


            if (preImage != null && preImage.new_CompensationsDGExtratktDBCRFNDetcGround.HasValue)
                result.TotalCostCompensationsDGExtratktDBCRFNGround = preImage.new_CompensationsDGExtratktDBCRFNDetcGround.HasValue ? preImage.new_CompensationsDGExtratktDBCRFNDetcGround.Value : 0;
            if (target.Attributes.Contains("new_CompensationsDGExtratktDBCRFNDetcGround".ToLower()))
                result.TotalCostCompensationsDGExtratktDBCRFNGround = target.new_CompensationsDGExtratktDBCRFNDetcGround.HasValue ? target.new_CompensationsDGExtratktDBCRFNDetcGround.Value : 0;


            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostDepartureFLTResponse response)
        {
            return response.TotalCostDepartureCost;
        }
    }


}
