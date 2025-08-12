using Microsoft.AspNetCore.Mvc.Rendering;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs
{
    public class UpdateExpenceForm_VM
    {
        public UpdateExpence_VM UpdateExpence { get; set; }
        public SelectList ExpenceTypes { get; set; }
    }
}
