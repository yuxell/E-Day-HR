using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.Expence_DTOs
{
    public class ListExpence_DTO
    {
        public int ID { get; set; }
        public string AppUser { get; set; }
        public string ExpenceType { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public string FilePath { get; set; }
        public string Explanation { get; set; }

        public DateTime? ApprovalDate { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime? ResponseDate { get; set; }
    }
}
