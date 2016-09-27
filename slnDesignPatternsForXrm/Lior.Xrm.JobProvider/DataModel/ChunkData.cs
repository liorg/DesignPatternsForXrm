using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Xrm.JobsProvider.DataModel
{
    public class ChunkData
    {
        public string Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Page { get; set; }
        public int Limit { get; set; }
        public Guid JobId { get; set; }
        public string FetchFilterXmlObject { get; set; }
    }
}
