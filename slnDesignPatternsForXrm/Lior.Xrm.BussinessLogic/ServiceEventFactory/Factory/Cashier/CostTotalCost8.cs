using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCost8Response
    {
        public decimal TotalCost8{ get; set; }
    }

    public class TotalCost8Request : ISectionCallCenter
    {
        public int NumberOfPassenger8 { get; set; }
       
        public bool Anycostcallcenter { get; set; }

        public decimal TotalCost8 { get; set; }

        public Guid? Source8{ get; set; }


    }

    public class XrmTotalCost8: XrmDecoratorV2<TotalCost8Response>
    {
        public XrmTotalCost8(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCost8Response response)
        {
            _new_incidentservice.new_TotalCost8= response.TotalCost8;

        }
    }

    public class CostTotalCost8 : ParticipantOnCalculator<TotalCost8Request, TotalCost8Response>
    {

        public CostTotalCost8(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCost8Response> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCost8Response CalcTotal(TotalCost8Request request)
        {
            var response = new TotalCost8Response();
             var cashierHelper = _openClosedCalculator.CashierHelper;
             var parameterCost = cashierHelper.FindMatchCost(request.Source8);
            response.TotalCost8= request.Anycostcallcenter ? request.NumberOfPassenger8 * parameterCost : 0;
            return response;
        }

        protected override TotalCost8Request LoadRequest()
        {
            var result = new TotalCost8Request();
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
            if (preImage != null && preImage.new_NumberOfPassenger8.HasValue)
                result.NumberOfPassenger8 = preImage.new_NumberOfPassenger8.HasValue ? preImage.new_NumberOfPassenger8.Value : 0;
            if (target.Attributes.Contains("new_NumberOfPassenger8".ToLower()))
                result.NumberOfPassenger8 = target.new_NumberOfPassenger8.HasValue ? target.new_NumberOfPassenger8.Value : 0;

            if (preImage != null && preImage.new_Source8!=null)
                result.Source8 = preImage.new_Source8 != null ? preImage.new_Source8.Id :(Guid?) null;
            if (target.Attributes.Contains("new_Source8".ToLower()))
                result.Source8= target.new_Source8 != null ? target.new_Source8.Id : (Guid?)null;

           

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCost8Response response)
        {
            return response.TotalCost8;
        }
    }


}
