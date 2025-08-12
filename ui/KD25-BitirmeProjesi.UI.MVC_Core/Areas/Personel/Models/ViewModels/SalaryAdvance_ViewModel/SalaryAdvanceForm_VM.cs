using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.SalaryAdvance_ViewModel
{
    public class SalaryAdvanceForm_VM
    {
        public int ID { get; set; } // Güncelleme işlemleri için gerekli

        [Display(Name = "Avans Talep Tarihi")]
        public DateTime? RequestDate { get; set; }

        [Display(Name = "Avans Türü")]
        [Required(ErrorMessage = "Avans türü seçilmelidir.")]
        public string SalaryAdvanceType { get; set; }

        [Display(Name = "Açıklama")]
        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        public string Explanation { get; set; }

        [Display(Name = "Tutar")]
        [Required(ErrorMessage = "Tutar girilmelidir.")]
        [Range(1, double.MaxValue, ErrorMessage = "Tutar 1'den büyük olmalıdır.")]
        public decimal Amount { get; set; }

        [Display(Name = "Onay Durumu")]
        public ApprovalStatus ApprovalStatus { get; set; }

        [Display(Name = "Yanıt Tarihi")]
        public DateTime? ResponseDate { get; set; }

        [Display(Name = "Para Birimi")]
        [Required(ErrorMessage = "Para birimi seçilmelidir.")]
        public CurrencyType CurrencyType { get; set; }

        public int AppUserID { get; set; } // Session'dan doldurulacak
    }
}
