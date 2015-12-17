
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Lior.Xrm.Xrm;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public class TotalCostCompensationsResponse
    {
        //new_compensationcostflight
        public decimal TotalCompensationCostFlight { get; set; }
    }

    public class TotalCostCompensationsRequest : ICustomerRelations
    {
        public bool AnycostCustomerRelation { get; set; }
        public decimal TotalCompensationCostFlight { get; set; }


    }

    public class XrmCostCompensations : XrmDecoratorV2<TotalCostCompensationsResponse>
    {
        public XrmCostCompensations(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostCompensationsResponse response)
        {
            _new_incidentservice.new_CompensationCostFlight = response.TotalCompensationCostFlight;

        }
    }

    public class CostCompensations : ParticipantOnCalculator<TotalCostCompensationsRequest, TotalCostCompensationsResponse>
    {

        public CostCompensations(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostCompensationsResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }



        protected override TotalCostCompensationsResponse CalcTotal(TotalCostCompensationsRequest request)
        {
            var response = new TotalCostCompensationsResponse();
            response.TotalCompensationCostFlight = 0;

            if (!request.AnycostCustomerRelation)
                return response;

            var openClose = this.OpenClosedCalculator;
            var orginal = openClose.GetOriginalFlight();
            decimal benefitvalues = 0;
            if (orginal != null)
            {
                var query = @"<fetch mapping='logical' version='1.0'>
	                        <entity name='new_compensation'>
                                <attribute name='new_benefitvalue' />
		                        <link-entity name='incident' from='incidentid' to='new_incidentid' >
			                       <link-entity name='new_flightoccurrence' from='new_flightoccurrenceid' to='new_main_flight' >
                                          <filter type='and'>                                                         
                                                <condition attribute='new_flightoccurrenceid' operator='eq' value='" + orginal.Id.ToString() + @"' />
                                           </filter>
			                     </link-entity>
		                        </link-entity>
	                        </entity>
                        </fetch>";
                var compensations = openClose.Service.RetrieveMultiple(new FetchExpression(query));
                if (compensations.Entities.Count > 0)
                {
                    foreach (var compensation in compensations.Entities)
                    {
                        var benefitvalue = compensation.GetAttributeValue<Money>("new_benefitvalue");
                        if (benefitvalue != null)
                            benefitvalues += benefitvalue.Value;
                    }
                    request.TotalCompensationCostFlight = benefitvalues;
                    response.TotalCompensationCostFlight = benefitvalues;

                }
            }
            return response;

        }

        protected override TotalCostCompensationsRequest LoadRequest()
        {
            var result = new TotalCostCompensationsRequest();
            var preImage = _openClosedCalculator.PreImage;
            var target = _openClosedCalculator.Target;
            var parameters = _openClosedCalculator.Paramters;

            #region   AnycostCustomerRelation

            if (preImage != null && preImage.new_AnyCostCustomerRelation.HasValue)
                result.AnycostCustomerRelation = preImage.new_AnyCostCustomerRelation.HasValue ? preImage.new_AnyCostCustomerRelation.Value : false;
            if (target.Attributes.Contains("new_AnyCostCustomerRelation".ToLower()))
                result.AnycostCustomerRelation = target.new_AnyCostCustomerRelation.HasValue ? target.new_AnyCostCustomerRelation.Value : false;
            #endregion

            #region TotalCost
            if (preImage != null && preImage.new_CompensationCostFlight.HasValue)
                result.TotalCompensationCostFlight = preImage.new_CompensationCostFlight.HasValue ? preImage.new_CompensationCostFlight.Value : 0;
            if (target.Attributes.Contains("new_CompensationCostFlight".ToLower()))
                result.TotalCompensationCostFlight = target.new_CompensationCostFlight.HasValue ? target.new_CompensationCostFlight.Value : 0;

            #endregion

            return result;
        }

        public override decimal GetTotal(TotalCostCompensationsResponse response)
        {
            return response.TotalCompensationCostFlight;
        }
    }


}
