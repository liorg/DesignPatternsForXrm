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
    //
    public class TotalCostNumberOfPassengerCompensatedResponse
    {
        //new_numberofpassengercompensated
        public int TotalNumberOfPassengerCompensated { get; set; }
    }

    public class TotalCostNumberOfPassengerCompensatedRequest : ICustomerRelations
    {
        public bool AnycostCustomerRelation { get; set; }
        public int TotalNumberOfPassengerCompensated { get; set; }


    }

    public class XrmCostNumberOfPassengerCompensated: XrmDecoratorV2<TotalCostNumberOfPassengerCompensatedResponse>
    {
        public XrmCostNumberOfPassengerCompensated(new_incidentservice new_incidentservice)
            : base(new_incidentservice)
        {

        }

        public override void SetTotal(TotalCostNumberOfPassengerCompensatedResponse response)
        {
            _new_incidentservice.new_NumberOfPassengerCompensated = response.TotalNumberOfPassengerCompensated;

        }
    }

    public class CostNumberOfPassengerCompensated : ParticipantOnCalculator<TotalCostNumberOfPassengerCompensatedRequest, TotalCostNumberOfPassengerCompensatedResponse>
    {

        public CostNumberOfPassengerCompensated(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TotalCostNumberOfPassengerCompensatedResponse> xrmDecorator)
            : base(openClosedCalculator, priority, xrmDecorator)
        {

        }

        protected override TotalCostNumberOfPassengerCompensatedResponse CalcTotal(TotalCostNumberOfPassengerCompensatedRequest request)
        {
            var response = new TotalCostNumberOfPassengerCompensatedResponse();
             response.TotalNumberOfPassengerCompensated = 0;
               
            if (!request.AnycostCustomerRelation)
               return response;
            
            var openClose = this.OpenClosedCalculator;
            var orginal = openClose.GetOriginalFlight();
            int numberofpassengerscompensateds= 0;
            if (orginal != null)
            {
                var query = @"<fetch mapping='logical' version='1.0'>
	                        <entity name='new_compensation'>
                                <attribute name='new_numberofpassengerscompensated' />
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
                        var numberofpassengerscompensated = compensation.GetAttributeValue<int?>("new_numberofpassengerscompensated");
                     if (numberofpassengerscompensated != null)
                         numberofpassengerscompensateds += numberofpassengerscompensated.Value;
                    }
                    request.TotalNumberOfPassengerCompensated = numberofpassengerscompensateds;
                    response.TotalNumberOfPassengerCompensated = numberofpassengerscompensateds;
                   
                }
            }
            return response;
                 
        }

        protected override TotalCostNumberOfPassengerCompensatedRequest LoadRequest()
        {
            var result = new TotalCostNumberOfPassengerCompensatedRequest();
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
            if (preImage != null && preImage.new_NumberOfPassengerCompensated.HasValue)
                result.TotalNumberOfPassengerCompensated = preImage.new_NumberOfPassengerCompensated.HasValue ? preImage.new_NumberOfPassengerCompensated.Value : 0;
            if (target.Attributes.Contains("new_NumberOfPassengerCompensated".ToLower()))
                result.TotalNumberOfPassengerCompensated = target.new_NumberOfPassengerCompensated.HasValue ? target.new_NumberOfPassengerCompensated.Value : 0;

            #endregion

            return result;
        }

        public override decimal GetTotal(TotalCostNumberOfPassengerCompensatedResponse response)
        {
            return response.TotalNumberOfPassengerCompensated;
        }
    }


}
