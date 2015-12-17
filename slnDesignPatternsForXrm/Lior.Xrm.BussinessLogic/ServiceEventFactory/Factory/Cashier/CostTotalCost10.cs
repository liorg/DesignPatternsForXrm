using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCost10Response
    {
        public decimal TotalCost10 { get; set; }
    }

    public class TotalCost10Request : ISectionCallCenter
    {
        public int NumberOfPassenger10 { get; set; }
       
        public bool Anycostcallcenter { get; set; }

        public decimal TotalCost10 { get; set; }

        public Guid? Source10 { get; set; }


    }

    public class XrmTotalCost10: XrmDecoratorV2<TotalCost10Response>
    {
        public XrmTotalCost10(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCost10Response response)
        {
            _new_incidentservice.new_TotalCost10 = response.TotalCost10;

        }
    }

    public class CostTotalCost10 : ParticipantOnCalculator<TotalCost10Request, TotalCost10Response>
    {

        public CostTotalCost10(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCost10Response> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCost10Response CalcTotal(TotalCost10Request request)
        {
            var response = new TotalCost10Response();
             var cashierHelper = _openClosedCalculator.CashierHelper;
             var parameterCost = cashierHelper.FindMatchCost(request.Source10);
             response.TotalCost10 = request.Anycostcallcenter ? request.NumberOfPassenger10 * parameterCost : 0;
            return response;
        }

        protected override TotalCost10Request LoadRequest()
        {
            var result = new TotalCost10Request();
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
            if (preImage != null && preImage.new_NumberOfPassenger10.HasValue)
                result.NumberOfPassenger10 = preImage.new_NumberOfPassenger10.HasValue ? preImage.new_NumberOfPassenger10.Value : 0;
            if (target.Attributes.Contains("new_NumberOfPassenger10".ToLower()))
                result.NumberOfPassenger10 = target.new_NumberOfPassenger10.HasValue ? target.new_NumberOfPassenger10.Value : 0;

            if (preImage != null && preImage.new_Source10 != null)
                result.Source10 = preImage.new_Source10 != null ? preImage.new_Source10.Id : (Guid?)null;
            if (target.Attributes.Contains("new_Source10".ToLower()))
                result.Source10 = target.new_Source10 != null ? target.new_Source10.Id : (Guid?)null;

           

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCost10Response response)
        {
            return response.TotalCost10;
        }
    }


}
