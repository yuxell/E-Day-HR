using System.ComponentModel.DataAnnotations;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums
{
    public enum CurrencyType
    {
        [Display(Name = "Türk Lirası")]
        TurkLirası = 1,
        [Display(Name = "Euro")]
        Euro = 2,
        [Display(Name = "Dolar")]
        Dolar = 3
    }
}
