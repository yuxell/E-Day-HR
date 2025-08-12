using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Login_DTOs
{
    public class UpdatePassword_DTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Şifre ve şifre tekrarı eşleşmiyor.")]
        public string ConfirmPassword { get; set; }
    }
}
