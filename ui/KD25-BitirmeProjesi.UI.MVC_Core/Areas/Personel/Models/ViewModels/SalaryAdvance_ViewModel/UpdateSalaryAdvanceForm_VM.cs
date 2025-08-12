using System.ComponentModel.DataAnnotations;
namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.SalaryAdvance_ViewModel
{
	public class UpdateSalaryAdvanceForm_VM
	{
		[Required(ErrorMessage = "Onay durumu zorunludur.")]
		public string ApprovalStatus { get; set; } // Enum yerine string, UI ile uyumlu

		public string? ResponseDate { get; set; }  // string olarak tutuluyor (örn. kullanıcıdan gelen veri)
	}
}
