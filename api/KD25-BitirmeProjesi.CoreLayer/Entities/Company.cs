using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Abstracts;
using KD25_BitirmeProjesi.CoreLayer.Enums;

namespace KD25_BitirmeProjesi.CoreLayer.Entities
{
    public class Company : IBaseEntity
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }

        // Sonradan eklenen özellikler
        public string Title { get; set; }
        public long MersisNum { get; set; }
        public long TaxNum { get; set; }
        public string TaxOffice { get; set; }
        public string Logo { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int? NumberOfEmployees { get; set; } = 0;
        public string FoundationYear { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public bool IsActive { get; set; }


        // Nav Prop
        public ICollection<Department>? Departments { get; set; }
        public ICollection<AppUser>? AppUsers { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public RecordStatus RecordStatus { get; set; } = RecordStatus.IsAdded;
    }
}
