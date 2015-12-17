using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCost4Response
    {
        public decimal TotalCost4 { get; set; }
    }

    public class TotalCost4Request : ISectionCallCenter
    {
        public int NumberOfPassenger4 { get; set; }
       
        public bool Anycostcallcenter { get; set; }

        public decimal TotalCost4 { get; set; }

        public Guid? Source4 { get; set; }


    }

    public class XrmTotalCost4 : XrmDecoratorV2<TotalCost4Response>
    {
        public XrmTotalCost4(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCost4Response response)
        {
            _new_incidentservice.new_TotalCost4 = response.TotalCost4;

        }
    }

    public class CostTotalCost4: ParticipantOnCalculator<TotalCost4Request, TotalCost4Response>
    {

        public CostTotalCost4(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCost4Response> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCost4Response CalcTotal(TotalCost4Request request)
        {
            TotalCost4Response response = new TotalCost4Response();
             var callCenterHelper = _openClosedCalculator.CallCenterHelper;
            var parameterCost=callCenterHelper.FindMatchCost(request.Source4) ;
            response.TotalCost4 = request.Anycostcallcenter ? request.NumberOfPassenger4 * parameterCost : 0;
            return response;
        }

        protected override TotalCost4Request LoadRequest()
        {
            var result = new TotalCost4Request();
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
            if (preImage != null && preImage.new_NumberOfPassenger4.HasValue)
                result.NumberOfPassenger4 = preImage.new_NumberOfPassenger4.HasValue ? preImage.new_NumberOfPassenger4.Value : 0;
            if (target.Attributes.Contains("new_NumberOfPassenger4".ToLower()))
                result.NumberOfPassenger4 = target.new_NumberOfPassenger4.HasValue ? target.new_NumberOfPassenger4.Value : 0;

            if (preImage != null && preImage.new_Source4!=null)
                result.Source4 = preImage.new_Source4 != null ? preImage.new_Source4.Id : (Guid?)null;
            if (target.Attributes.Contains("new_Source4".ToLower()))
                result.Source4 = target.new_Source4 != null ? target.new_Source4.Id : (Guid?)null;

           

            #endregion


            return result;
        }

        public override decimal GetTotal(TotalCost4Response response)
        {
            return response.TotalCost4;
        }
    }


}
