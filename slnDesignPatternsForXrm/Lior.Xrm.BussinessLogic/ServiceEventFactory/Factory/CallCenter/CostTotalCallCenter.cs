using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    //CostTotalCallCenter
    public class TotalCostCallCenterResponse
    {
        public decimal TotalCallCenter { get; set; }
    }

    public class TotalCostCallCenterRequest : ISectionCallCenter
    {

        public bool Anycostcallcenter { get; set; }

        public decimal AdditionalCostsTravlersTransfer { get; set; }
        public decimal TotalCostCompensationsdgextratktdbcrfn { get; set; }


    }

    public class XrmCostTotalCallCenter : XrmDecoratorV2<TotalCostCallCenterResponse>
    {
        public XrmCostTotalCallCenter(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostCallCenterResponse response)
        {
            _new_incidentservice.new_TotalCallCenter = response.TotalCallCenter;

        }
    }

    public class CostTotalCallCenter : ParticipantOnCalculator<TotalCostCallCenterRequest, TotalCostCallCenterResponse>
    {

        public CostTotalCallCenter(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostCallCenterResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCostCallCenterResponse CalcTotal(TotalCostCallCenterRequest request)
        {
            TotalCostCallCenterResponse response = new TotalCostCallCenterResponse();
            decimal sumOfcostTotalCost1 = _depencies.ContainsKey("TotalCost1") ? _depencies["TotalCost1"].GetTotal() : 0;
            decimal sumOfcostTotalCost2 = _depencies.ContainsKey("TotalCost2") ? _depencies["TotalCost2"].GetTotal() : 0;
            decimal sumOfcostTotalCost3 = _depencies.ContainsKey("TotalCost3") ? _depencies["TotalCost3"].GetTotal() : 0;
            decimal sumOfcostTotalCost4 = _depencies.ContainsKey("TotalCost4") ? _depencies["TotalCost4"].GetTotal() : 0;
            decimal sumOfcostTotalCost5 = _depencies.ContainsKey("TotalCost5") ? _depencies["TotalCost5"].GetTotal() : 0;
            if(request.Anycostcallcenter)
                    response.TotalCallCenter=  request.AdditionalCostsTravlersTransfer+request.TotalCostCompensationsdgextratktdbcrfn+
                                        sumOfcostTotalCost1 + sumOfcostTotalCost2 + sumOfcostTotalCost3 + sumOfcostTotalCost4 + sumOfcostTotalCost5;

            
            else
                response.TotalCallCenter=0;
            
            return response;
        }

        protected override TotalCostCallCenterRequest LoadRequest()
        {
            var result = new TotalCostCallCenterRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region ISectionCallCenter
            if (preImage != null && preImage.new_AnyCostCallCenter.HasValue)
                result.Anycostcallcenter = preImage.new_AnyCostCallCenter.HasValue ? preImage.new_AnyCostCallCenter.Value : false;
            if (target.Attributes.Contains("new_anycostcallcenter"))
                result.Anycostcallcenter = target.new_AnyCostCallCenter.HasValue ? target.new_AnyCostCallCenter.Value : false;
            #endregion

            #region TotalCos
            if (preImage != null && preImage.new_AdditionalCostsTravlersTransfer.HasValue)
                result.AdditionalCostsTravlersTransfer = preImage.new_AdditionalCostsTravlersTransfer.HasValue ? preImage.new_AdditionalCostsTravlersTransfer.Value : 0;
            if (target.Attributes.Contains("new_AdditionalCostsTravlersTransfer".ToLower()))
                result.AdditionalCostsTravlersTransfer = target.new_AdditionalCostsTravlersTransfer.HasValue ? target.new_AdditionalCostsTravlersTransfer.Value : 0;

            if (preImage != null && preImage.new_TotalCostCompensationsDGExtratktDBCRFN.HasValue)
                result.TotalCostCompensationsdgextratktdbcrfn = preImage.new_TotalCostCompensationsDGExtratktDBCRFN.HasValue ? preImage.new_TotalCostCompensationsDGExtratktDBCRFN.Value : 0;
            if (target.Attributes.Contains("new_TotalCostCompensationsDGExtratktDBCRFN".ToLower()))
                result.TotalCostCompensationsdgextratktdbcrfn = target.new_TotalCostCompensationsDGExtratktDBCRFN.HasValue ? target.new_TotalCostCompensationsDGExtratktDBCRFN.Value : 0;

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostCallCenterResponse response)
        {
            return response.TotalCallCenter;
        }
    }


}
