namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.CompanyManager.Models.ViewModels
{
    public class ListPersonel_VM
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string? Avatar { get; set; }
        public string Department { get; set; }
        public string? Proficiency { get; set; } // Meslek
    }
}
