using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lior.Xrm.Xrm;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Factory
{
    public class CallCenterParameters
    {
        public Guid? ParamSourcetotarget1 { get; set; }
        public Guid? ParamSourcetotarget2 { get; set; }
        public Guid? ParamSourcetotarget3 { get; set; }
        public Guid? ParamSourcetotarget4 { get; set; }
        public Guid? ParamSourcetotarget5 { get; set; }


        public decimal ParamCost1 { get; set; }
        public decimal ParamCost2 { get; set; }
        public decimal ParamCost3 { get; set; }
        public decimal ParamCost4 { get; set; }
        public decimal ParamCost5 { get; set; }



       
    }
    public class CallCenterHelper
    {
        new_incidentserviceparameter _paramters;
        CallCenterParameters _callCenterParameters;
        public CallCenterHelper(new_incidentserviceparameter paramters)
        {
            _paramters = paramters;
            Load();
        }

        


         void Load()
        {
             _callCenterParameters = new CallCenterParameters();

            if (_paramters.Attributes.Contains("new_SourceToTarget1".ToLower()))
                _callCenterParameters.ParamSourcetotarget1 = _paramters.new_SourceToTarget1 != null ? _paramters.new_SourceToTarget1.Id : (Guid?)null;
            if (_paramters.Attributes.Contains("new_Cost1".ToLower()))
                _callCenterParameters.ParamCost1 = _paramters.new_Cost1.HasValue ? _paramters.new_Cost1.Value : 0;


            if (_paramters.Attributes.Contains("new_SourceToTarget2".ToLower()))
                _callCenterParameters.ParamSourcetotarget2 = _paramters.new_SourceToTarget2 != null ? _paramters.new_SourceToTarget2.Id : (Guid?)null;
            if (_paramters.Attributes.Contains("new_Cost2".ToLower()))
                _callCenterParameters.ParamCost2 = _paramters.new_Cost2.HasValue ? _paramters.new_Cost2.Value : 0;

            if (_paramters.Attributes.Contains("new_SourceToTarget3".ToLower()))
                _callCenterParameters.ParamSourcetotarget3 = _paramters.new_SourceToTarget3 != null ? _paramters.new_SourceToTarget3.Id : (Guid?)null;
            if (_paramters.Attributes.Contains("new_Cost3".ToLower()))
                _callCenterParameters.ParamCost3 = _paramters.new_Cost3.HasValue ? _paramters.new_Cost3.Value : 0;

            if (_paramters.Attributes.Contains("new_SourceToTarget4".ToLower()))
                _callCenterParameters.ParamSourcetotarget4 = _paramters.new_SourceToTarget4 != null ? _paramters.new_SourceToTarget4.Id : (Guid?)null;
            if (_paramters.Attributes.Contains("new_Cost4".ToLower()))
                _callCenterParameters.ParamCost4 = _paramters.new_Cost4.HasValue ? _paramters.new_Cost4.Value : 0;

            if (_paramters.Attributes.Contains("new_SourceToTarget5".ToLower()))
                _callCenterParameters.ParamSourcetotarget5 = _paramters.new_SourceToTarget5 != null ? _paramters.new_SourceToTarget5.Id : (Guid?)null;
            if (_paramters.Attributes.Contains("new_Cost5".ToLower()))
                _callCenterParameters.ParamCost5 = _paramters.new_Cost5.HasValue ? _paramters.new_Cost5.Value : 0;
        }

        public decimal FindMatchCost(Guid? source)
        {
            if (!source.HasValue)
                return 0;

            if (_callCenterParameters.ParamSourcetotarget1.HasValue && _callCenterParameters.ParamSourcetotarget1.Value == source.Value)
                return _callCenterParameters.ParamCost1;
            if (_callCenterParameters.ParamSourcetotarget2.HasValue && _callCenterParameters.ParamSourcetotarget2.Value == source.Value)
                return _callCenterParameters.ParamCost2;
            if (_callCenterParameters.ParamSourcetotarget3.HasValue && _callCenterParameters.ParamSourcetotarget3.Value == source.Value)
                return _callCenterParameters.ParamCost3;
            if (_callCenterParameters.ParamSourcetotarget4.HasValue && _callCenterParameters.ParamSourcetotarget4.Value == source.Value)
                return _callCenterParameters.ParamCost4;
            if (_callCenterParameters.ParamSourcetotarget5.HasValue && _callCenterParameters.ParamSourcetotarget5.Value == source.Value)
                return _callCenterParameters.ParamCost5;
            
            return 0;

        }




    }
}
