using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.LeaveRecordType_DTOs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.LeaveRecord_VMs;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Controllers
{
    [Area("Personel")]
    public class PersonelLeaveRecordTypeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;

        public PersonelLeaveRecordTypeController(IHttpClientFactory httpClientFactory, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Mevcut izin türlerini listeler.
        /// API'den tüm izin türleri getirilir.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var types = await _httpClient.GetFromJsonAsync<IEnumerable<ListLeaveRecordType_DTO>>(
                "https://localhost:7071/api/LeaveRecordType/List");

            return View(types);
        }

        /// <summary>
        /// Yeni izin türü ekleme formunu döner.
        /// </summary>
        [HttpGet]
        public IActionResult AddLeaveRecordType() => View();

        /// <summary>
        /// Yeni bir izin türünü API aracılığıyla veri tabanına kaydeder.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddLeaveRecordType(AddLeaveRecordType_VM leaveRecordType)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            leaveRecordType.CreatedAt = DateTime.UtcNow;
            leaveRecordType.RecordStatus = RecordStatus.IsAdded;

            var content = new StringContent(JsonConvert.SerializeObject(leaveRecordType), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7071/api/LeaveRecordType/AddLeaveRecordType", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            ViewBag.Message = "Kayıt başarısız.";
            return View(leaveRecordType);
        }

        /// <summary>
        /// Belirli bir izin türünü pasifleştirir (Soft Delete).
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> SoftDeleteLeaveRecordType(int id)
        {
            var token = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsync($"https://localhost:7071/api/LeaveRecordType/SoftDeleteLeaveRecordType/{id}", null);

            if (response.IsSuccessStatusCode)
                TempData["SuccessMessage"] = "Kayıt pasifleştirildi.";
            else
                TempData["ErrorMessage"] = "Kayıt pasifleştirilemedi.";

            return RedirectToAction("Index");
        }
    }
}
