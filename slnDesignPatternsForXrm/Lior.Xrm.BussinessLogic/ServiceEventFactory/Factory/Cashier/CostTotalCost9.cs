using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCost9Response
    {
        public decimal TotalCost9{ get; set; }
    }

    public class TotalCost9Request : ISectionCallCenter
    {
        public int NumberOfPassenger9 { get; set; }
       
        public bool Anycostcallcenter { get; set; }

        public decimal TotalCost9 { get; set; }

        public Guid? Source9{ get; set; }


    }

    public class XrmTotalCost9: XrmDecoratorV2<TotalCost9Response>
    {
        public XrmTotalCost9(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCost9Response response)
        {
            _new_incidentservice.new_TotalCost9= response.TotalCost9;

        }
    }

    public class CostTotalCost9: ParticipantOnCalculator<TotalCost9Request, TotalCost9Response>
    {

        public CostTotalCost9(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCost9Response> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCost9Response CalcTotal(TotalCost9Request request)
        {
            var response = new TotalCost9Response();
             var cashierHelper = _openClosedCalculator.CashierHelper;
             var parameterCost = cashierHelper.FindMatchCost(request.Source9);
            response.TotalCost9= request.Anycostcallcenter ? request.NumberOfPassenger9 * parameterCost : 0;
            return response;
        }

        protected override TotalCost9Request LoadRequest()
        {
            var result = new TotalCost9Request();
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
            if (preImage != null && preImage.new_NumberOfPassenger9.HasValue)
                result.NumberOfPassenger9 = preImage.new_NumberOfPassenger9.HasValue ? preImage.new_NumberOfPassenger9.Value : 0;
            if (target.Attributes.Contains("new_NumberOfPassenger9".ToLower()))
                result.NumberOfPassenger9 = target.new_NumberOfPassenger9.HasValue ? target.new_NumberOfPassenger9.Value : 0;

            if (preImage != null && preImage.new_Source9!=null)
                result.Source9 = preImage.new_Source9 != null ? preImage.new_Source9.Id :(Guid?) null;
            if (target.Attributes.Contains("new_Source9".ToLower()))
                result.Source9= target.new_Source9 != null ? target.new_Source9.Id : (Guid?)null;

           

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCost9Response response)
        {
            return response.TotalCost9;
        }
    }


}
