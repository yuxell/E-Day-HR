using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs
{
    public class UpdateExpence_VM
    {
        // Update işleminde sadece ApprovalStatus ve RecordStatus değişecek, bunlar ve ID bilgisi yeterli olabilir, diğerleri sadece gösterim amaçlı kullanılabilir. Burada yapılacak işlemler netleştirilmeli ve buna göre gereksiz property var ise kaldırılabilir
        public int ID { get; set; }
        public int ExpenceTypeID { get; set; }
        public decimal Amount { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public IFormFile FilePath { get; set; }
        public string Explanation { get; set; }
        public int AppUserID { get; set; }


        public ApprovalStatus ApprovalStatus { get; set; }
        public RecordStatus RecordStatus { get; set; }
    }
}
