using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automation.Service.ViewModel
{
    public class ModuleControllerVM : BaseEntityVM
    {
        public int SLNO { get; set; }
        public string ModuleID { get; set; }
        public int ModuleSeqID { get; set; }
        public string MachineID { get; set; }
        public int MachineSequenceID { get; set; }
        public string Run { get; set; }
        public bool? IsLocked { get; set; }
        public int? LockedByUser { get; set; }
        public int StatusID { get; set; }
        public int CUDStatusID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UserId { get; set; }
    }

    public class ModuleController_MapVM : BaseEntityVM
    {
        public int RefId { get; set; }
        public string ModuleID { get; set; }
        public int ModuleSeqID { get; set; }
        public string MachineID { get; set; }
        public int MachineSequenceID { get; set; }
        public string Run { get; set; }
        public int? LockedByUser { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UserId { get; set; }
    }

    public class TestControllerVM : BaseEntityVM
    {
        public string FeatureID { get; set; }
        public string TestCaseID { get; set; }
        public string Run { get; set; }
        public string Iterations { get; set; }
        public string Browsers { get; set; }
        public int SequenceID { get; set; }
        public string TestType { get; set; }
        public string JiraID { get; set; }
        public int StepsCount { get; set; }
        public string TestScriptName { get; set; }
        public string TestScriptDescription { get; set; }
        public bool? IsLocked { get; set; }
        public int? LockedByUser { get; set; }
        public int StatusID { get; set; }
        public int CUDStatusID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UserId { get; set; }
    }

    public class TestController_MapVM : BaseEntityVM
    {
        public int RefId { get; set; }
        public string FeatureID { get; set; }
        public string TestCaseID { get; set; }
        public string Run { get; set; }
        public string Iterations { get; set; }
        public string Browsers { get; set; }
        public int SequenceID { get; set; }
        public string TestType { get; set; }
        public string JiraID { get; set; }
        public int StepsCount { get; set; }
        public string TestScriptName { get; set; }
        public string TestScriptDescription { get; set; }
        public int? LockedByUser { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UserId { get; set; }
    }

    public class BrowserControllerVM : BaseEntityVM
    {
        public string VMID { get; set; }
        public string Browser { get; set; }
        public string Exec { get; set; }
        public int StatusID { get; set; }
        public int CUDStatusID { get; set; }
        public bool? IsLocked { get; set; }
        public int? LockedByUser { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UserId { get; set; }
    }
}
