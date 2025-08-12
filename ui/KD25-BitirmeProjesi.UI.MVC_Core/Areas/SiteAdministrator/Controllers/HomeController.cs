using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.SiteAdministrator.Models.ViewModels;
using KD25_BitirmeProjesi.UI.MVC_Core.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.SiteAdministrator.Controllers
{
    [Area("SiteAdministrator")]
    [Route("Admin")]
    //[Authorize(Roles = "Personel,Admin,CompanyManager")]

    public class HomeController : BaseController
    {
        //private readonly IHttpClientFactory _httpClientFactory; // BaseController'dan geliyor - Kullanıcı ismini Template'e aktarmak için
        // Ctor'a ekle : base(httpClientFactory) 

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;

        public HomeController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _baseUrl = _configuration["ApiSettings:BaseUrl"]; // API Base URL
        }

        public async Task<IActionResult> Index()
        {
            // Session'dan token'ı al
            var token = HttpContext.Session.GetString("Token");

            // Token yoksa kullanıcıyı Login sayfasına yönlendir
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" });
            }

            // Kullanıcı bilgilerini almak için HTTP istemcisini oluştur
            var client = _httpClientFactory.CreateClient();
            var userId = HttpContext.Session.GetInt32("UserId"); // Aktif kullanıcının ID'si

            if (!userId.HasValue)
            {
                ViewBag.Error = "Kullanıcı bilgisi alınamadı.";
                return View("Error");
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}SiteAdministrator/summary/{userId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // API'ye istek gönder
            var response = await client.SendAsync(request);

            // API çağrısı başarısızsa hata mesajı göster
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Veriler alınırken bir hata oluştu.";
                return View(new AppUser_VM()); // Boş kullanıcı nesnesi dön
            }

            // API'den gelen veriyi JSON olarak oku
            var json = await response.Content.ReadAsStringAsync();

            // JSON verisini UserSummary_VM nesnesine dönüştür
            var user = JsonConvert.DeserializeObject<AppUser_VM>(json);

            // -Kullanıcı ismini 
            

            // Kullanıcıyı View'da göster
            return View(user);
        }

        [HttpGet("Detail")]
        public async Task<IActionResult> Detail()
        {
            var token = HttpContext.Session.GetString("Token");
            var userId = HttpContext.Session.GetInt32("UserId");

            if (string.IsNullOrEmpty(token) || userId == null)
                return RedirectToAction("Login", "Login", new { area = "" });

            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}SiteAdministrator/detail/{userId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Detay bilgisi alınamadı.";
                return View("Error");
            }

            var json = await response.Content.ReadAsStringAsync();
            var user = JsonConvert.DeserializeObject<AppUserDetail_VM>(json);

            return View(user);
        }

        [HttpPost("Detail")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Detail(AppUserDetail_VM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            // Model'i API'nin beklediği DTO'ya dönüştür
            // Model'i API'nin beklediği DTO'ya dönüştür
            var dto = new AppUserDetail_VM
            {
                UserID = model.UserID,
                Avatar = "/images/avatar.png",
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                Surname = model.Surname,
                SecondSurname = model.SecondSurname,
                BirthDate = model.BirthDate,
                BirthPlace = model.BirthPlace,
                NationalID = model.NationalID,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Proficiency = model.Proficiency,
                Address = model.Address,
                Salary = model.Salary,
                CurrencyType = model.CurrencyType.ToString(), // Enum string olarak gönderilmeli
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,

                // API'de Include edilen ilişkisel alanlar
           
            };

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsJsonAsync($"{_baseUrl}SiteAdministrator/update", dto);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ViewBag.Error = $"API Hatası: {errorContent}";
                return View(model);
            }

            TempData["Success"] = "Güncelleme başarılı!";
            return RedirectToAction("Detail");
        }




    }






}
