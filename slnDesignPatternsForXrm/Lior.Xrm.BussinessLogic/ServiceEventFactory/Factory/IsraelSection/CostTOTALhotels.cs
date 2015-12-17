using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TOTALhotelsResponse
    {
        public decimal TOTALhotels { get; set; }
    }

    public class TOTALhotelsRequest : ISectionAnyIsraelStation
    {
        public decimal TotalCostNightMostOfTheYear { get; set; }
        public decimal TotalCostNightSummerAndHolidays { get; set; }
        public decimal TotalCostNightWithBreakfast { get; set; }
        public decimal TotalCostNightWithLunch { get; set; }
        public decimal TotalCostNightWithDinner { get; set; }

        public decimal AdditionalCostshotels { get; set; }
        public bool AnyCostIsraelStation { get; set; }

        public decimal TOTALhotels { get; set; }
    }

    public class XrmCostTOTALhotels : XrmDecoratorV2<TOTALhotelsResponse>
    {
        public XrmCostTOTALhotels(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TOTALhotelsResponse response)
        {
            _new_incidentservice.new_TOTALhotels = response.TOTALhotels;
           
        }
    }

    public class CostTOTALhotels : ParticipantOnCalculator<TOTALhotelsRequest, TOTALhotelsResponse>
    {

        public CostTOTALhotels(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TOTALhotelsResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TOTALhotelsResponse CalcTotal(TOTALhotelsRequest request)
        {
            TOTALhotelsResponse response = new TOTALhotelsResponse();
            decimal sumOftotalCostNightMostOfTheYear = _depencies.ContainsKey("TotalCostNightMostOfTheYear") ? _depencies["TotalCostNightMostOfTheYear"].GetTotal() : 0;
            decimal sumOftotalCostNightSummerAndHolidays = _depencies.ContainsKey("TotalCostNightSummerAndHolidays") ? _depencies["TotalCostNightSummerAndHolidays"].GetTotal() : 0;
            decimal sumOftotalCostNightWithBreakfast = _depencies.ContainsKey("TotalCostNightWithBreakfast") ? _depencies["TotalCostNightWithBreakfast"].GetTotal() : 0;
            decimal sumOftotalCostNightWithLunch = _depencies.ContainsKey("TotalCostNightWithLunch") ? _depencies["TotalCostNightWithLunch"].GetTotal() : 0;
            decimal sumOftotalCostNightWithDinner = _depencies.ContainsKey("TotalCostNightWithDinner") ? _depencies["TotalCostNightWithDinner"].GetTotal() : 0;
            
            response.TOTALhotels = request.AnyCostIsraelStation ? request.AdditionalCostshotels+
                                sumOftotalCostNightMostOfTheYear + sumOftotalCostNightSummerAndHolidays + sumOftotalCostNightWithBreakfast +
                                sumOftotalCostNightWithLunch + sumOftotalCostNightWithDinner : 0;
            return response;
        }

        protected override TOTALhotelsRequest LoadRequest()
        {
            var result = new TOTALhotelsRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region SectionAnyCostIsraelStation
            if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
                result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            if (target.Attributes.Contains("new_anycostisraelstation"))
                result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            #endregion

            #region TotalCostBus
            //if (preImage != null && preImage.new_TotalCostNightMostOfTheYear.HasValue)
            //    result.TotalCostNightMostOfTheYear = preImage.new_TotalCostNightMostOfTheYear.HasValue ? preImage.new_TotalCostNightMostOfTheYear.Value : 0;
            //if (target.Attributes.Contains("new_totalcostnightmostfftheyear"))
            //    result.TotalCostNightMostOfTheYear = target.new_TotalCostNightMostOfTheYear.HasValue ? target.new_TotalCostNightMostOfTheYear.Value : 0;

            // if (preImage != null && preImage.new_TotalCostNightSummerAndHolidays.HasValue)
            //    result.TotalCostNightSummerAndHolidays = preImage.new_TotalCostNightSummerAndHolidays.HasValue ? preImage.new_TotalCostNightSummerAndHolidays.Value : 0;
            //if (target.Attributes.Contains("new_totalcostnightsummerandHolidays".ToLower()))
            //    result.TotalCostNightSummerAndHolidays = target.new_TotalCostNightSummerAndHolidays.HasValue ? target.new_TotalCostNightSummerAndHolidays.Value : 0;

            //if (preImage != null && preImage.new_TotalCostNightWithBreakfast.HasValue)
            //    result.TotalCostNightWithBreakfast = preImage.new_TotalCostNightWithBreakfast.HasValue ? preImage.new_TotalCostNightWithBreakfast.Value : 0;
            //if (target.Attributes.Contains("new_TotalCostNightWithBreakfast".ToLower()))
            //    result.TotalCostNightWithBreakfast = target.new_TotalCostNightWithBreakfast.HasValue ? target.new_TotalCostNightWithBreakfast.Value : 0;

            //if (preImage != null && preImage.new_TotalCostNightWithLunch.HasValue)
            //    result.TotalCostNightWithLunch = preImage.new_TotalCostNightWithLunch.HasValue ? preImage.new_TotalCostNightWithLunch.Value : 0;
            //if (target.Attributes.Contains("new_TotalCostNightWithLunch".ToLower()))
            //    result.TotalCostNightWithLunch = target.new_TotalCostNightWithLunch.HasValue ? target.new_TotalCostNightWithLunch.Value : 0;

            // if (preImage != null && preImage.new_TotalCostNightWithDinner.HasValue)
            //    result.TotalCostNightWithDinner = preImage.new_TotalCostNightWithDinner.HasValue ? preImage.new_TotalCostNightWithDinner.Value : 0;
            //if (target.Attributes.Contains("new_TotalCostNightWithDinner".ToLower()))
            //    result.TotalCostNightWithDinner = target.new_TotalCostNightWithDinner.HasValue ? target.new_TotalCostNightWithDinner.Value : 0;

            if (preImage != null && preImage.new_AdditionalCostshotels.HasValue)
                result.AdditionalCostshotels = preImage.new_AdditionalCostshotels.HasValue ? preImage.new_AdditionalCostshotels.Value : 0;
            if (target.Attributes.Contains("new_AdditionalCostshotels".ToLower()))
                result.AdditionalCostshotels = target.new_AdditionalCostshotels.HasValue ? target.new_AdditionalCostshotels.Value : 0;

            #endregion


            return result;
        }


        public override decimal GetTotal(TOTALhotelsResponse response)
        {
            return response.TOTALhotels;
        }
    }


}
