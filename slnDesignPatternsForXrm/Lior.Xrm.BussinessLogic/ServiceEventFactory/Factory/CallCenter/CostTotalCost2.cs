using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCost2Response
    {
        public decimal TotalCost2 { get; set; }
    }

    public class TotalCost2Request : ISectionCallCenter
    {
        public int NumberOfPassenger2 { get; set; }
       
        public bool Anycostcallcenter { get; set; }

        public decimal TotalCost2 { get; set; }

        public Guid? Source2 { get; set; }


    }

    public class XrmTotalCost2 : XrmDecoratorV2<TotalCost2Response>
    {
        public XrmTotalCost2(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCost2Response response)
        {
            _new_incidentservice.new_TotalCost2 = response.TotalCost2;

        }
    }

    public class CostTotalCost2 : ParticipantOnCalculator<TotalCost2Request, TotalCost2Response>
    {

        public CostTotalCost2(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCost2Response> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCost2Response CalcTotal(TotalCost2Request request)
        {
            TotalCost2Response response = new TotalCost2Response();
             var callCenterHelper = _openClosedCalculator.CallCenterHelper;
            var parameterCost=callCenterHelper.FindMatchCost(request.Source2) ;
            response.TotalCost2 = request.Anycostcallcenter ? request.NumberOfPassenger2 * parameterCost : 0;
            return response;
        }

        protected override TotalCost2Request LoadRequest()
        {
            var result = new TotalCost2Request();
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
            if (preImage != null && preImage.new_NumberOfPassenger2.HasValue)
                result.NumberOfPassenger2 = preImage.new_NumberOfPassenger2.HasValue ? preImage.new_NumberOfPassenger2.Value : 0;
            if (target.Attributes.Contains("new_NumberOfPassenger2".ToLower()))
                result.NumberOfPassenger2 = target.new_NumberOfPassenger2.HasValue ? target.new_NumberOfPassenger2.Value : 0;

            if (preImage != null && preImage.new_Source2!=null)
                result.Source2 = preImage.new_Source2 != null ? preImage.new_Source2.Id : (Guid?)null;
            if (target.Attributes.Contains("new_Source2".ToLower()))
                result.Source2 = target.new_Source2 != null ? target.new_Source2.Id : (Guid?)null;

           

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCost2Response response)
        {
            return response.TotalCost2;
        }
    }


}
