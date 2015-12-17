using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Contract
{
    public interface ISectionAnyIsraelStation 
    {
        bool AnyCostIsraelStation { get; set; }
    }

    public interface ISectionCallCenter
    {
        bool Anycostcallcenter { get; set; } //  new_anycostcallcenter
    }
    public interface IGroundOperation
    {
    bool Anycostgroundoperation{ get; set; } 
    }
    public interface ISundor
    {
        bool Anycostsundor { get; set; } 
    }
    public interface ICrew
    {
        bool Anycostplacingteams { get; set; }
    }

    public interface ICustomerRelations
    {
        bool AnycostCustomerRelation { get; set; }//new_anycostcustomerrelation
    }
}
