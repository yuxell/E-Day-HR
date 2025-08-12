using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.Expence_DTOs
{
    public class AddExpence_DTO
    {
        public CurrencyType Currency { get; set; }
        public int ExpenceTypeID { get; set; }
        public decimal Amount { get; set; }
        public string FilePath { get; set; }
        public int AppUserID { get; set; }
        public string Explanation { get; set; }
    }
}
