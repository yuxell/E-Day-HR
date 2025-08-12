using KD25_BitirmeProjesi.UI.MVC_Core.Areas.CompanyManager.Models.ViewModels;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.LeaveRecord_DTOs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.SalaryAdvance_ViewModel;
using KD25_BitirmeProjesi.UI.MVC_Core.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager")]
    [Route("CompanyManager")]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration; // API end pointini alabilmek için
        private readonly string _baseUrl; // API end pointini online'a geçince tek bir yerden değiştirmek için

        public HomeController(IHttpClientFactory httpClientFactory, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
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
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }

            var userId = HttpContext.Session.GetInt32("UserId"); // Sessiondan Aktif user'ın ID bilgisi alınıyor
            var companyId = HttpContext.Session.GetInt32("CompanyId"); // Sessiondan Aktif user'ın CompanyID bilgisi alınıyor


            var urlExpenceList = $"{_baseUrl}Expence/ListLast?CompanyID={companyId}&takeNumber=4";
            var urlSalaryAdvanceList = $"{_baseUrl}SalaryAdvance/List?userId={userId}";
            var urlLeaveRecordList = $"{_baseUrl}LeaveRecord/ListByCompany?companyId={companyId}&takeNumber=4";
            var urlPersonelList = $"{_baseUrl}CompanyManager/ListPersonels?companyId={companyId}&takeNumber=4";

            try
            {
                // CustomHttpClientHelper kullanarak işlemleri daha da kısattık ve değişiklik durumunda uygulamak tek bir yerden olacak
                var expenses = await CustomHttpClientHelper.GetAsync<IEnumerable<ListExpence_VM>>(_httpClientFactory, token, urlExpenceList);

                var salaryAdvances = await CustomHttpClientHelper.GetAsync<IEnumerable<ListSalaryAdvance_VM>>(_httpClientFactory, token, urlSalaryAdvanceList);

                var leaveRecords = await CustomHttpClientHelper.GetAsync<IEnumerable<ListLeaveRecord_DTO>>(_httpClientFactory, token, urlLeaveRecordList);

                var personels = await CustomHttpClientHelper.GetAsync<IEnumerable<ListPersonel_VM>>(_httpClientFactory, token, urlPersonelList);

                Console.WriteLine("\n");
                Console.WriteLine("Expence List");
                foreach (var item in expenses)
                {
                    Console.WriteLine(item.ExpenceType);
                }

                Console.WriteLine("\n");
                Console.WriteLine("Salary Advance List");
                foreach (var item in salaryAdvances)
                {
                    Console.WriteLine(item.Amount);
                    Console.WriteLine(item.AppUser);
                }

                Console.WriteLine("\n");
                Console.WriteLine("Leave Record List");
                foreach (var item in leaveRecords)
                {
                    Console.WriteLine(item.LeaveRecordType);
                    Console.WriteLine(item.ApprovalStatus);
                }

                Console.WriteLine("\n");
                Console.WriteLine("Personel List");
                foreach (var item in personels)
                {
                    Console.WriteLine(item.FirstName);
                    Console.WriteLine(item.Department);
                }


                var dashboard_VM = new CompanyManagerDashboard_VM
                {
                    Expences = expenses,
                    SalaryAdvances = salaryAdvances,
                    LeaveRecords = leaveRecords,
                    Personels = personels
                };

                return View(dashboard_VM);
                //return Content("Manager deneme - qindex");
            }
            catch
            {
                ViewBag.Error = "Veriler alınırken bir hata oluştu.";
                return View(new CompanyManagerDashboard_VM());
            }
        }
    }
}
