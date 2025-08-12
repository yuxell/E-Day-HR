using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.LeaveRecord_VMs
{
    public class UpdateLeaveRecord_VM
    {
        public int ID { get; set; }
        public string LeaveRecordType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalDays { get; set; }
        public string AppUser { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public RecordStatus RecordStatus { get; set; }
    }
}
