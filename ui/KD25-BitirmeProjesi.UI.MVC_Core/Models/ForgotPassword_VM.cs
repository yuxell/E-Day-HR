using System.ComponentModel.DataAnnotations;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Models
{
    public class ForgotPassword_VM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string ClientAppUrl { get; set; } = "https://localhost:7071"; // MVC URL'in 
    }
}
