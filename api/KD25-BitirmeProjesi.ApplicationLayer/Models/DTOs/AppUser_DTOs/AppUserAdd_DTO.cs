using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.AppUser_DTOs
{
    public class AppUserAdd_DTO
    {
        
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string Surname { get; set; }
        public string? SecondSurname { get; set; }
        public string? Avatar { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BirthPlace { get; set; }
        public string NationalID { get; set; }
       
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int CompanyID { get; set; }
        public int DepartmentID { get; set; }
        public string? Proficiency { get; set; } // Meslek
        public string? Address { get; set; }
        public decimal? Salary { get; set; }
        public int? CurrencyTypeID { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }


        public string UserName { get; set; } 


        [Compare("PasswordConfirmed")]
        public string Password { get; set; }
        public string PasswordConfirmed { get; set; }

    }
}
