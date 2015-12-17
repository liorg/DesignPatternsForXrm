using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    //CostNightSummerAndHolidays
    public class TotalNightSummerAndHolidaysRequest : ISectionAnyIsraelStation
    {
        public decimal NightSummerAndHolidays { get; set; }
        public decimal ParamNightSummerAndHolidays { get; set; }
        public bool AnyCostIsraelStation { get; set; }

        public decimal TotalCostNightSummerAndHolidays { get; set; }
    }

    public class TotalNightSummerAndHolidaysResponse
    {
        public decimal TotalCostNightSummerAndHolidays { get; set; }
    }

    public class XrmCostNightSummerAndHolidays : XrmDecoratorV2<TotalNightSummerAndHolidaysResponse>
    {
        public XrmCostNightSummerAndHolidays(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalNightSummerAndHolidaysResponse response)
        {
            _new_incidentservice.new_TotalCostNightSummerAndHolidays = response.TotalCostNightSummerAndHolidays;

        }
    }

    public class CostNightSummerAndHolidays : ParticipantOnCalculator<TotalNightSummerAndHolidaysRequest, TotalNightSummerAndHolidaysResponse>
    {

        public CostNightSummerAndHolidays(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalNightSummerAndHolidaysResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalNightSummerAndHolidaysResponse CalcTotal(TotalNightSummerAndHolidaysRequest request)
        {
            TotalNightSummerAndHolidaysResponse response = new TotalNightSummerAndHolidaysResponse();
            response.TotalCostNightSummerAndHolidays = request.AnyCostIsraelStation ? request.ParamNightSummerAndHolidays * request.NightSummerAndHolidays : 0;
            return response;
        }

        protected override TotalNightSummerAndHolidaysRequest LoadRequest()
        {
            var result = new TotalNightSummerAndHolidaysRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region SectionAnyCostIsraelStation
            if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
                result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            if (target.Attributes.Contains("new_anycostisraelstation"))
                result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            #endregion

            #region TotalCostNightSummerAndHolidays
            if (preImage != null && preImage.new_TotalCostNightSummerAndHolidays.HasValue)
                result.TotalCostNightSummerAndHolidays = preImage.new_TotalCostNightSummerAndHolidays.HasValue ? preImage.new_TotalCostNightSummerAndHolidays.Value : 0;
            if (target.Attributes.Contains("new_totalcostnightsummerandholidays"))
                result.TotalCostNightSummerAndHolidays = target.new_TotalCostNightSummerAndHolidays.HasValue ? target.new_TotalCostNightSummerAndHolidays.Value : 0;

            if (preImage != null && preImage.new_NightSummerAndHolidays.HasValue)
                result.NightSummerAndHolidays = preImage.new_NightSummerAndHolidays.HasValue ? preImage.new_NightSummerAndHolidays.Value : 0;
            if (target.Attributes.Contains("new_nightsummerandholidays"))
                result.NightSummerAndHolidays = target.new_NightSummerAndHolidays.HasValue ? target.new_NightSummerAndHolidays.Value : 0;

            if (parameters.Attributes.Contains("new_nightsummerandholidays"))
                result.ParamNightSummerAndHolidays = parameters.new_NightSummerAndHolidays.HasValue ? parameters.new_NightSummerAndHolidays.Value : 0;

            #endregion

            return result;
        }



        public override decimal GetTotal(TotalNightSummerAndHolidaysResponse response)
        {
            return response.TotalCostNightSummerAndHolidays;
        }
    }


}
