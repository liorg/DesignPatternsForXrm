using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.Xrm;
using Microsoft.Xrm.Sdk;
using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Factory;
using Microsoft.Xrm.Sdk.Query;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{

    public abstract class OpenClosedCalculator
    {
        protected new_incidentservice _target, _preImage;
        protected IOrganizationService _service;
        protected CallCenterHelper _callCenterHelper;
        protected CashierHelper _cashierHelper;
        protected new_incidentserviceparameter _paramters = null;

        protected new_flightoccurrence _orginalFlightOccurrence = null;

        public abstract void Register(ParticipantOnCalculator calc);

        protected List<ParticipantOnCalculator> _calcs;

        public CallCenterHelper CallCenterHelper
        {
            get { return _callCenterHelper; }
        }

        public CashierHelper CashierHelper
        {
            get { return _cashierHelper; }
        }

        public new_incidentservice PreImage
        {
            get { return _preImage; }
        }

        public new_incidentservice Target
        {
            get { return _target; }

        }

        public new_incidentserviceparameter Paramters
        {
            get { return _paramters; }
        }

        public IOrganizationService Service
        {
            get { return _service; }
        }

        public OpenClosedCalculator(IOrganizationService service, new_incidentservice target, new_incidentservice preImage, new_incidentserviceparameter paramters)
        {
            _calcs = new List<ParticipantOnCalculator>();
            _service = service;
            _target = target;
            _preImage = preImage;
            _paramters = paramters;
            _callCenterHelper = new CallCenterHelper(_paramters);
            _cashierHelper = new CashierHelper(_paramters);
        }

        public virtual new_flightoccurrence GetOriginalFlight()
        {

            if (_orginalFlightOccurrence == null)
            {
                Guid? flight = null;
                if (_preImage != null && _preImage.new_Flight != null)
                    flight = _preImage.new_Flight.Id;
                if (_target.Attributes.Contains("new_flight"))
                {
                    if (_target.new_Flight != null)
                        flight = _target.new_Flight.Id;
                    else
                        flight = null;
                }
                if (!flight.HasValue) 
                    throw new ArgumentException("There is no original Flight");
                Entity temp = _service.Retrieve("new_flightoccurrence", flight.Value, new ColumnSet(true));
               _orginalFlightOccurrence = temp.ToEntity < new_flightoccurrence>();
            }
          

            return _orginalFlightOccurrence;
        }
        public virtual void CalcAll()
        {
            foreach (var calc in _calcs.OrderBy(p => p.Priority))
            {
                calc.SetXrmCalc();
            }
        }
    }

}
