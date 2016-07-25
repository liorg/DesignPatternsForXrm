using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Xrm.Workflow.BL
{
    public class DocModel
    {
        public bool? IsTestDrive { get; set; }
        public bool? IsTardeIn { get; set; }
        public EntityReference OwnerId { get; set; }
        public EntityReference RegardingId { get; set; }
        public bool? IsLoan { get; set; }
        public bool? Delivery { get; set; }
        public string Subject { get; set; }
        public string EntityName { get; set; }
    }
}
