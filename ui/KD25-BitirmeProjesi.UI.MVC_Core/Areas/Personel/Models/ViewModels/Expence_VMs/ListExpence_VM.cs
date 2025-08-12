using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs
{
    public class ListExpence_VM
    {
        public int ID { get; set; }
        public string ExpenceType { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public string FilePath { get; set; } // listelemede kullanılmayacaksa kaldırabiliriz
        public string Explanation { get; set; } // listelemede kullanılmayacaksa kaldırabiliriz
        public string AppUser { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }

    }
}
