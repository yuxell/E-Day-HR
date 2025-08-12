using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.CoreLayer.Enums;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Advance_DTOs
{
	public class AddSalaryAdvance_DTO
	{
		public string SalaryAdvanceType { get; set; }
		public string Explanation { get; set; }
		public decimal Amount { get; set; }
		public CurrencyType CurrencyType { get; set; }
		public int AppUserID { get; set; }
	}
}
