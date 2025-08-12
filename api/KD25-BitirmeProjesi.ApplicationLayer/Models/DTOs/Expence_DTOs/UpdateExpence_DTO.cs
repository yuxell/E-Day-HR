using KD25_BitirmeProjesi.CoreLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs
{
    public class UpdateExpence_DTO
    {
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime ResponseDate { get; set; }
    }
}
