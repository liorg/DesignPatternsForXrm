
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Xrm.Workflow.BL
{
    public class DocLogic
    {
        IOrganizationService _service;
        public DocLogic(IOrganizationService service)
        {
            _service = service;
        }

        public void Excute(LoadDocParametersFactory loadDoc)
        {
            if (loadDoc == null)
                return;
            var doc = loadDoc.GetParameters();
            BuilderFetch buildXML = new BuilderFetch();
            var fetchxml = buildXML.AddConditionLoad(doc)
                            .AddConditionDelivery(doc)
                                    .AddConditionTradeIn(doc)
                                            .AddConditionTestDrive(doc)
                                                                .Builder(doc);

            EntityCollection result = _service.RetrieveMultiple(new FetchExpression(fetchxml));
            foreach (var doctype in result.Entities)
            {
                Entity task = new Entity("task");
                bool? isRequried = doctype.GetAttributeValue<bool?>("new_required");
                bool setReq = false;
                if (isRequried.HasValue && isRequried.Value == true)
                    setReq = true;
                task.Attributes.Add("subject", doc.Subject);
                task.Attributes.Add("new_is_required", setReq);
                task.Attributes.Add("new_task_type", new OptionSetValue(20));//scan(10=regular)

                task.Attributes.Add("ownerid", doc.OwnerId);
                task.Attributes.Add("regardingobjectid", doc.RegardingId);

                _service.Create(task);
            }


        }

        class BuilderFetch
        {
            Dictionary<string, bool> _conditions;
            string _xml = "";
            public BuilderFetch()
            {
                _conditions = new Dictionary<string, bool>();
            }

            public string Builder(DocModel doc)
            {
                _xml = @"< fetch version = '1.0' output - format = 'xml-platform' mapping = 'logical' distinct = 'false' >
              <entity name = 'new_doc_to_entity' >
                  <attribute name = 'new_doc_to_entityid' />
                  <attribute name = 'new_name' />
                  <attribute name = 'createdon' />
                  <attribute name = 'new_description' />
                  <attribute name = 'new_testdrive_ind' />
                  <attribute name = 'new_ready_for_delivery_ind' />
                  <attribute name = 'new_customer_class_glb' />
                  <attribute name = 'new_taradin_ind' />
                  <attribute name = 'new_required' />
                  <attribute name = 'new_loan_ind' />          
              <filter type = 'and'>";

                if (_conditions.Any())
                {
                    _xml = _xml + @"< filter type = 'and' > ";

                    foreach (var condition in _conditions.Keys)
                    {
                        _xml = _xml + "<condition attribute = '" + condition + "' operator= 'eq' value = '" + (_conditions[condition] == true ? "1" : "0") + "' />";
                    }
                    _xml = _xml + @"</filter>";

                }

                //<condition attribute = 'new_taradin_ind' operator= 'eq' value = '1' />
                //<condition attribute = 'new_loan_ind' operator= 'eq' value = '1' />
                //<condition attribute = 'new_ready_for_delivery_ind' operator= 'eq' value = '1' />
                //<condition attribute = 'new_testdrive_ind' operator= 'eq' value = '1' />

                _xml = _xml + @"<link-entity name = 'new_doc_type' from = 'new_doc_typeid' to = 'new_doc_type' visible = 'false' link-type = 'inner' alias = 'aa' >
                               <attribute name = 'new_description' />
                               <attribute name = 'new_name' />
                               <attribute name = 'new_doc_type' />
                         <filter type='and'>
                                < condition attribute = 'new_entity_scheme_name' operator= 'eq' value = '"+ doc.EntityName+ "' /> </ filter>  </link-entity>    </ entity >  </ fetch > ";
                return _xml;
            }

            public BuilderFetch AddConditionLoad(DocModel doc)
            {
                if (doc.IsLoan.HasValue)
                {
                    if (!_conditions.ContainsKey("new_loan_ind"))
                        _conditions.Add("new_loan_ind", doc.IsLoan.Value);
                }
                return this;
            }

            public BuilderFetch AddConditionDelivery(DocModel doc)
            {
                if (doc.Delivery.HasValue)
                {
                    if (!_conditions.ContainsKey("new_ready_for_delivery_ind"))
                        _conditions.Add("new_ready_for_delivery_ind", doc.Delivery.Value);
                }
                return this;
            }

            public BuilderFetch AddConditionTradeIn(DocModel doc)
            {
                if (doc.IsTardeIn.HasValue)
                {
                    if (!_conditions.ContainsKey("new_taradin_ind"))
                        _conditions.Add("new_taradin_ind", doc.IsTardeIn.Value);
                }
                return this;
            }

            public BuilderFetch AddConditionTestDrive(DocModel doc)
            {
                if (doc.IsTestDrive.HasValue)
                {
                    if (!_conditions.ContainsKey("new_testdrive_ind"))
                        _conditions.Add("new_testdrive_ind", doc.IsTestDrive.Value);
                }
                return this;
            }
        }
    }
}
