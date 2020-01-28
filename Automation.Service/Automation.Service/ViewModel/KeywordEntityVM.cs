using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automation.Service.ViewModel
{
    public class KeywordEntityVM : BaseEntityVM
    {
        public string FunctionName { get; set; }
        public string StepDescription { get; set; }
        public string ActionOrKeyword { get; set; }
        public string ObjectLogicalName { get; set; }
        public string Run { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string Param4 { get; set; }
        public string Param5 { get; set; }
        public string Param6 { get; set; }
        public string Param7 { get; set; }
        public string Param8 { get; set; }
        public string Param9 { get; set; }
        public string Param10 { get; set; }
        public string Module { get; set; }
        public int StatusID { get; set; }
        public int CUDStatusID { get; set; }
        public bool? IsLocked { get; set; }
        public int? LockedByUser { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UserId { get; set; }
    }

    public class KeywordEntity_MapVM : BaseEntityVM
    {
        public int MasterKeywordID { get; set; }
        public string Module { get; set; }
        public string FunctionName { get; set; }
        public string StepDescription { get; set; }
        public string ActionOrKeyword { get; set; }
        public string ObjectLogicalName { get; set; }
        public string Run { get; set; }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string Param4 { get; set; }
        public string Param5 { get; set; }
        public string Param6 { get; set; }
        public string Param7 { get; set; }
        public string Param8 { get; set; }
        public string Param9 { get; set; }
        public string Param10 { get; set; }
        public string Param11 { get; set; }
        public string Param12 { get; set; }
        public string Param13 { get; set; }
        public string Param14 { get; set; }
        public string Param15 { get; set; }
        public string Param16 { get; set; }
        public string Param17 { get; set; }
        public string Param18 { get; set; }
        public string Param19 { get; set; }
        public string Param20 { get; set; }
        public int? LockedByUser { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UserId { get; set; }
    }
}
