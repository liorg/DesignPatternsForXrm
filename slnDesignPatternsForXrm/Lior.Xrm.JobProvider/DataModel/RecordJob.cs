using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lior.Xrm.JobsProvider.DataModel
{
    public class RecordJob<T> where T : JobRecordBase
    {
        public T JobRecord { get; set; }
        public Guid RecordId { get; set; }
        public int Retry { get; set; }
        public Guid JobId { get; set; }
    }
}
