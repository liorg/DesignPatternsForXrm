using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Xrm.JobsProvider.DataModel
{
    public interface IFetchCrmData<T> : ICommandJobBase<T>
   where T : JobRecordBase
    {
        string FetchXml { get; set; }
        int Count { get; set; }
        T ConvertToObject(EntityReference itemFetchXml, out bool toInsert);
    }
}
