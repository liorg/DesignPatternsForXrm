using Microsoft.Crm.Sdk.Messages;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

using System.Activities;


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Malam.Toto.Crm.Workflow.Utils;
using System;
using printpdf.Utils;
using printpdf;
using Lior.Xrm.Workflow.BL;

namespace Lior.Xrm.Workflow
{
    public class  Print : CodeActivity
    {
        [RequiredArgument]
        [Input("Path To Save(Temp)")]
        public InArgument<string> PathToSave { get; set; }

        [RequiredArgument]
        [Input("Retry Printering")]
        public InArgument<int> MaxRetry { get; set; }

        [RequiredArgument]
        [Input("Goust Script Path")]
        public InArgument<string> GoustScriptPath { get; set; }

        [RequiredArgument]
        [Input("Printer Name")]
        public InArgument<string> PrinterName { get; set; }

        /// <summary>
        /// login on wf service user profile
        /// must install gs910w64 on server
        ///  http://www.ghostscript.com/download/gsdnld.html or sharedDlls
        /// </summary>
        /// <param name="executionContext"></param>
        protected override void Execute(CodeActivityContext executionContext)
        {
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();
            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);
          
            var entityid = (Guid)context.InputParameters["EntityId"];

            TaskDocumentsPrinter logic = new TaskDocumentsPrinter(service);
            var data = logic.GetPdfFromTask(entityid);
            if (!data.Any()) 
                return;

            var pathRoot = PathToSave.Get(executionContext);
            var goustScriptPath = GoustScriptPath.Get(executionContext);
            var printer = PrinterName.Get(executionContext);
             var maxRetry = MaxRetry.Get(executionContext);
             var path="";
            FileUtils fileUtil=new FileUtils();
            try
            {
                foreach (var fileAndData in data)
                {
                    path = fileUtil.CopyToTemp(pathRoot, fileAndData);
                    UtilPrinters.Print(new Settings
                    {
                        GoustScriptPath = goustScriptPath,
                        printerName = printer,
                        MaxRetry = maxRetry,
                        WaitingRetry=1000
                    }, path, Log);
                }
                
            }
            catch (System.Exception e)
            {
                throw new InvalidWorkflowException(e.ToString());
            }


        }

        void Log(string s)
        {
        }

    }
}
