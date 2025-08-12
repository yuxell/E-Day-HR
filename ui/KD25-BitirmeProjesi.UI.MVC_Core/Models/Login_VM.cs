using System.ComponentModel.DataAnnotations;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Models
{
    /// <summary>
    /// Kullanıcı giriş ekranı için gerekli olan ViewModel.
    /// </summary>
    public class Login_VM
    {
        [Required(ErrorMessage = "Email zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre gereklidir.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

    }
}
