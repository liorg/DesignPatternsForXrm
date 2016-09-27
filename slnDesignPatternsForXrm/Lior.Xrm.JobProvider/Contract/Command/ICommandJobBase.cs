using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Lior.Xrm.JobsProvider.DataModel
{
    public interface ICommandJobBase<T> where T : JobRecordBase
    {
        IJobProvider<T> JobProvider { get; set; }
        CofigurationJob CofigurationJob { get; }
        ResultJob Execute(T job, Action<T, string, EventLogEntryType> log);
        void PostExecute(IEnumerable<RecordJob<T>> FailedRecords, Action<T, string, EventLogEntryType> log);
    }
}
