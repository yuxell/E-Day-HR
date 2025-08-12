using System.Configuration;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using KD25_BitirmeProjesi.PresentationLayer.Models.VMs.AdvanceVMs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.SalaryAdvanceDTOs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.SalaryAdvance_ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Controllers
{
	[Area("Personel")]
    [Route("Personel/PersonelSalaryAdvance/")]
	public class PersonelSalaryAdvanceController : Controller
	{
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration; // API end pointini alabilmek için
        private readonly string _baseUrl; // API end pointini online'a geçince tek bir yerden değiştirmek için

        public PersonelSalaryAdvanceController(IHttpClientFactory httpClientFactory, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
		{
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            _baseUrl = _configuration["ApiSettings:BaseUrl"]; // <<< constructor'da bir kere okur
        }

        #region ListSalaryAdvance
        [HttpGet("ListSalaryAdvance")]
        public async Task<IActionResult> ListSalaryAdvance()
        {
            var token = HttpContext.Session.GetString("Token"); //token 

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" });
            }

            var client = _httpClientFactory.CreateClient();
            var userId = HttpContext.Session.GetInt32("UserId");

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}SalaryAdvance/List?userId={userId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Veriler alınırken bir hata oluştu.";
                return View(new List<ListSalaryAdvance_VM>()); // boş liste döner
            }

            var json = await response.Content.ReadAsStringAsync();
            var salaryAdvances = JsonConvert.DeserializeObject<IEnumerable<ListSalaryAdvance_VM>>(json);

            return View(salaryAdvances);
        }

        #endregion  

        #region GETAddSalaryAdvance
        // GET: AddSalaryAdvance
        [HttpGet("AddSalaryAdvance")]
		public IActionResult AddSalaryAdvance()
		{
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }
            return View();
		}

		#endregion

		#region POST AddSalaryAdvance

		[HttpPost("AddSalaryAdvance")]
		public async Task<IActionResult> AddSalaryAdvance(AddSalaryAdvance_VM addSalaryAdvance)
		{

            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }

            // ViewModel'i JSON formatına çeviriyoruz
            addSalaryAdvance.AppUserID = 1;

			var json = JsonConvert.SerializeObject(addSalaryAdvance);

			// JSON verisini içeren bir HTTP içeriği oluşturuyoruz
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			// API'ye POST isteği gönderiyoruz
			var response = await _httpClient.PostAsync("https://localhost:7071/api/SalaryAdvance/AddSalaryAdvance", content);

			// Eğer API başarılı dönerse kullanıcıyı AddSalaryAdvance sayfasına yönlendiriyoruz
			if (response.IsSuccessStatusCode)
			{
				TempData["SuccessMessage"] = "Avans talebi başarıyla gönderildi.";
				return RedirectToAction("ListSalaryAdvance");
			}

			// Eğer bir hata oluşursa, kullanıcıya hata mesajı gösteriyoruz
			TempData["ErrorMessage"] = "Hata oluştu: " + response.ReasonPhrase;
			return View(addSalaryAdvance);
		}

        #endregion

        #region SalaryAdvanceDetails
        [HttpGet("GetSalaryAdvanceDetails")]
        public async Task<IActionResult> GetSalaryAdvanceDetails(int id)

        {

            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))

            {

                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir

            }

            var userId = HttpContext.Session.GetInt32("UserId");

            var client = _httpClientFactory.CreateClient();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{_baseUrl}SalaryAdvance/Details/{id}");

            if (!response.IsSuccessStatusCode)

            {

                return View("Error");

            }

            var leaveRecord = await response.Content.ReadFromJsonAsync<SalaryAdvance_VM>();

            return View(leaveRecord);

        }



        #endregion

        #region SoftDeleteSalaryAdvance
        [HttpPost("SoftDeleteSalaryAdvance")]
        public async Task<IActionResult> SoftDeleteSalaryAdvance(int id)
        {
            // Token'ı al
            var token = HttpContext.Session.GetString("Token"); // küçük harf değil büyük T! 
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }

            var client = _httpClientFactory.CreateClient(); // Client'ı her zaman Factory'den oluştur
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // API'ye PUT isteği gönder 
            var response = await client.PutAsync($"{_baseUrl}SalaryAdvance/Passive/{id}", null);

            // API yanıtına göre mesaj ver
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Avans talebi başarıyla pasifleştirildi.";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                TempData["ErrorMessage"] = "Avans talebi onaylanmış ya da reddedilmiş, iptal edilemez.";
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["ErrorMessage"] = "Avans bulunamadı.";
            }
            else
            {
                TempData["ErrorMessage"] = "Bir hata oluştu. Avans talebi pasifleştirilemedi.";
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region GET UpdateSalaryAdvance
        [HttpGet("UpdateSalaryAdvance")]
		public async Task<IActionResult> UpdateSalaryAdvance( int id)
		{
            //// Kullanıcının oturumundaki JWT Token kontrolü
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }
            Console.WriteLine(id);
			//try
			//{
				// HTTP isteklerine authorization ekleniyor
			//httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				// API'den ViewModel (yani AddSalaryAdvance_VM) alınıyor
				var viewModel = await _httpClient.GetFromJsonAsync<SalaryAdvance_VM>($"https://localhost:7071/api/SalaryAdvance/Details/{id}");
                Console.WriteLine("---------------------------------");

			var salaryAdvance_VM = new UpdateSalaryAdvance_VM
			{
				Id = id,
				ApprovalStatus = viewModel.ApprovalStatus.ToString(),
				ResponseDate = viewModel.ResponseDate.ToString(),
                SalaryAdvanceType = viewModel.SalaryAdvanceType,
				Amount = viewModel.Amount,
				AppUserID = viewModel.AppUserID,
				CurrencyType = viewModel.CurrencyType,
				Explanation = viewModel.Explanation
				
            };

			return View(salaryAdvance_VM);

                //// Eğer veri bulunamadıysa kullanıcıya bilgi ver
                //if (viewModel == null)
                //{
                //	TempData["Error"] = "Maaş avansı bilgisi bulunamadı.";
                //	return RedirectToAction("Index", "SalaryAdvance");
                //}

                // Form sayfasına ViewModel gönder
			//}
			//catch (Exception)
			//{
			//	// Hata durumunda loglama yapılabilir (isteğe bağlı)
			//	return View("Error");
			//}
		}
		#endregion


		#region POST UpdateSalaryAdvance
		[HttpPost("UpdateSalaryAdvance")]
		public async Task<IActionResult> UpdateSalaryAdvance(int id, UpdateSalaryAdvance_VM updateSalaryAdvance_VM)
		{
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }
            //// Oturumdan JWT token ve kullanıcı ID'sini alıyoruz
            //var token = HttpContext.Session.GetString("JwtToken");
            //var userId = HttpContext.Session.GetString("userId");

            //// Eğer token ya da userId eksikse, kullanıcıyı login sayfasına yönlendiriyoruz
            //if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
            //{
            //	ModelState.AddModelError(string.Empty, "Oturum süresi dolmuş olabilir.");
            //	return RedirectToAction("Index", "Login", new { area = "" });
            //}

            //         // Güncellenen SalaryAdvance için AppUserID'yi modelin içine atıyoruz
            //         /*updateSalaryAdvance_DTO = int.Parse(userId);*/ // ----------------------------------------------------------------------------userid gibi proplari tanımla

            //// HTTP isteği için yetkilendirme başlığını ayarlıyoruz
            //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // ViewModel'i JSON formatına çeviriyoruz
            var json = JsonConvert.SerializeObject(updateSalaryAdvance_VM);

			// JSON verisini içeren bir HTTP içeriği oluşturuyoruz
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			// API'ye PUT isteği gönderiyoruz
			var response = await _httpClient.PutAsync($"https://localhost:7071/api/SalaryAdvance/Update/{id}", content);

			// Eğer API başarılı dönerse kullanıcıyı UpdateSalaryAdvance sayfasına yönlendiriyoruz
			if (response.IsSuccessStatusCode)
			{
				TempData["SuccessMessage"] = "Avans talebi başarıyla güncellendi.";
                return RedirectToAction("Index", "SalaryAdvance");

            }

            // Eğer bir hata oluşursa, kullanıcıya hata mesajı gösteriyoruz
            TempData["ErrorMessage"] = "Hata oluştu: " + response.ReasonPhrase;
			return View(updateSalaryAdvance_VM);
		}
		#endregion
	}
}


