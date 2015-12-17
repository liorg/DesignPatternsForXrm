using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCost3Response
    {
        public decimal TotalCost3 { get; set; }
    }

    public class TotalCost3Request : ISectionCallCenter
    {
        public int NumberOfPassenger3 { get; set; }
       
        public bool Anycostcallcenter { get; set; }

        public decimal TotalCost3 { get; set; }

        public Guid? Source3 { get; set; }


    }

    public class XrmTotalCost3 : XrmDecoratorV2<TotalCost3Response>
    {
        public XrmTotalCost3(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCost3Response response)
        {
            _new_incidentservice.new_TotalCost3 = response.TotalCost3;

        }
    }

    public class CostTotalCost3 : ParticipantOnCalculator<TotalCost3Request, TotalCost3Response>
    {

        public CostTotalCost3(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCost3Response> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCost3Response CalcTotal(TotalCost3Request request)
        {
            TotalCost3Response response = new TotalCost3Response();
             var callCenterHelper = _openClosedCalculator.CallCenterHelper;
            var parameterCost=callCenterHelper.FindMatchCost(request.Source3) ;
            response.TotalCost3 = request.Anycostcallcenter ? request.NumberOfPassenger3 * parameterCost : 0;
            return response;
        }

        protected override TotalCost3Request LoadRequest()
        {
            var result = new TotalCost3Request();
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
            if (preImage != null && preImage.new_NumberOfPassenger3.HasValue)
                result.NumberOfPassenger3 = preImage.new_NumberOfPassenger3.HasValue ? preImage.new_NumberOfPassenger3.Value : 0;
            if (target.Attributes.Contains("new_NumberOfPassenger3".ToLower()))
                result.NumberOfPassenger3 = target.new_NumberOfPassenger3.HasValue ? target.new_NumberOfPassenger3.Value : 0;

            if (preImage != null && preImage.new_Source3!=null)
                result.Source3 = preImage.new_Source3 != null ? preImage.new_Source3.Id : (Guid?)null;
            if (target.Attributes.Contains("new_Source3".ToLower()))
                result.Source3 = target.new_Source3 != null ? target.new_Source3.Id : (Guid?)null;

           

            #endregion


            return result;
        }

        public override decimal GetTotal(TotalCost3Response response)
        {
            return response.TotalCost3;
        }
    }


}
