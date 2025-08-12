using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.LeaveRecord_VMs
{
    public class AddLeaveRecordType_VM
    {
        public string LeaveRecordName { get; set; }

        public DateTime CreatedAt { get; set; }
        //public DateTime? UpdatedAt { get; set; }
        //public DateTime? DeletedAt { get; set; }
        public RecordStatus RecordStatus { get; set; }
    }
}
