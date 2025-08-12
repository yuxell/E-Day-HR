namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.SiteAdministrator.Models.ViewModels.Company_VMs
{
    public class CreateCompany_VM
    {
        public string CompanyName { get; set; }
        public string Title { get; set; }
        public long MersisNum { get; set; }
        public long TaxNum { get; set; }
        public string TaxOffice { get; set; }
        public IFormFile LogoFile { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public int? NumberOfEmployees { get; set; }
        public string FoundationYear { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
