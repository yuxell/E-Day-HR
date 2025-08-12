using System.Net.Http.Headers;
using System.Text;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.LeaveRecord_DTOs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.LeaveRecord_VMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Controllers
{
    [Area("Personel")]
    [Route("Personel/PersonelLeaveRecord/")]
    public class PersonelLeaveRecordController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration; // API end pointini alabilmek için
        private readonly string _baseUrl; // API end pointini online'a geçince tek bir yerden değiştirmek için

        public PersonelLeaveRecordController(IHttpClientFactory httpClientFactory, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            _baseUrl = _configuration["ApiSettings:BaseUrl"]; // <<< constructor'da bir kere okur
        }

        //list
        [HttpGet("ListLeaveRecord")]
        public async Task<IActionResult> ListLeaveRecord()
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }

            var client = _httpClientFactory.CreateClient();
            var userId = HttpContext.Session.GetInt32("UserId"); // Sessiondan Aktif user'ın ID bilgisi alınıyor

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}LeaveRecord/List?userId={userId.ToString()}"); 
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Veriler alınırken bir hata oluştu.";
                return View(new List<ListLeaveRecord_VM>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var records = JsonConvert.DeserializeObject<IEnumerable<ListLeaveRecord_VM>>(json);

            return View(records);
        }

        [HttpGet("AddLeaveRecord")]
        public IActionResult AddLeaveRecord()
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }

            return View();
        }

        [HttpPost("AddLeaveRecord")]
        public async Task<IActionResult> AddLeaveRecord(AddLeaveRecord_VM leaveRecord)
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }

            var client = _httpClientFactory.CreateClient();
            var userId = HttpContext.Session.GetInt32("UserId"); // Sessiondan Aktif user'ın ID bilgisi alınıyor

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}LeaveRecord/Add?userId={userId.ToString()}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //var token = HttpContext.Session.GetString("JwtToken");
            //var userIdStr = HttpContext.Session.GetString("userId");

            //if (string.IsNullOrEmpty(token) || !int.TryParse(userIdStr, out int userId))
            //{
            //    ModelState.AddModelError(string.Empty, "Oturum süresi doldu.");
            //    return RedirectToAction("Index", "Login", new { area = "" });
            //}

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            //leaveRecord.AppUserID = userId;

            var content = new StringContent(JsonConvert.SerializeObject(leaveRecord), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://localhost:7071/api/LeaveRecord/AddLeaveRecord", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("ListLeaveRecord", "PersonelLeaveRecord");
            else
            {
                ViewBag.Message = "Hata: " + response.ReasonPhrase;
                return View(leaveRecord);
            }
        }

        //public async Task<IActionResult> GetLeaveRecordDetails(int id)
        //{
        //    var token = HttpContext.Session.GetString("Token");

        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
        //    }

        //    var client = _httpClientFactory.CreateClient();
        //    var userId = HttpContext.Session.GetInt32("UserId"); // Sessiondan Aktif user'ın ID bilgisi alınıyor

        //    var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}LeaveRecord/Details?userId={userId.ToString()}");
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    //var token = HttpContext.Session.GetString("JwtToken");
        //    //if (!string.IsNullOrEmpty(token))
        //    //{
        //    //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    //}

        //    var response = await _httpClient.GetFromJsonAsync<ListLeaveRecord_DTO>(
        //        $"https://localhost:7071/api/LeaveRecord/Details/{id}");

        //    return response == null ? View("Error") : View(response);
        //}
        [HttpGet("GetLeaveRecordDetails")]
        public async Task<IActionResult> GetLeaveRecordDetails(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{_baseUrl}LeaveRecord/Details/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return View("Error");
            }

            var leaveRecord = await response.Content.ReadFromJsonAsync<LeaveRecordDetail_VM>();

            return View(leaveRecord);
        }


        //[HttpGet]
        //public async Task<IActionResult> UpdateLeaveRecord(int id)
        //{
        //    var token = HttpContext.Session.GetString("Token");

        //    if (string.IsNullOrEmpty(token))
        //    {
        //        return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
        //    }

        //    var client = _httpClientFactory.CreateClient();
        //    var userId = HttpContext.Session.GetInt32("UserId"); // Sessiondan Aktif user'ın ID bilgisi alınıyor

        //    var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}LeaveRecord/Update?userId={userId}");
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    //// 1) API’den mevcut veriyi al
        //    //var token = HttpContext.Session.GetString("JwtToken");
        //    //if (!string.IsNullOrEmpty(token))
        //    //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    var dto = await _httpClient.GetFromJsonAsync<ListLeaveRecord_DTO>(
        //        $"https://localhost:7071/api/LeaveRecord/Details/{id}");

        //    if (dto is null) return View("Error");

        //    // 2) ViewModel’i hazırla
        //    var vm = new UpdateLeaveRecordForm_VM
        //    {
        //        LeaveRecord = new UpdateLeaveRecord_VM
        //        {
        //            ID = dto.ID,
        //            LeaveRecordType = dto.LeaveRecordType,
        //            StartDate = dto.StartDate,
        //            EndDate = dto.EndDate,
        //            TotalDays = dto.TotalDays,
        //            AppUser = dto.AppUser,
        //            ApprovalStatus = dto.ApprovalStatus,
        //            RecordStatus = dto.RecordStatus
        //        },
        //        LeaveRecordTypes = new SelectList(
        //            Enum.GetValues(typeof(RecordStatus))
        //                .Cast<RecordStatus>()
        //                .Select(x => new { Id = (int)x, Name = x.ToString() }),
        //            "Id", "Name", dto.RecordStatus)
        //    };

        //    return View(vm);
        //}

        [HttpGet("UpdateLeaveRecord")]
        public async Task<IActionResult> UpdateLeaveRecord(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" });
            }

            var client = _httpClientFactory.CreateClient();

            var vm = await client.GetFromJsonAsync<UpdateLeaveRecord_VM>(
                $"https://localhost:7071/api/LeaveRecord/Details/{id}");

            if (vm is null)
                return View("Error");

            return View(vm);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> UpdateLeaveRecord(int id, UpdateLeaveRecordForm_VM form)
        //{
        //    if (!ModelState.IsValid)                 // temel doğrulama
        //    {
        //        // SelectList’i yeniden kur (post‑back’lerde kaybolur)
        //        form.LeaveRecordTypes = new SelectList(
        //            Enum.GetValues(typeof(RecordStatus))
        //                .Cast<RecordStatus>()
        //                .Select(x => new { Id = (int)x, Name = x.ToString() }),
        //            "Id", "Name", form.LeaveRecord.RecordStatus);

        //        return View(form);
        //    }

        //    //var token = HttpContext.Session.GetString("JwtToken");
        //    //if (string.IsNullOrEmpty(token))
        //    //{
        //    //    ModelState.AddModelError(string.Empty, "Oturum süresi doldu.");
        //    //    return RedirectToAction("Index", "Login", new { area = "" });
        //    //}

        //    //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //    var json = JsonConvert.SerializeObject(form.LeaveRecord);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    // 3) API’ye PUT veya PATCH
        //    var response = await _httpClient.PutAsync(
        //        $"https://localhost:7071/api/LeaveRecord/Update/{id}", content);

        //    if (response.IsSuccessStatusCode)
        //        return RedirectToAction(nameof(ListLeaveRecord));   // 4) başarı

        //    // başarısızlık: hata mesajını göster
        //    ViewBag.Message = "Hata: " + response.ReasonPhrase;
        //    // SelectList’i koru
        //    form.LeaveRecordTypes ??= new SelectList(
        //        Enum.GetValues(typeof(RecordStatus))
        //            .Cast<RecordStatus>()
        //            .Select(x => new { Id = (int)x, Name = x.ToString() }),
        //        "Id", "Name", form.LeaveRecord.RecordStatus);

        //    return View(form);
        //}

        [HttpPost("UpdateLeaveRecord")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateLeaveRecord(int id, UpdateLeaveRecord_VM model)
        {
            if (!ModelState.IsValid)
            {
                // Model geçersizse, formu yeniden göster
                return View(model);
            }

            //var token = HttpContext.Session.GetString("JwtToken");
            //if (string.IsNullOrEmpty(token))
            //{
            //    ModelState.AddModelError(string.Empty, "Oturum süresi doldu.");
            //    return RedirectToAction("Index", "Login", new { area = "" });
            //}

            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(
                $"https://localhost:7071/api/LeaveRecord/Update/{id}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(ListLeaveRecord));

            ViewBag.Message = "Hata: " + response.ReasonPhrase;
            return View(model);
        }

        [HttpPost("SoftDeleteLeaveRecord")]
        public async Task<IActionResult> SoftDeleteLeaveRecord(int id)
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }

            var userId = HttpContext.Session.GetInt32("UserId");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PutAsync($"https://localhost:7071/api/LeaveRecord/Delete/{id}", null);
            return RedirectToAction("ListLeaveRecord", "LeaveRecord");
        }
    }
}

