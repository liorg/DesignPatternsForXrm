using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Xrm.JobsProvider.DataModel
{
    [Flags]
    public enum ResultJob
    {
        Success = 0, Insert = 1, Update = 2, NoUpdate = 4, Failed = 8, FailedRetry = 16, PartiallyFailed = 32
    }
}
