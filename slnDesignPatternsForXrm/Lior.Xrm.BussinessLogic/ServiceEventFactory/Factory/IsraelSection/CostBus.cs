using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCostBusResponse
    {
        public decimal TotalCostBus { get; set; }
    }

    public class TotalCostBusRequest : ISectionAnyIsraelStation
    {
        public decimal Bus { get; set; }
        public decimal ParamBus { get; set; }
        public bool AnyCostIsraelStation { get; set; }

        public decimal TotalCostBus { get; set; }
    }

    public class XrmCostBus : XrmDecoratorV2<TotalCostBusResponse>
    {
        public XrmCostBus(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostBusResponse response)
        {
            _new_incidentservice.new_TotalCostBus = response.TotalCostBus;

        }
    }

    public class CostBus : ParticipantOnCalculator<TotalCostBusRequest, TotalCostBusResponse>
    {
       
        public CostBus(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostBusResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }

        

        protected override TotalCostBusResponse CalcTotal(TotalCostBusRequest request)
        {
            TotalCostBusResponse response = new TotalCostBusResponse();
            response.TotalCostBus = request.AnyCostIsraelStation ? request.ParamBus * request.Bus : 0;
            return response;
        }

        protected override TotalCostBusRequest LoadRequest()
        {
            var result = new TotalCostBusRequest();
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
            if (preImage != null && preImage.new_TotalCostBus.HasValue)
                result.TotalCostBus = preImage.new_TotalCostBus.HasValue ? preImage.new_TotalCostBus.Value : 0;
            if (target.Attributes.Contains("new_totalcostbus"))
                result.TotalCostBus = target.new_TotalCostBus.HasValue ? target.new_TotalCostBus.Value : 0;

            if (preImage != null && preImage.new_Bus.HasValue)
                result.Bus = preImage.new_Bus.HasValue ? preImage.new_Bus.Value : 0;
            if (target.Attributes.Contains("new_bus"))
                result.Bus = target.new_Bus.HasValue ? target.new_Bus.Value : 0;

            if (parameters.Attributes.Contains("new_bus"))
                result.ParamBus = parameters.new_Bus.HasValue ? parameters.new_Bus.Value : 0;

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostBusResponse response)
        {
            return response.TotalCostBus;
        }
    }


}
