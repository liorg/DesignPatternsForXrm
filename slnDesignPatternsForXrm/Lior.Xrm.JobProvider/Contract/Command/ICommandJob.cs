using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace Lior.Xrm.JobsProvider.DataModel
{
    public interface ICommandJob<T> : ICommandJobBase<T> where T : JobRecordBase
    {
        IEnumerable<T> Get(Action<T, string, EventLogEntryType> log);
    }
}
