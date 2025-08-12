using Microsoft.AspNetCore.Mvc.Rendering;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.LeaveRecord_VMs
{
    public class AddLeaveRecordForm_VM
    {
        public AddLeaveRecord_VM LeaveRecordForm_VM { get; set; }
        public SelectList LeaveRecordTypes { get; set; }
        public SelectList ApppUsers { get; set; }

    }
}
