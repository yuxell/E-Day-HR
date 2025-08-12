using Microsoft.AspNetCore.Mvc.Rendering;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.LeaveRecord_VMs
{
    public class UpdateLeaveRecordForm_VM
    {
        public UpdateLeaveRecord_VM LeaveRecord { get; set; }
        public SelectList LeaveRecordTypes { get; set; }
    }
}
