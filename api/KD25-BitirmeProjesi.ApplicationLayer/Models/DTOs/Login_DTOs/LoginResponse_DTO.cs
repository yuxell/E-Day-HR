using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Login_DTOs
{
    public class LoginResponse_DTO
    {
        //public int AppUserID { get; set; }
        //public string UserName { get; set; }
        //public string Email { get; set; }
        //public string Role { get; set; }

        public string Token { get; set; }
        public string RoleName { get; set; }
        public int UserId { get; set; }
        public int? CompanyId { get; set; }
        public string FullName { get; set; }
    }
}
