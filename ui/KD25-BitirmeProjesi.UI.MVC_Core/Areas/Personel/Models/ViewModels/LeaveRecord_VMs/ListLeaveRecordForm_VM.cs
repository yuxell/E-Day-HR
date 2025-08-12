using Microsoft.AspNetCore.Mvc.Rendering;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.LeaveRecord_VMs
{
    public class ListLeaveRecordForm_VM
    {
        public ListLeaveRecord_VM LeaveRecord { get; set; }
        public SelectList LeaveRecordTypes { get; set; }
    }
}
