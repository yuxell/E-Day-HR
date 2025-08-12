using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Company_DTOs
{
    public class CreateCompany_DTO
    {
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public long MersisNum { get; set; }
        public long TaxNum { get; set; }
        public string TaxOffice { get; set; }
        public string Logo { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int? NumberOfEmployees { get; set; }
        public string FoundationYear { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
