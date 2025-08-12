using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.AppUser_DTOs
{
    public class AppUserList_DTO
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string Surname { get; set; }
        public string? SecondSurname { get; set; }
        public string? Avatar { get; set; }
        public string Department { get; set; }
        public string? Proficiency { get; set; } // Meslek
        public string? Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
 
    }
}
