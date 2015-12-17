using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory;
using Lior.Xrm.BussinessLogic.DAL;
using Lior.Xrm.Xrm;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Xrm.Console.APP
{
    public class AppSendEmails
    {
        OrganizationServiceProxy _service;
        IEmailTemplate _wfemailTemp;
     
        Guid _userid;
        public AppSendEmails(OrganizationServiceProxy service)
        {
            _service = service;
            _wfemailTemp = new ConsoleEmailTemplate(_service);
            WhoAmIRequest requst = new WhoAmIRequest();
            WhoAmIResponse res = (WhoAmIResponse)_service.Execute(requst);
            _userid = res.UserId;
        }
        public void Get()
        {
            ServiceEventDAL dal = new ServiceEventDAL(_service);
            var data = dal.GetAll();
           
            int errors = 0;
            int success = 0;
            foreach (var new_incidentservice in data)
            {
                try
                {
                    Excute(new_incidentservice);
                    success++;
                }
                catch (Exception e)
                {
                   
                    errors++;

                }
            }
        
          
            // _log.Update();

        }
        public void Excute(new_incidentservice new_incidentservice)
        {

            SubjectEmailSender email = new SubjectEmailSender(_service, new_incidentservice);
            HelperEvent helper = new HelperEvent(_service, new_incidentservice);
            var parameters = helper.GetParamters();
            var originalFlight = helper.GetOriginalFlight();

            var israelWF = new IsraelWF(_service, new_incidentservice, parameters, originalFlight, _wfemailTemp);
            var callCenterWF = new CallCenterWF(_service, new_incidentservice, parameters, originalFlight, _wfemailTemp);
            var cashierWF = new CashierWF(_service, new_incidentservice, parameters, originalFlight, _wfemailTemp);
            var groupWF = new GroupWF(_service, new_incidentservice, parameters, originalFlight, _wfemailTemp);
            var foodWF = new FoodWF(_service, new_incidentservice, parameters, originalFlight, _wfemailTemp);
            var customerRelationWF = new CustomerRelationWF(_service, new_incidentservice, parameters, originalFlight, _wfemailTemp);
            var scheduleWF = new ScheduleWF(_service, new_incidentservice, parameters, originalFlight, _wfemailTemp);
            var cargoWF = new CargoWF(_service, new_incidentservice, parameters, originalFlight, _wfemailTemp);
            var crewsWF = new CrewsWF(_service, new_incidentservice, parameters, originalFlight, _wfemailTemp);
            var sundorWF = new SundorWF(_service, new_incidentservice, parameters, originalFlight, _wfemailTemp);

            email.Attach(israelWF);
            email.Attach(callCenterWF);
            email.Attach(cashierWF);
            email.Attach(groupWF);
            email.Attach(foodWF);
            email.Attach(customerRelationWF);
            email.Attach(scheduleWF);
            email.Attach(cargoWF);
            email.Attach(crewsWF);
            email.Attach(sundorWF);

            email.SendEmailsToTeams(_userid);


        }
    }
}
