using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Plugin.EmailSend
{
   public interface IAgentSend
    {
        void Send(ITracingService log, ConfigEmail config,EmailModel emailModel);
    }
}
