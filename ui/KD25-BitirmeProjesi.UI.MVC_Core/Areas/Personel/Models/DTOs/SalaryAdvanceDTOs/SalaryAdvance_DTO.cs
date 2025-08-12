using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.SalaryAdvanceDTOs
{
    public class SalaryAdvance_DTO
    {
        public int ID { get; set; }
        public DateTime? RequestDate { get; set; }
        public string SalaryAdvanceType { get; set; }
        public string Explanation { get; set; }
        public decimal Amount { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime? ResponseDate { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public int AppUserID { get; set; }
    }
}
