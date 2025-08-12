namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.SalaryAdvance_ViewModel
{
	public class ListSalaryAdvanceForm_VM
	{
		public int ID { get; set; }
		public DateTime? RequestDate { get; set; }
		public string SalaryAdvanceType { get; set; }
		public string Explanation { get; set; }
		public decimal Amount { get; set; }
		public string ApprovalStatus { get; set; } // Enum yerine string format
		public string? ResponseDate { get; set; }  // Format: string, kullanıcıya gösterim için
		public string CurrencyType { get; set; }   // Enum yerine string format
		public string AppUser { get; set; }
	}
}
