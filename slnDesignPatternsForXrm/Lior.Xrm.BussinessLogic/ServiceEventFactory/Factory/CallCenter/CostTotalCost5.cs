using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCost5Response
    {
        public decimal TotalCost5 { get; set; }
    }

    public class TotalCost5Request : ISectionCallCenter
    {
        public int NumberOfPassenger5 { get; set; }
       
        public bool Anycostcallcenter { get; set; }

        public decimal TotalCost5 { get; set; }

        public Guid? Source5 { get; set; }


    }

    public class XrmTotalCost5 : XrmDecoratorV2<TotalCost5Response>
    {
        public XrmTotalCost5(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCost5Response response)
        {
            _new_incidentservice.new_TotalCost5 = response.TotalCost5;

        }
    }

    public class CostTotalCost5: ParticipantOnCalculator<TotalCost5Request, TotalCost5Response>
    {

        public CostTotalCost5(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCost5Response> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCost5Response CalcTotal(TotalCost5Request request)
        {
            TotalCost5Response response = new TotalCost5Response();
             var callCenterHelper = _openClosedCalculator.CallCenterHelper;
            var parameterCost=callCenterHelper.FindMatchCost(request.Source5) ;
            response.TotalCost5 = request.Anycostcallcenter ? request.NumberOfPassenger5* parameterCost : 0;
            return response;
        }

        protected override TotalCost5Request LoadRequest()
        {
            var result = new TotalCost5Request();
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
            if (preImage != null && preImage.new_NumberOfPassenger5.HasValue)
                result.NumberOfPassenger5 = preImage.new_NumberOfPassenger5.HasValue ? preImage.new_NumberOfPassenger5.Value : 0;
            if (target.Attributes.Contains("new_NumberOfPassenger5".ToLower()))
                result.NumberOfPassenger5= target.new_NumberOfPassenger5.HasValue ? target.new_NumberOfPassenger5.Value : 0;

            if (preImage != null && preImage.new_Source5!=null)
                result.Source5 = preImage.new_Source5 != null ? preImage.new_Source5.Id :(Guid?) null;
            if (target.Attributes.Contains("new_Source5".ToLower()))
                result.Source5 = target.new_Source5 != null ? target.new_Source5.Id : (Guid?)null;

           

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCost5Response response)
        {
            return response.TotalCost5;
        }
    }


}
