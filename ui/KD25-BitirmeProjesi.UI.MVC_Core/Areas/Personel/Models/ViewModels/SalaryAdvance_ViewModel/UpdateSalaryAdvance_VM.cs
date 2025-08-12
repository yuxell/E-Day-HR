using System.ComponentModel.DataAnnotations;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.PresentationLayer.Models.VMs.AdvanceVMs
{
	public class UpdateSalaryAdvance_VM
	{
		public int Id { get; set; }
        public string SalaryAdvanceType { get; set; }
        public CurrencyType CurrencyType { get; set; } 
        public decimal Amount { get; set; }
        public string? Explanation { get; set; }
        public int AppUserID { get; set; }

        [Required]
		public string ApprovalStatus { get; set; }  // Enum yerine string, UI ile uyumlu

		public string? ResponseDate { get; set; }   // Tarih formatı string olarak kalsın istersen (örn. kullanıcıdan gelen veri için)
		
	}
}
