using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs
{
    public class AddExpenceType_DTO
    {
        public string ExpenceTypeName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
