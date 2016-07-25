
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lior.Xrm.Workflow.BL
{

    public abstract class LoadDocParametersFactory
    {
        protected Entity _target;
        protected IOrganizationService _service;
        protected string _subject;
        Entity _preImage;
        public LoadDocParametersFactory(IOrganizationService service, Entity target, string subject)
        {
            _target = target;
            _service = service;
            _subject = subject;
        }
        public abstract DocModel GetParameters();

        protected DocModel Loadhelper()
        {
            DocModel doc = new DocModel();
            doc.Subject = _subject;
            doc.EntityName = _target.LogicalName;
            var preImage = LoadPreImage();
            var ownerid = new EntityReference();

            if (preImage != null && preImage.GetAttributeValue<EntityReference>("ownerid") != null)
                doc.OwnerId = preImage.GetAttributeValue<EntityReference>("ownerid");
            if (_target != null && _target.GetAttributeValue<EntityReference>("ownerid") != null)
                doc.OwnerId = _target.GetAttributeValue<EntityReference>("ownerid");
            doc.RegardingId = _target.ToEntityReference();

            return doc;
        }

        protected Entity LoadPreImage()
        {
            if (_preImage == null)
                _preImage = _service.Retrieve(_target.LogicalName, _target.Id, new Microsoft.Xrm.Sdk.Query.ColumnSet(true));
            return _preImage;
        }


    }
    
    public class OpportunityParametersFactory : LoadDocParametersFactory
    {
        public OpportunityParametersFactory(IOrganizationService service, Entity target, string subject) : base(service, target, subject)
        {

        }

        public override DocModel GetParameters()
        {
            DocModel doc = Loadhelper();
            var preImage = LoadPreImage();
            doc.IsTardeIn= _target.GetAttributeValue<bool?>("new_is_tradein");
            doc.IsTestDrive = _target.GetAttributeValue<bool?>("new_is_testdrive");
            doc.IsLoan = _target.GetAttributeValue<bool?>("new_is_finance");
            //doc.Delivery = _target.GetAttributeValue<bool?>("new_is_finance");
            
            return doc;
        }
    }

    public class OrderParametersFactory : LoadDocParametersFactory
    {
        public OrderParametersFactory(IOrganizationService service, Entity target, string subject) : base(service, target, subject)
        {

        }

        public override DocModel GetParameters()
        {
            DocModel doc = Loadhelper();
            var preImage = LoadPreImage();
            doc.Delivery = _target.GetAttributeValue<bool?>("new_ready_for_delivery");
            return doc;
        }
    }


}
