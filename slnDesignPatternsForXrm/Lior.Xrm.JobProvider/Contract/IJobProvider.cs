using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using System.Diagnostics;

namespace Lior.Xrm.JobsProvider.DataModel
{
    public interface IPostGetJob
    {
        void PostGet();
    }
    public interface IJobProvider<T> where T : JobRecordBase
    {
        IOrganizationService Service { get; }
        RunningJob RunningJob { get; }
        bool IsJobRunToday { get; }
    }

   


    
   
}
