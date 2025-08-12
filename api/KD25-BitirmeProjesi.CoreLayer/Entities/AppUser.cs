using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.CoreLayer.Enums;
using Microsoft.AspNetCore.Identity;

namespace KD25_BitirmeProjesi.CoreLayer.Entities
{
    public class AppUser : IdentityUser<int>
    {
      
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string Surname { get; set; }
        public string? SecondSurname { get; set; }
        public string? Avatar { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BirthPlace { get; set; }
        public string NationalID { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CompanyID { get; set; }
        public int? DepartmentID { get; set; }
        public string? Proficiency { get; set; } // Meslek
        public string? Address { get; set; }
        public decimal? Salary { get; set; }
        public CurrencyType CurrencyType { get; set; } = CurrencyType.TurkLirası;



        // Email, Phone, UserName Identity den gelen kullanılacak

        // Nav Prop
        public Company? Company { get; set; }
        public Department? Department { get; set; }
        public ICollection<Expence>? Expences { get; set; }
        public ICollection<LeaveRecord>? LeaveRecords { get; set; }
        public ICollection<SalaryAdvance>? SalaryAdvances { get; set; }
    }
}
