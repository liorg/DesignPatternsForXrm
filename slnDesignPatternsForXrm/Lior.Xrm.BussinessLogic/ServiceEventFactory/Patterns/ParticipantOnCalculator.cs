using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator;


namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory
{
    public abstract class ParticipantOnCalculator
    {

        int _priority;

        protected Dictionary<string, ParticipantOnCalculator> _depencies;

        public int Priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
            }
        }

        public virtual ParticipantOnCalculator AddDependency(string key, ParticipantOnCalculator d)
        {
            _depencies.Add(key, d);
            return this;
        }

        public abstract decimal GetTotal();

        protected OpenClosedCalculator _openClosedCalculator;

        // Constructor
        public ParticipantOnCalculator(OpenClosedCalculator openClosedCalculator, int priority)
        {
            _openClosedCalculator = openClosedCalculator;
            _priority = priority;
            _depencies = new Dictionary<string, ParticipantOnCalculator>();
        }


        public OpenClosedCalculator OpenClosedCalculator
        {
            set { _openClosedCalculator = value; }
            get { return _openClosedCalculator; }
        }

        public abstract void SetXrmCalc();

    }

    public abstract class ParticipantOnCalculator<TRequest, TResponse> : ParticipantOnCalculator
    {

        protected XrmDecoratorV2<TResponse> _xrmDecorator;
        protected TRequest _request;
        protected TResponse _response;
        public ParticipantOnCalculator(OpenClosedCalculator openClosedCalculator, int priority, XrmDecoratorV2<TResponse> xrmDecorator)
            : base(openClosedCalculator, priority)
        {
            _xrmDecorator = xrmDecorator;
            _request=LoadRequest();

        }
        protected abstract TResponse CalcTotal(TRequest request);

        protected abstract TRequest LoadRequest();

        public override void SetXrmCalc()
        {
            _response = CalcTotal(_request);
            _xrmDecorator.SetTotal(_response);;
        }

        public override decimal GetTotal()
        {
            return GetTotal(_response);
        }
        public abstract decimal GetTotal(TResponse response);
        

    }


}
