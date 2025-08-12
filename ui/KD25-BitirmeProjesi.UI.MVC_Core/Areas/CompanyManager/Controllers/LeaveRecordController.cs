using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager")]
    [Route("LeaveRecord")]
    public class LeaveRecordController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration; // API end pointini alabilmek için
        private readonly string _baseUrl; // API end pointini online'a geçince tek bir yerden değiştirmek için

        public LeaveRecordController(IHttpClientFactory httpClientFactory, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _baseUrl = _configuration["ApiSettings:BaseUrl"]; // <<< constructor'da bir kere okur
        }
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            return View();
        }
    }
}
