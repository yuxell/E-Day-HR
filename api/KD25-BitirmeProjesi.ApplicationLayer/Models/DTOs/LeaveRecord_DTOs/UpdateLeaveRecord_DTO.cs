using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Enums;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.LeaveRecord_DTOs
{
    public class UpdateLeaveRecord_DTO
    {
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime ResponseDate { get; set; }
    }
}
