using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.SalaryAdvance_ViewModel
{
	public class AddSalaryAdvance_VM
	{
		public int AppUserID { get; set; }
		public string SalaryAdvanceType { get; set; }
		public CurrencyType CurrencyType { get; set; } = CurrencyType.TurkLirası;
		public decimal Amount { get; set; }
		public string? Explanation { get; set; }


		// Eğer CurrencyType ya da SalaryAdvanceType seçim olarak UI'de gösterilecekse (örneğin dropdown), ViewModel)
		//public List<SelectListItem>? CurrencyTypeList { get; set; }
		//public List<SelectListItem>? SalaryAdvanceTypeList { get; set; }



	}
}
