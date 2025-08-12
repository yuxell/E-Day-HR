using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.CoreLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs
{
    public class ListExpences_DTO
    {
        public int ID { get; set; }
        public string AppUser { get; set; }
        public string ExpenceType { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType Currency { get; set; }
        public string FilePath { get; set; }
        public string Explanation { get; set; }

        public DateTime? ApprovalDate { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime? ResponseDate { get; set; }
        
    }
}
