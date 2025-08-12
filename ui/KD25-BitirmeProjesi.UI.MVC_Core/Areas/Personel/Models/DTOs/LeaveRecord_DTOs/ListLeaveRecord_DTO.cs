using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.LeaveRecord_DTOs
{
    public class ListLeaveRecord_DTO
    {
        public int ID { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public int TotalDays { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveRecordType { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string AppUser { get; set; }
        public RecordStatus RecordStatus { get; set; }
    }
}
