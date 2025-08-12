namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.LeaveRecord_DTOs
{
    public class AddLeaveRecord_DTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LeaveRecordTypeID { get; set; }
        public int AppUserID { get; set; }
    }
}
