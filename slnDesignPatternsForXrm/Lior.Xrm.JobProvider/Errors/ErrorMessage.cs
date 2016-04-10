using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Lior.Xrm.JobsProvider.Errors
{
    [Serializable]
    public class ErrorMessage
    {
        public string JobName { get; set; }
        public string Message { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public EventLogEntryType LogType { get; set; }
        public bool WriteToLogCrm { get; set; }
        public Exception Exeption { get; set; }
        public OrganizationServiceProxy CrmService { get; set; }
    }
}
