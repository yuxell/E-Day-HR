using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.LeaveRecord_VMs
{
    public class AddLeaveRecord_VM
    {
        public int LeaveRecordTypeID { get; set; }
        public int AppUserID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        //public DateTime RequestDate { get; set; } 
        public ApprovalStatus ApprovalStatus { get; set; }
        //public DateTime? ResponseDate { get; set; }
    }
}
