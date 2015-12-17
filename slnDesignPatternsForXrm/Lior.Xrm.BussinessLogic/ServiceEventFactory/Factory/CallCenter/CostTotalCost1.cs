using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCost1Response
    {
        public decimal TotalCost1 { get; set; }
    }

    public class TotalCost1Request : ISectionCallCenter
    {
        public int NumberOfPassenger1 { get; set; }
       
        public bool Anycostcallcenter { get; set; }

        public decimal TotalCost1 { get; set; }

        public Guid? Source1 { get; set; }


    }

    public class XrmTotalCost1 : XrmDecoratorV2<TotalCost1Response>
    {
        public XrmTotalCost1(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCost1Response response)
        {
            _new_incidentservice.new_TotalCost1 = response.TotalCost1;

        }
    }

    public class CostTotalCost1 : ParticipantOnCalculator<TotalCost1Request, TotalCost1Response>
    {

        public CostTotalCost1(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCost1Response> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCost1Response CalcTotal(TotalCost1Request request)
        {
            TotalCost1Response response = new TotalCost1Response();
             var callCenterHelper = _openClosedCalculator.CallCenterHelper;
            var parameterCost=callCenterHelper.FindMatchCost(request.Source1) ;
            response.TotalCost1 = request.Anycostcallcenter ? request.NumberOfPassenger1 * parameterCost : 0;
            return response;
        }

        protected override TotalCost1Request LoadRequest()
        {
            var result = new TotalCost1Request();
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
            if (preImage != null && preImage.new_NumberOfPassenger1.HasValue)
                result.NumberOfPassenger1 = preImage.new_NumberOfPassenger1.HasValue ? preImage.new_NumberOfPassenger1.Value : 0;
            if (target.Attributes.Contains("new_NumberOfPassenger1".ToLower()))
                result.NumberOfPassenger1 = target.new_NumberOfPassenger1.HasValue ? target.new_NumberOfPassenger1.Value : 0;

            if (preImage != null && preImage.new_Source1!=null)
                result.Source1 = preImage.new_Source1 != null ? preImage.new_Source1.Id :(Guid?) null;
            if (target.Attributes.Contains("new_Source1".ToLower()))
                result.Source1 = target.new_Source1 != null ? target.new_Source1.Id : (Guid?)null;

           

            #endregion


            return result;
        }



        public override decimal GetTotal(TotalCost1Response response)
        {
            return response.TotalCost1;
        }
    }


}
