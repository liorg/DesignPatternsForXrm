using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{



    //CostNightMostOfTheYear

    public class TotalCostNightMostOfTheYearRequest : ISectionAnyIsraelStation
    {
        public decimal NightMostOfTheYear { get; set; }
        public decimal ParamNightMostOfTheYear { get; set; }
        public bool AnyCostIsraelStation { get; set; }
        public decimal TotalCostNightMostOfTheYear { get; set; }
    }

    public class TotalCostNightMostOfTheYearResponse
    {
        public decimal TotalCostNightMostOfTheYear { get; set; }
    }

    public class XrmCostNightMostOfTheYear: XrmDecoratorV2<TotalCostNightMostOfTheYearResponse>
    {
        public XrmCostNightMostOfTheYear(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostNightMostOfTheYearResponse response)
        {
            _new_incidentservice.new_TotalCostNightMostOfTheYear = response.TotalCostNightMostOfTheYear;

        }
    }

    public class CostNightMostOfTheYear : ParticipantOnCalculator<TotalCostNightMostOfTheYearRequest, TotalCostNightMostOfTheYearResponse>
    {

        public CostNightMostOfTheYear(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostNightMostOfTheYearResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCostNightMostOfTheYearResponse CalcTotal(TotalCostNightMostOfTheYearRequest request)
        {
            TotalCostNightMostOfTheYearResponse response = new TotalCostNightMostOfTheYearResponse();
            response.TotalCostNightMostOfTheYear = request.AnyCostIsraelStation ? request.ParamNightMostOfTheYear * request.NightMostOfTheYear : 0;
            return response;
        }

        protected override TotalCostNightMostOfTheYearRequest LoadRequest()
        {
            var result = new TotalCostNightMostOfTheYearRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region SectionAnyCostIsraelStation
            if (preImage != null && preImage.new_AnyCostIsraelStation.HasValue)
                result.AnyCostIsraelStation = preImage.new_AnyCostIsraelStation.HasValue ? preImage.new_AnyCostIsraelStation.Value : false;
            if (target.Attributes.Contains("new_anycostisraelstation"))
                result.AnyCostIsraelStation = target.new_AnyCostIsraelStation.HasValue ? target.new_AnyCostIsraelStation.Value : false;
            #endregion

            #region TotalcostnightmostOftheyear
            if (preImage != null && preImage.new_TotalCostNightMostOfTheYear.HasValue)
                result.TotalCostNightMostOfTheYear = preImage.new_TotalCostNightMostOfTheYear.HasValue ? preImage.new_TotalCostNightMostOfTheYear.Value : 0;
            if (target.Attributes.Contains("new_totalcostnightmostOftheyear"))
                result.TotalCostNightMostOfTheYear = target.new_TotalCostNightMostOfTheYear.HasValue ? target.new_TotalCostNightMostOfTheYear.Value : 0;

            if (preImage != null && preImage.new_NightMostOfTheYear.HasValue)
                result.NightMostOfTheYear = preImage.new_NightMostOfTheYear.HasValue ? preImage.new_NightMostOfTheYear.Value : 0;
            if (target.Attributes.Contains("new_nightmostoftheyear"))
                result.NightMostOfTheYear = target.new_NightMostOfTheYear.HasValue ? target.new_NightMostOfTheYear.Value : 0;

            if (parameters.Attributes.Contains("new_nightmostoftheyear"))
                result.ParamNightMostOfTheYear = parameters.new_NightMostOfTheYear.HasValue ? parameters.new_NightMostOfTheYear.Value : 0;
            else
                result.ParamNightMostOfTheYear = 0;
            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostNightMostOfTheYearResponse response)
        {
            return response.TotalCostNightMostOfTheYear;
        }
    }


}
