using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Xrm.JobsProvider.DataModel
{
    public interface IFetchFilterXmlObjects<T> : IFetchFilterXmlObjects, IChunkData<T>
   where T : JobRecordBase
    {

    }
    public interface IFetchFilterXmlObjects : IChunkData
    {
        Queue<string> FetchFilterXmlObjects { get; }
    }
}
