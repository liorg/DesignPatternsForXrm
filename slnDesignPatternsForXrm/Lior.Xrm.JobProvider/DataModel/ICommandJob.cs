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

    public interface ICommandJob<T> where T : JobRecordBase
    {
        IJobProvider<T> JobProvider { get; set; }
        CofigurationJob CofigurationJob { get; }
        IEnumerable<T> Get(Action<T, string, EventLogEntryType> log);
        ResultJob Execute(T job, Action<T, string, EventLogEntryType> log);
        void PostExecute(IEnumerable<RecordJob<T>> FailedRecords, Action<T, string, EventLogEntryType> log);
    }


    [Flags]
    public enum ResultJob
    {
        Success = 0, Insert = 1, Update = 2, NoUpdate = 4, Failed = 8, FailedRetry = 16, PartiallyFailed = 32
    }
    public class CofigurationJob
    {
        public string JobName { get; set; }
        public string Version { get; set; }
        public int? MaxRetries { get; set; }
    }
}
