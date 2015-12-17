using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lior.Xrm.Xrm;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Decorator
{

    public abstract class XrmDecoratorV2<TResponse>
    {
        protected new_incidentservice _new_incidentservice;

        public XrmDecoratorV2(new_incidentservice new_incidentservice)
        {
            // _context = context;
            _new_incidentservice = new_incidentservice;


        }

        public abstract void SetTotal(TResponse response);

    }

 
}
