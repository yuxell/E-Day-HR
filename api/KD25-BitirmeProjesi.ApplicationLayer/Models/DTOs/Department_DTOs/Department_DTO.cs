using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Enums;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Department_DTOs
{
    public class Department_DTO
    {
        public int ID { get; set; }
        public string DepartmentName { get; set; }
        public int CompanyID { get; set; }
        //public DateTime CreatedAt { get; set; } 
        //public DateTime? UpdatedAt { get; set; }
        //public DateTime? DeletedAt { get; set; }
        public RecordStatus RecordStatus { get; set; } 
    }
}
