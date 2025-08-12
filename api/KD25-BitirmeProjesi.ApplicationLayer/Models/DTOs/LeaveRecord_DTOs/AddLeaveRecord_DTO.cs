using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Entities;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.LeaveRecord_DTOs
{
    public class AddLeaveRecord_DTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LeaveRecordTypeID { get; set; }
        public int AppUserID { get; set; }
    }
}
