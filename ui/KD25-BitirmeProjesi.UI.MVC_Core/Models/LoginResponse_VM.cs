namespace KD25_BitirmeProjesi.UI.MVC_Core.Models
{
    public class LoginResponse_VM
    {
        public string Token { get; set; }
        public string RoleName { get; set; }
        public int UserId { get; set; }
        public int? CompanyId { get; set; }
        public string FullName { get; set; }
    }
}
