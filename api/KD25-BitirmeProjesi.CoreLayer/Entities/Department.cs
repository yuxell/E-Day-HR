using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Abstracts;
using KD25_BitirmeProjesi.CoreLayer.Enums;

namespace KD25_BitirmeProjesi.CoreLayer.Entities
{
    public class Department : IBaseEntity
    {
        public int ID { get; set; }
        public string DepartmentName { get; set; }
        public int CompanyID { get; set; }

        //nav props
        public Company? Company { get; set; }
        public ICollection<AppUser>? AppUsers { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public RecordStatus RecordStatus { get; set; } = RecordStatus.IsAdded;
    }
}
