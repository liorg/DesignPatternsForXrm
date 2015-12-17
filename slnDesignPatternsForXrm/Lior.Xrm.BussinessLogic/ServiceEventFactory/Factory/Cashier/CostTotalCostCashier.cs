using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    //CostTotalCostCashier
    public class TotalCostCashierResponse
    {
        public decimal TotalCashier { get; set; }
    }

    public class TotalCostCashierRequest : ISectionCallCenter
    {

        public bool Anycostcallcenter { get; set; }

        public decimal AdditionalCostscashier { get; set; }
        public decimal Totalcostcompensationsdgextratktdbcrfncas { get; set; }


    }

    public class XrmCostTotalCostCashier : XrmDecoratorV2<TotalCostCashierResponse>
    {
        public XrmCostTotalCostCashier(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostCashierResponse response)
        {
            _new_incidentservice.new_TotalCashier = response.TotalCashier;

        }
    }

    public class CostTotalCostCashier : ParticipantOnCalculator<TotalCostCashierRequest, TotalCostCashierResponse>
    {

        public CostTotalCostCashier(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostCashierResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCostCashierResponse CalcTotal(TotalCostCashierRequest request)
        {
            var response = new TotalCostCashierResponse();
            decimal sumOfcostTotalCost6 = _depencies.ContainsKey("TotalCost6") ? _depencies["TotalCost6"].GetTotal() : 0;
            decimal sumOfcostTotalCost7 = _depencies.ContainsKey("TotalCost7") ? _depencies["TotalCost7"].GetTotal() : 0;
            decimal sumOfcostTotalCost8 = _depencies.ContainsKey("TotalCost8") ? _depencies["TotalCost8"].GetTotal() : 0;
            decimal sumOfcostTotalCost9 = _depencies.ContainsKey("TotalCost9") ? _depencies["TotalCost9"].GetTotal() : 0;
            decimal sumOfcostTotalCost10 = _depencies.ContainsKey("TotalCost10") ? _depencies["TotalCost10"].GetTotal() : 0;
            if(request.Anycostcallcenter)
                    response.TotalCashier=  request.AdditionalCostscashier+request.Totalcostcompensationsdgextratktdbcrfncas+
                                        sumOfcostTotalCost6 + sumOfcostTotalCost7 + sumOfcostTotalCost8 + sumOfcostTotalCost9 + sumOfcostTotalCost10;

            
            else
                response.TotalCashier=0;
            
            return response;
        }

        protected override TotalCostCashierRequest LoadRequest()
        {
            var result = new TotalCostCashierRequest();
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
            if (preImage != null && preImage.new_AdditionalCostsCashier.HasValue)
                result.AdditionalCostscashier = preImage.new_AdditionalCostsCashier.HasValue ? preImage.new_AdditionalCostsCashier.Value : 0;
            if (target.Attributes.Contains("new_AdditionalCostsCashier".ToLower()))
                result.AdditionalCostscashier = target.new_AdditionalCostsCashier.HasValue ? target.new_AdditionalCostsCashier.Value : 0;

            if (preImage != null && preImage.new_TotalCostCompensationsDGExtratktDBCRFNcas.HasValue)
                result.Totalcostcompensationsdgextratktdbcrfncas = preImage.new_TotalCostCompensationsDGExtratktDBCRFNcas.HasValue ? preImage.new_TotalCostCompensationsDGExtratktDBCRFNcas.Value : 0;
            if (target.Attributes.Contains("new_TotalCostCompensationsDGExtratktDBCRFNcas".ToLower()))
                result.Totalcostcompensationsdgextratktdbcrfncas = target.new_TotalCostCompensationsDGExtratktDBCRFNcas.HasValue ? target.new_TotalCostCompensationsDGExtratktDBCRFNcas.Value : 0;

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCostCashierResponse response)
        {
            return response.TotalCashier;
        }
    }


}
