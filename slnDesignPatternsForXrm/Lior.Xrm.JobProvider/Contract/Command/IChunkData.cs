using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Xrm.JobsProvider.DataModel
{
    public interface IChunkData
    {
        DateTime Next(DateTime date);
        int Limit { get; set; }
        int Page { get; set; }
        DateTime FromDate { get; set; }
        DateTime? MaxDate { get; set; }
    }
  
    public interface IChunkData<T> : IChunkData, ICommandJobBase<T>
    where T : JobRecordBase
    {

        IEnumerable<T> Get(Action<T, string, EventLogEntryType> log, ChunkData data);
    }

   
}
