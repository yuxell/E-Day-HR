using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.SalaryAdvance_ViewModel
{
		public class ListSalaryAdvance_VM
		{
			public int ID { get; set; }
			public DateTime RequestDate { get; set; }
			public string SalaryAdvanceType { get; set; }
			public string Explanation { get; set; }
			public decimal Amount { get; set; }
			public ApprovalStatus ApprovalStatus { get; set; }
			public DateTime? ResponseDate { get; set; }
			public CurrencyType CurrencyType { get; set; } // ViewModel'de genelde enum yerine string
			public string AppUserID { get; set; }
			public string AppUser { get; set; }
    }
	}

