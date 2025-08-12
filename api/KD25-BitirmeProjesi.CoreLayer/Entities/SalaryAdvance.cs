using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Abstracts;
using KD25_BitirmeProjesi.CoreLayer.Enums;
using KD25_BitirmeProjesi.CoreLayer.Interfaces;

namespace KD25_BitirmeProjesi.CoreLayer.Entities
{
    public class SalaryAdvance : IBaseEntity
	{
        public int ID { get; set; }
        public string SalaryAdvanceType { get; set; }
        public CurrencyType CurrencyType { get; set; } = CurrencyType.TurkLirası;
        public decimal Amount { get; set; }
        public string? Explanation { get; set; }
        public int AppUserID { get; set; }

        public DateTime? RequestDate { get; set; } = DateTime.UtcNow;
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
        public DateTime? ResponseDate { get; set; }

        //nav porps
        public AppUser? AppUser {  get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public RecordStatus RecordStatus { get; set; } = RecordStatus.IsAdded;


    }
}
