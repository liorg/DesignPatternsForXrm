using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lior.Xrm.JobsProvider.Errors;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using System.Diagnostics;

namespace Lior.Xrm.JobsProvider.DataModel
{
    public interface IProccessCrmLogHandler
    {
        void Init();
        void WriteStatistics(RunningJob jobstatus);
        void WriteValidationBussinessLogic(string log, string stackOverflow, string xmlObject);
        void WriteErrorRecord(string xmlObject, Exception e);
        void WriteError(Exception e);
        void Finish(RunningJob jobDetatil);
        bool EnsureCanWriteError();
        Guid? GetLogID();
    }
}
