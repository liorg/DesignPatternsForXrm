using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Xrm.Workflow.BL
{
    public class TaskDocumentsPrinter
    {
        IOrganizationService _service;
        public TaskDocumentsPrinter( IOrganizationService service)
        {
            _service = service;
        }
        public List<Tuple<string, string>> GetPdfFromTask(Guid taskid)
        {
            var fecthXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='annotation'><attribute name='documentbody' /><attribute name='filename' />
                            <attribute name='annotationid' />
                                     <filter type='and'>  
                                    <condition attribute='isdocument' operator='eq' value='1' />
                                    <condition attribute='filename' operator='like' value='%.pdf' />
                                    </filter>
                    <link-entity name='task' alias='ab' to='objectid' from='activityid'>
                    <filter type='and'><condition attribute='activityid' operator='eq'  value='" + taskid.ToString() + "' /></filter></link-entity></entity></fetch>";
            List<Tuple<string, string>> pdfs = new List<Tuple<string, string>>();



            EntityCollection result = _service.RetrieveMultiple(new FetchExpression(fecthXml));
            foreach (var c in result.Entities)
            {

                if (!c.Attributes.Contains("filename"))
                    continue;
                var filename = c.Attributes["filename"].ToString();
                if (!String.IsNullOrEmpty(filename) && filename.ToLower().Contains(".pdf"))
                {
                    var documentbody = c.Attributes["documentbody"].ToString();
                    pdfs.Add(new Tuple<string, string>(filename, documentbody));
                }
            }
            return pdfs;
        }


        public Tuple<string, string> GetPdfLastCreatedOnGuaranteeBank(Guid taskid)
        {
            var fecthXml = @"<fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false'>
                            <entity name='annotation'>
                                <attribute name='documentbody' />
                                <attribute name='filename' />
                                <attribute name='annotationid' />
                                <attribute name='documentbody' /> 
                                 <order attribute='createdon' descending='true'/>
                                 <filter type='and'>
                                      <condition attribute='isdocument' operator='eq' value='1' />
                                    </filter>
                                 <link-entity name='tot_bank_guarantee' from='tot_bank_guaranteeid' to='objectid' alias='ai'>
                                    <link-entity name='task' from='regardingobjectid' to='tot_bank_guaranteeid' alias='aj'>
                                        <filter type='and'>
                                             <condition attribute='activityid' operator='eq'  value='" + taskid.ToString() + "' /></filter></link-entity></link-entity></entity></fetch>";


            EntityCollection result = _service.RetrieveMultiple(new FetchExpression(fecthXml));
            if (result != null && result.Entities != null && result.Entities.Any())
            {
                foreach (var c in result.Entities)
                {

                    if (!c.Attributes.Contains("filename"))
                        continue;
                    var filename = c.Attributes["filename"].ToString();
                    if (!String.IsNullOrEmpty(filename) &&
                        ((filename.ToLower().Contains(".pdf")) ||
                        (filename.ToLower().Contains(".gif")) ||
                        (filename.ToLower().Contains(".jpg")) ||
                        (filename.ToLower().Contains(".png")) ||
                        (filename.ToLower().Contains(".tif")))
                        )
                    {
                        var documentbody = c.Attributes["documentbody"].ToString();
                        return new Tuple<string, string>(filename, documentbody);
                    }
                }

            }


            return new Tuple<string, string>(String.Empty, String.Empty);
        }
    }
}
