using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lior.Xrm.Xrm;

namespace Lior.Xrm.BusinessUnit.BL.ServiceEventFactory.Factory
{
    public class CashierParameters
    {
        public Guid? ParamSourcetotarget6 { get; set; }
        public Guid? ParamSourcetotarget7 { get; set; }
        public Guid? ParamSourcetotarget8 { get; set; }
        public Guid? ParamSourcetotarget9 { get; set; }
        public Guid? ParamSourcetotarget10 { get; set; }


        public decimal ParamCost6 { get; set; }
        public decimal ParamCost7 { get; set; }
        public decimal ParamCost8 { get; set; }
        public decimal ParamCost9 { get; set; }
        public decimal ParamCost10 { get; set; }



       
    }
    public class CashierHelper
    {
        new_incidentserviceparameter _paramters;

        CashierParameters _cacherarameters;
        public CashierHelper(new_incidentserviceparameter paramters)
        {
            _paramters = paramters;
            Load();
        }

        


         void Load()
        {
            _cacherarameters = new CashierParameters();

            if (_paramters.Attributes.Contains("new_SourceToTarget6".ToLower()))
                _cacherarameters.ParamSourcetotarget6 = _paramters.new_SourceToTarget6 != null ? _paramters.new_SourceToTarget6.Id : (Guid?)null;
            if (_paramters.Attributes.Contains("new_Cost6".ToLower()))
                _cacherarameters.ParamCost6 = _paramters.new_Cost6.HasValue ? _paramters.new_Cost6.Value : 0;


            if (_paramters.Attributes.Contains("new_SourceToTarget7".ToLower()))
                _cacherarameters.ParamSourcetotarget7 = _paramters.new_SourceToTarget7 != null ? _paramters.new_SourceToTarget7.Id : (Guid?)null;
            if (_paramters.Attributes.Contains("new_Cost7".ToLower()))
                _cacherarameters.ParamCost7 = _paramters.new_Cost7.HasValue ? _paramters.new_Cost7.Value : 0;

            if (_paramters.Attributes.Contains("new_SourceToTarget8".ToLower()))
                _cacherarameters.ParamSourcetotarget8 = _paramters.new_SourceToTarget8 != null ? _paramters.new_SourceToTarget8.Id : (Guid?)null;
            if (_paramters.Attributes.Contains("new_Cost8".ToLower()))
                _cacherarameters.ParamCost8 = _paramters.new_Cost8.HasValue ? _paramters.new_Cost8.Value : 0;

            if (_paramters.Attributes.Contains("new_SourceToTarget9".ToLower()))
                _cacherarameters.ParamSourcetotarget9 = _paramters.new_SourceToTarget9 != null ? _paramters.new_SourceToTarget9.Id : (Guid?)null;
            if (_paramters.Attributes.Contains("new_Cost9".ToLower()))
                _cacherarameters.ParamCost9 = _paramters.new_Cost9.HasValue ? _paramters.new_Cost9.Value : 0;

            if (_paramters.Attributes.Contains("new_SourceToTarget10".ToLower()))
                _cacherarameters.ParamSourcetotarget10 = _paramters.new_SourceToTarget10 != null ? _paramters.new_SourceToTarget10.Id : (Guid?)null;
            if (_paramters.Attributes.Contains("new_Cost10".ToLower()))
                _cacherarameters.ParamCost10 = _paramters.new_Cost10.HasValue ? _paramters.new_Cost10.Value : 0;
        }

        public decimal FindMatchCost(Guid? source)
        {
            if (!source.HasValue)
                return 0;

            if (_cacherarameters.ParamSourcetotarget6.HasValue && _cacherarameters.ParamSourcetotarget6.Value == source.Value)
                return _cacherarameters.ParamCost6;
            if (_cacherarameters.ParamSourcetotarget7.HasValue && _cacherarameters.ParamSourcetotarget7.Value == source.Value)
                return _cacherarameters.ParamCost7;
            if (_cacherarameters.ParamSourcetotarget8.HasValue && _cacherarameters.ParamSourcetotarget8.Value == source.Value)
                return _cacherarameters.ParamCost8;
            if (_cacherarameters.ParamSourcetotarget9.HasValue && _cacherarameters.ParamSourcetotarget9.Value == source.Value)
                return _cacherarameters.ParamCost9;
            if (_cacherarameters.ParamSourcetotarget10.HasValue && _cacherarameters.ParamSourcetotarget10.Value == source.Value)
                return _cacherarameters.ParamCost10;
            
            return 0;

        }




    }
}
