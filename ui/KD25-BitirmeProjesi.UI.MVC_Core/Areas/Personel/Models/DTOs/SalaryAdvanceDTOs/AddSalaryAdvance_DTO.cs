namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.SalaryAdvanceDTOs
{
	public class AddSalaryAdvance_DTO
	{
		//public DateTime RequestDate { get; set; }
		public int SalaryAdvanceType { get; set; }
		public string Explanation { get; set; }
		public decimal Amount { get; set; }
		//public ApprovalStatus ApprovalStatus { get; set; }
		//public DateTime ResponseDate { get; set; }
		public int CurrencyType { get; set; }
		public int AppUserId { get; set; }
	}
}
