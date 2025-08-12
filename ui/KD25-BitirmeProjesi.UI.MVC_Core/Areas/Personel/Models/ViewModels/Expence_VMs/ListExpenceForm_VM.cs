using Microsoft.AspNetCore.Mvc.Rendering;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs
{
    public class ListExpenceForm_VM
    {
        public ListExpence_VM ListExpence { get; set; }
        public SelectList ExpenceTypes{ get; set; }
    }
}
