using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCost7Response
    {
        public decimal TotalCost7{ get; set; }
    }

    public class TotalCost7Request : ISectionCallCenter
    {
        public int NumberOfPassenger7 { get; set; }
       
        public bool Anycostcallcenter { get; set; }

        public decimal TotalCost7 { get; set; }

        public Guid? Source7{ get; set; }


    }

    public class XrmTotalCost7: XrmDecoratorV2<TotalCost7Response>
    {
        public XrmTotalCost7(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCost7Response response)
        {
            _new_incidentservice.new_TotalCost7 = response.TotalCost7;

        }
    }

    public class CostTotalCost7 : ParticipantOnCalculator<TotalCost7Request, TotalCost7Response>
    {

        public CostTotalCost7(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCost7Response> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCost7Response CalcTotal(TotalCost7Request request)
        {
            var response = new TotalCost7Response();
             var cashierHelper = _openClosedCalculator.CashierHelper;
             var parameterCost = cashierHelper.FindMatchCost(request.Source7);
            response.TotalCost7 = request.Anycostcallcenter ? request.NumberOfPassenger7 * parameterCost : 0;
            return response;
        }

        protected override TotalCost7Request LoadRequest()
        {
            var result = new TotalCost7Request();
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
            if (preImage != null && preImage.new_NumberOfPassenger7.HasValue)
                result.NumberOfPassenger7 = preImage.new_NumberOfPassenger7.HasValue ? preImage.new_NumberOfPassenger7.Value : 0;
            if (target.Attributes.Contains("new_NumberOfPassenger7".ToLower()))
                result.NumberOfPassenger7 = target.new_NumberOfPassenger7.HasValue ? target.new_NumberOfPassenger7.Value : 0;

            if (preImage != null && preImage.new_Source7!=null)
                result.Source7 = preImage.new_Source7 != null ? preImage.new_Source7.Id :(Guid?) null;
            if (target.Attributes.Contains("new_Source7".ToLower()))
                result.Source7 = target.new_Source7 != null ? target.new_Source7.Id : (Guid?)null;

           

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCost7Response response)
        {
            return response.TotalCost7;
        }
    }


}
