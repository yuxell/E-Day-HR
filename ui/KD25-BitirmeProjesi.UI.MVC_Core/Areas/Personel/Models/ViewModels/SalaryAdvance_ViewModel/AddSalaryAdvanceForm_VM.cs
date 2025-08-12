using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.SalaryAdvance_ViewModel
{
	public class AddSalaryAdvanceForm_VM
	{
		public string SalaryAdvanceType { get; set; }

		public CurrencyType CurrencyType { get; set; } = CurrencyType.TurkLirası;

		public decimal Amount { get; set; }

		public string? Explanation { get; set; }

		//Dosya Yükleme Alanı
		public IFormFile? Document { get; set; }

		//Dropdown Seçenekleri
		public List<SelectListItem>? CurrencyTypeList { get; set; }
		public List<SelectListItem>? SalaryAdvanceTypeList { get; set; }
	}
}
