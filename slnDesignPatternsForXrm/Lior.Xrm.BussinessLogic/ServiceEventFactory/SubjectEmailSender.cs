using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lior.Xrm.Xrm;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Crm.Sdk.Messages;

using System.Web;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{

    public class SubjectEmailSender
    {
        List<Observer> _observers;
        IOrganizationService _service;
        new_incidentservice _target;

        public SubjectEmailSender(IOrganizationService service, new_incidentservice target)
        {
            _observers = new List<Observer>();
            _service = service;
            _target = target;
        }

        public void Attach(Observer observer)
        {
            if (observer.CanSendEmail())
                _observers.Add(observer);
        }

        public void SendEmailsToTeams(Guid FromWho)
        {
            List<Email> emails = CreateEmails(FromWho);
            foreach (var email in emails)
            {
                SendEmail(email.ActivityId);
            }
        }
     
        List<Email> CreateEmails(Guid FromWho)
        {
            List<Email> emails = new List<Email>();
            foreach (Observer o in _observers)
            {
                var email = o.SendEmail(FromWho);
                emails.Add(email);
            }
            return emails;
        }

        void SendEmail(Guid? emailId)
        {
            if (emailId == null) throw new ArgumentNullException("no email created");
            SendEmailRequest request = new SendEmailRequest
            {
                EmailId = emailId.Value,
                TrackingToken = string.Empty,
                IssueSend = true
            };
            _service.Execute(request);
        }
    }
}
