using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Lior.Xrm.Xrm;


using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory;
using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;
using Microsoft.Xrm.Sdk.Query;

namespace Lior.Xrm.BusinessUnit.BL
{

    public class IncidentServiceEventCalc
    {
        protected IOrganizationService _service;
        new_incidentserviceparameter _paramters;

        new_incidentserviceparameter GetParamters()
        {
            if (_paramters == null)
            {
                using (var service = new XrmServiceContext(_service))
                {
                    _paramters = (from pa in service.new_incidentserviceparameterSet
                                  where pa.statecode.Value == 0
                                  select pa).FirstOrDefault();

                }
            }
            return _paramters;
        }
        bool _isOnCreate;
        public IncidentServiceEventCalc(IOrganizationService service,bool isOnCreate=false)
        {
            _service = service;
            _isOnCreate = isOnCreate;
        }

        public void CalcIncidentService(new_incidentservice target, new_incidentservice preImage)
        {
            var paramters = GetParamters();
            CalcService calcService = new CalcService(_service, target, preImage, paramters);

            if (IsHasAleady(calcService))
            {
                throw new InvalidPluginExecutionException("Service Event For This Flight Already Exist");
            }

            // Israel Section //
            var costBreakfast = new CostBreakfast(calcService, 7, new XrmCostBreakfast(target));
            var costCoffeeAndCake = new CostCoffeeAndCake(calcService, 8, new XrmCostCoffeeAndCake(target));
            var costHotMeal = new CostHotMeal(calcService, 9, new XrmCostHotMeal(target));
            var totalCostMeals = new TotalCostMeals(calcService, 10, new XrmTotalCostMeals(target));

            var costbus = new CostBus(calcService, 11, new XrmCostBus(target));
            var costtaxi = new CostTaxi(calcService, 12, new XrmCostTaxi(target));
            var costTOTALtransportation = new CostTOTALtransportation(calcService, 13, new XrmCostTOTALtransportation(target));
            var costNightMostOfTheYear = new CostNightMostOfTheYear(calcService, 14, new XrmCostNightMostOfTheYear(target));
            var costNightSummerAndHolidays = new CostNightSummerAndHolidays(calcService, 15, new XrmCostNightSummerAndHolidays(target));
            var costNightWithBreakfast = new CostNightWithBreakfast(calcService, 16, new XrmCostNightWithBreakfast(target));
            var costNightWithLunch = new CostNightWithLunch(calcService, 17, new XrmCostNightWithLunch(target));
            var costNightWithDinner = new CostNightWithDinner(calcService, 18, new XrmCostNightWithDinner(target));
            var costTOTALhotels = new CostTOTALhotels(calcService, 19, new XrmCostTOTALhotels(target)); //calc 

            var costIsraelStation = new CostIsraelStation(calcService, 20, new XrmCostIsraelStation(target));//calc

            // Call Center //
            var costTotalCost1 = new CostTotalCost1(calcService, 21, new XrmTotalCost1(target));
            var costTotalCost2 = new CostTotalCost2(calcService, 22, new XrmTotalCost2(target));
            var costTotalCost3 = new CostTotalCost3(calcService, 23, new XrmTotalCost3(target));
            var costTotalCost4 = new CostTotalCost4(calcService, 24, new XrmTotalCost4(target));
            var costTotalCost5 = new CostTotalCost5(calcService, 25, new XrmTotalCost5(target));
            var costTotalCallCenter = new CostTotalCallCenter(calcService, 26, new XrmCostTotalCallCenter(target));

            // Cashier //
            var costTotalCost6 = new CostTotalCost6(calcService, 27, new XrmTotalCost6(target));
            var costTotalCost7 = new CostTotalCost7(calcService, 28, new XrmTotalCost7(target));
            var costTotalCost8 = new CostTotalCost8(calcService, 29, new XrmTotalCost8(target));
            var costTotalCost9 = new CostTotalCost9(calcService, 30, new XrmTotalCost9(target));
            var costTotalCost10 = new CostTotalCost10(calcService, 31, new XrmTotalCost10(target));
            var costTotalCostCashier = new CostTotalCostCashier(calcService, 32, new XrmCostTotalCostCashier(target));

            //GroundOperation
            var costArrivalFLT = new CostArrivalFLT(calcService, 33, new XrmCostArrivalFLT(target));
            var costDepartureFLT = new CostDepartureFLT(calcService, 34, new XrmCostDepartureFLT(target));
            var costGroundOperations = new CostGroundOperations(calcService, 35, new XrmCostGroundOperations(target));

            //CustomerRelations
            var costCompensations = new CostCompensations(calcService, 36, new XrmCostCompensations(target));
            var costNumberOfPassengerCompensated = new CostNumberOfPassengerCompensated(calcService, 37, new XrmCostNumberOfPassengerCompensated(target));
            var totalNumberOfcasesrelatedtotheFlight = new TotalNumberOfcasesrelatedtotheFlight(calcService, 38, new XrmTotalNumberOfcasesrelatedtotheFlight(target));
            var costTotalPassengerflighttkt = new CostTotalPassengerflighttkt(calcService, 39, new XrmCostTotalPassengerflighttkt(target));

            //Crews 
            var costCrews = new CostCrews(calcService, 40, new XrmCostCrews(target));

            //Sunder
            var costSunder = new CostSunder(calcService, 41, new XrmCostSunder(target));


            //all
            var costForIncidentService = new CostForIncidentService(calcService, 100, new XrmCostForIncidentService(target));

            totalCostMeals.AddDependency("CostBreakfast", costBreakfast).AddDependency("CostCoffeeAndCake", costCoffeeAndCake).AddDependency("CostHotMeal", costHotMeal);

            costTOTALtransportation.AddDependency("TotalBus", costbus).AddDependency("TotalTaxi", costtaxi);

            costIsraelStation.AddDependency("TOTALhotels", costTOTALhotels).AddDependency("TOTALtransportation", costTOTALtransportation);

            costTOTALhotels.AddDependency("TotalCostNightMostOfTheYear", costNightMostOfTheYear)
                .AddDependency("TotalCostNightSummerAndHolidays", costNightSummerAndHolidays)
                .AddDependency("TotalCostNightWithBreakfast", costNightWithBreakfast)
                .AddDependency("TotalCostNightWithLunch", costNightWithLunch)
                .AddDependency("TotalCostNightWithDinner", costNightWithDinner);


            costTotalCallCenter.AddDependency("TotalCost1", costTotalCost1)
                .AddDependency("TotalCost2", costTotalCost2)
                .AddDependency("TotalCost3", costTotalCost3)
                .AddDependency("TotalCost4", costTotalCost4)
                .AddDependency("TotalCost5", costTotalCost5);


            costTotalCostCashier.AddDependency("TotalCost6", costTotalCost6)
               .AddDependency("TotalCost7", costTotalCost7)
               .AddDependency("TotalCost8", costTotalCost8)
               .AddDependency("TotalCost9", costTotalCost9)
               .AddDependency("TotalCost10", costTotalCost10);

            costGroundOperations.AddDependency("TotalCostArrival", costArrivalFLT).AddDependency("TotalCostDepartureCost", costDepartureFLT);

            costForIncidentService.AddDependency("costIsraelStation", costIsraelStation)
                                    .AddDependency("costTotalCallCenter", costTotalCallCenter)
                                    .AddDependency("costTotalCostCashier", costTotalCostCashier)
                                    .AddDependency("costGroundOperations", costGroundOperations)
                                    .AddDependency("costCrews", costCrews)
                                    .AddDependency("costSunder", costSunder);

            //register israel section
            
            calcService.Register(costBreakfast);
            calcService.Register(costCoffeeAndCake);
            calcService.Register(costHotMeal);
            calcService.Register(totalCostMeals);
            
            calcService.Register(costbus);
            calcService.Register(costtaxi);

            //--- dependency---
            calcService.Register(costTOTALtransportation);
            calcService.Register(costNightMostOfTheYear);
            calcService.Register(costNightSummerAndHolidays);
            calcService.Register(costNightWithBreakfast);
            calcService.Register(costNightWithLunch);
            calcService.Register(costNightWithDinner);

            //--- dependency---
            calcService.Register(costTOTALhotels);
            //--- dependency---
            calcService.Register(costIsraelStation);

            //register callcenter
            calcService.Register(costTotalCost1);
            calcService.Register(costTotalCost2);
            calcService.Register(costTotalCost3);
            calcService.Register(costTotalCost4);
            calcService.Register(costTotalCost5);
            //--- dependency---
            calcService.Register(costTotalCallCenter);

            //register Cashier
            calcService.Register(costTotalCost6);
            calcService.Register(costTotalCost7);
            calcService.Register(costTotalCost8);
            calcService.Register(costTotalCost9);
            calcService.Register(costTotalCost10);
            //--- dependency---
            calcService.Register(costTotalCostCashier);

            calcService.Register(costArrivalFLT);
            calcService.Register(costDepartureFLT);
            calcService.Register(costGroundOperations);

            calcService.Register(costCompensations);
            calcService.Register(costNumberOfPassengerCompensated);
            calcService.Register(totalNumberOfcasesrelatedtotheFlight);
            calcService.Register(costTotalPassengerflighttkt);

            calcService.Register(costCrews);
            calcService.Register(costSunder);

            calcService.Register(costForIncidentService);
            // calc all
            calcService.CalcAll();
        }

        bool IsHasAleady(CalcService calcService)
        {
            var orginal = calcService.GetOriginalFlight();
            var id = calcService.Target.Id;
            if (orginal == null)
                throw new InvalidPluginExecutionException("there is no Flight for Service Event");
            var query = "";
            if (!_isOnCreate)
            {
                query = @"<fetch mapping='logical' version='1.0'>
		                        <entity name='new_incidentservice' >
                                    <attribute name='new_incidentserviceid' />
                                      <filter type='and'>                                                         
                                                <condition attribute='new_incidentserviceid' operator='ne' value='" + id.ToString() + @"' />
                                           </filter>
			                       <link-entity name='new_flightoccurrence' from='new_flightoccurrenceid' to='new_flight' >
                                          <filter type='and'>                                                         
                                                <condition attribute='new_flightoccurrenceid' operator='eq' value='" + orginal.Id.ToString() + @"' />
                                           </filter>
			                       </link-entity>
	                        </entity>
                        </fetch>";
            }
            else
            {
                query = @"<fetch mapping='logical' version='1.0'>
		                        <entity name='new_incidentservice' >
                                    <attribute name='new_incidentserviceid' />
			                           <link-entity name='new_flightoccurrence' from='new_flightoccurrenceid' to='new_flight' >
                                          <filter type='and'>                                                         
                                                <condition attribute='new_flightoccurrenceid' operator='eq' value='" + orginal.Id.ToString() + @"' />
                                           </filter>
			                       </link-entity>
	                        </entity>
                        </fetch>";
            }
            var compensations = calcService.Service.RetrieveMultiple(new FetchExpression(query));
            if (compensations.Entities.Count > 0)
            {
                return true;
            }

            return false;
        }

        public void SetTotalPassengerflighttkt(Entity targetFligt)
        {
            if (!targetFligt.Attributes.Contains("new_TotalPassengerFlight".ToLower()))
                return;
            var totalPassengerFlight = targetFligt.GetAttributeValue<int?>("new_TotalPassengerFlight".ToLower());
            var query = @"<fetch mapping='logical' version='1.0'>
		                        <entity name='new_incidentservice' >
                                    <attribute name='new_incidentserviceid' />
                                    <attribute name='new_totalpassengerflighttkt' />
			                       <link-entity name='new_flightoccurrence' from='new_flightoccurrenceid' to='new_flight' >
                                          <filter type='and'>                                                         
                                                <condition attribute='new_flightoccurrenceid' operator='eq' value='" + targetFligt.Id.ToString() + @"' />
                                           </filter>
			                       </link-entity>
	                        </entity>
                        </fetch>";
            var incidentservices = _service.RetrieveMultiple(new FetchExpression(query));
            if (incidentservices.Entities.Count > 0)
            {
                foreach (var incidentservice in incidentservices.Entities)
                {
                    incidentservice["new_totalpassengerflighttkt"] = totalPassengerFlight;
                    _service.Update(incidentservice);
                }
            }

        }


        
    }

}

