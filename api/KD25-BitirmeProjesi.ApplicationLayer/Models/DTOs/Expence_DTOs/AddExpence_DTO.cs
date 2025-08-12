using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.CoreLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs
{
    public class AddExpence_DTO
    {
        public CurrencyType Currency { get; set; }
        public int ExpenceTypeID { get; set; }
        public decimal Amount { get; set; }
        public string FilePath { get; set; }
        public int AppUserID { get; set; }
        public string Explanation { get; set; }
    }
}
