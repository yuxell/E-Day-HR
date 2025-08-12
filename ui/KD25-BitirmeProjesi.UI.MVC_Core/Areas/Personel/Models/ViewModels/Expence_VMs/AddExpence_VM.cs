using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs
{
    public class AddExpence_VM
    {
        public int ExpenceTypeID { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public IFormFile FilePath { get; set; }
        public string Explanation { get; set; }
        public int AppUserID { get; set; }
    }
}
