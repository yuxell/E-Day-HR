using KD25_BitirmeProjesi.UI.MVC_Core.Areas.CompanyManager.Models.ViewModels;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.LeaveRecord_DTOs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.SalaryAdvance_ViewModel;
using KD25_BitirmeProjesi.UI.MVC_Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager")]
    //[Route("Expence")]
    public class ExpenceController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration; // API end pointini alabilmek için
        private readonly string _baseUrl; // API end pointini online'a geçince tek bir yerden değiştirmek için

        public ExpenceController(IHttpClientFactory httpClientFactory, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _baseUrl = _configuration["ApiSettings:BaseUrl"]; // <<< constructor'da bir kere okur
        }
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" });
            }

            var userId = HttpContext.Session.GetInt32("UserId");
            var companyId = HttpContext.Session.GetInt32("CompanyId");

            var urlExpenceList = $"{_baseUrl}Expence/ListLast?CompanyID={companyId}";

            try
            {
                var expenses = await CustomHttpClientHelper.GetAsync<IEnumerable<ListExpence_VM>>(_httpClientFactory, token, urlExpenceList);

                ViewBag.BaseUri = _baseUrl;
                return View(expenses);
            }
            catch
            {
                ViewBag.Error = "Veriler alınırken bir hata oluştu.";
                return View(new ListExpence_VM());
            }
        }
    }
}
