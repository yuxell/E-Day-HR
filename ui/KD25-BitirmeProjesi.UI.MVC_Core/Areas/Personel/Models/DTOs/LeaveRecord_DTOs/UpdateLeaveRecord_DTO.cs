using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.LeaveRecord_DTOs
{
    public class UpdateLeaveRecord_DTO
    {
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime ResponseDate { get; set; }
    }
}
