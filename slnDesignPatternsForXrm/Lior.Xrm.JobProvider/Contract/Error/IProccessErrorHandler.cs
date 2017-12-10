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
    public interface IProccessErrorHandler<T> where T : JobRecordBase
    {
        void StartWritingErrors(ICommandJobBase<T> commandJob);
        void WriteLog(T model, string log, EventLogEntryType logType);
        void WriteLog(string modelXml, string log, EventLogEntryType logType);
        void FinishWritingErrors(RunningJob jobDetatil);
    }
}
