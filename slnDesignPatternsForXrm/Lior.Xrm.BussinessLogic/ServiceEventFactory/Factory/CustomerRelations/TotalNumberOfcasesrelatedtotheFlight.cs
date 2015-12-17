
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
    public class TotalNumberOfcasesrelatedtotheFlightResponse
    {
        //new_numberofcasesrelatedtotheflight_c
        public int TotalNumberOfcasesrelatedtotheFlight { get; set; }
    }

    public class TotalNumberOfcasesrelatedtotheFlightRequest : ICustomerRelations
    {
        public bool AnycostCustomerRelation { get; set; }
        public int TotalNumberOfcasesrelatedtotheFlight { get; set; }


    }

    public class XrmTotalNumberOfcasesrelatedtotheFlight : XrmDecoratorV2<TotalNumberOfcasesrelatedtotheFlightResponse>
    {
        public XrmTotalNumberOfcasesrelatedtotheFlight(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalNumberOfcasesrelatedtotheFlightResponse response)
        {
            _new_incidentservice.new_NumberOfCasesRelatedToTheFlight = response.TotalNumberOfcasesrelatedtotheFlight;

        }
    }

    public class TotalNumberOfcasesrelatedtotheFlight : ParticipantOnCalculator<TotalNumberOfcasesrelatedtotheFlightRequest, TotalNumberOfcasesrelatedtotheFlightResponse>
    {

        public TotalNumberOfcasesrelatedtotheFlight(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalNumberOfcasesrelatedtotheFlightResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }

        protected override TotalNumberOfcasesrelatedtotheFlightResponse CalcTotal(TotalNumberOfcasesrelatedtotheFlightRequest request)
        {
            var response = new TotalNumberOfcasesrelatedtotheFlightResponse();
            response.TotalNumberOfcasesrelatedtotheFlight = 0;

            if (!request.AnycostCustomerRelation)
                return response;

            var openClose = this.OpenClosedCalculator;
            var orginal = openClose.GetOriginalFlight();
            if (orginal != null)
            {
                var query = @"<fetch mapping='logical' version='1.0'>
		                        <entity name='incident' >
                                    <attribute name='incidentid' />
			                         <link-entity name='new_flightoccurrence' from='new_flightoccurrenceid' to='new_main_flight' >
                                          <filter type='and'>                                                         
                                                <condition attribute='new_flightoccurrenceid' operator='eq' value='" + orginal.Id.ToString() + @"' />
                                           </filter>
			                       </link-entity>
	                        </entity>
                        </fetch>";
                var compensations = openClose.Service.RetrieveMultiple(new FetchExpression(query));
                response.TotalNumberOfcasesrelatedtotheFlight = compensations.Entities.Count;

            }
            return response;

        }

        protected override TotalNumberOfcasesrelatedtotheFlightRequest LoadRequest()
        {
            var result = new TotalNumberOfcasesrelatedtotheFlightRequest();
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
            if (preImage != null && preImage.new_NumberOfCasesRelatedToTheFlight.HasValue)
                result.TotalNumberOfcasesrelatedtotheFlight = preImage.new_NumberOfCasesRelatedToTheFlight.HasValue ? preImage.new_NumberOfCasesRelatedToTheFlight.Value : 0;
            if (target.Attributes.Contains("new_NumberOfCasesRelatedToTheFlight".ToLower()))
                result.TotalNumberOfcasesrelatedtotheFlight = target.new_NumberOfCasesRelatedToTheFlight.HasValue ? target.new_NumberOfCasesRelatedToTheFlight.Value : 0;

            #endregion

            return result;
        }

        public override decimal GetTotal(TotalNumberOfcasesrelatedtotheFlightResponse response)
        {
            return response.TotalNumberOfcasesrelatedtotheFlight;
        }
    }


}
