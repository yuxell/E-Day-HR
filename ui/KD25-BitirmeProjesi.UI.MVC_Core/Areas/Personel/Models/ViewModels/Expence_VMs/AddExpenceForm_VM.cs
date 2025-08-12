using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.Expence_DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs
{
    public class AddExpenceForm_VM
    {
        public AddExpence_VM Expence { get; set; }
        public SelectList ExpenceTypes { get; set; }
        public SelectList CurrencyTypes { get; set; } // Enum'ı selectList'e dönüştürmek için
    }
}
