using Microsoft.AspNetCore.Mvc.Rendering;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.LeaveRecord_VMs
{
    public class AddLeaveRecordTypeForm_VM
    {
        public AddLeaveRecordType_VM LeaveRecordType { get; set; }
        public SelectList LeaveRecords { get; set; }
    }
}
