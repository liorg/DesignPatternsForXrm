using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCost6Response
    {
        public decimal TotalCost6 { get; set; }
    }

    public class TotalCost6Request : ISectionCallCenter
    {
        public int NumberOfPassenger6 { get; set; }
       
        public bool Anycostcallcenter { get; set; }

        public decimal TotalCost6 { get; set; }

        public Guid? Source6{ get; set; }


    }

    public class XrmTotalCost6: XrmDecoratorV2<TotalCost6Response>
    {
        public XrmTotalCost6(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCost6Response response)
        {
            _new_incidentservice.new_TotalCost6 = response.TotalCost6;

        }
    }

    public class CostTotalCost6 : ParticipantOnCalculator<TotalCost6Request, TotalCost6Response>
    {

        public CostTotalCost6(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCost6Response> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCost6Response CalcTotal(TotalCost6Request request)
        {
            var response = new TotalCost6Response();
             var cashierHelper = _openClosedCalculator.CashierHelper;
             var parameterCost = cashierHelper.FindMatchCost(request.Source6);
            response.TotalCost6 = request.Anycostcallcenter ? request.NumberOfPassenger6 * parameterCost : 0;
            return response;
        }

        protected override TotalCost6Request LoadRequest()
        {
            var result = new TotalCost6Request();
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
            if (preImage != null && preImage.new_NumberOfPassenger6.HasValue)
                result.NumberOfPassenger6 = preImage.new_NumberOfPassenger6.HasValue ? preImage.new_NumberOfPassenger6.Value : 0;
            if (target.Attributes.Contains("new_NumberOfPassenger6".ToLower()))
                result.NumberOfPassenger6 = target.new_NumberOfPassenger6.HasValue ? target.new_NumberOfPassenger6.Value : 0;

            if (preImage != null && preImage.new_Source6!=null)
                result.Source6 = preImage.new_Source6 != null ? preImage.new_Source6.Id :(Guid?) null;
            if (target.Attributes.Contains("new_Source6".ToLower()))
                result.Source6 = target.new_Source6 != null ? target.new_Source6.Id : (Guid?)null;

           

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCost6Response response)
        {
            return response.TotalCost6;
        }
    }


}
