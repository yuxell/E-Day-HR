using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Enums;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Advance_DTOs
{
	public class UpdateSalaryAdvance_DTO
	{
        public int Id { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
		public string? ResponseDate { get; set; }
	}
}
