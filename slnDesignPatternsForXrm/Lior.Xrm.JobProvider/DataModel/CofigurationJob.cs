using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Xrm.JobsProvider.DataModel
{
    public class CofigurationJob
    {
        public string JobName { get; set; }
        public string Version { get; set; }
        public int? MaxRetries { get; set; }
        public bool IsDeleteSuccessRows { get; set; }
        public int? RowspPage { get; set; }
    }
}
