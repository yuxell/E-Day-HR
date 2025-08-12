namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.SiteAdministrator.Models.ViewModels
{
    public class AppUserDetail_VM
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Surname { get; set; }
        public string SecondSurname { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string NationalID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Proficiency { get; set; }
        public string Address { get; set; }
        public decimal? Salary { get; set; }
        public string CurrencyType { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
    }
}
