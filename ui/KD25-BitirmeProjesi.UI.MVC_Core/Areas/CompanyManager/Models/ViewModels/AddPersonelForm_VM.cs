using Microsoft.AspNetCore.Mvc.Rendering;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.CompanyManager.Models.ViewModels
{
    public class AddPersonelForm_VM
    {
        public AddPersonel_VM? Personel { get; set; }
        public SelectList? Companies { get; set; }
        public SelectList? Departments { get; set; }
        public SelectList? Proficiencies { get; set; }
        public SelectList? CurrencyTypes { get; set; }
    }
}
