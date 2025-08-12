using KD25_BitirmeProjesi.CoreLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs
{
    public class UpdateExpenceType_DTO
    {
        public int ID { get; set; }
        public string ExpenceTypeName { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public RecordStatus RecordStatus { get; set; } = RecordStatus.IsUpdated;
    }
}
