using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.Expence_DTOs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs;
using KD25_BitirmeProjesi.UI.MVC_Core.Helpers;
using KD25_BitirmeProjesi.UI.MVC_Core.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using static System.Net.WebRequestMethods;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Controllers
{
    [Area("Personel")]
    [Route("Personel/Expence/")]
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

        [HttpGet("")]
        public async Task<IActionResult> Index() // Expence'in index'inde de Expence listesi gösteriliyor
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }

            var userId = HttpContext.Session.GetInt32("UserId"); // Sessiondan Aktif user'ın ID bilgisi alınıyor
            var url = $"{_baseUrl}Expence/List?userId={userId}";

            try
            {
                // CustomHttpClientHelper kullanarak işlemleri daha da kısattık ve değişiklik durumunda uygulamak tek bir yerden olacak
                var expenses = await CustomHttpClientHelper.GetAsync<IEnumerable<ListExpence_VM>>(_httpClientFactory, token, url);
                return View(expenses);
            }
            catch
            {
                ViewBag.Error = "Veriler alınırken bir hata oluştu.";
                return View(new List<ListExpence_VM>());
            }
        }


        [HttpGet("AddExpence")] // Add Expence GET
        public async Task<IActionResult> AddExpence()
        {
            AddExpenceForm_VM expenceForm = new AddExpenceForm_VM();

            var token = HttpContext.Session.GetString("Token");
            var userId = HttpContext.Session.GetInt32("userId");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }
            
            var companyId = 1; // Tokendan Company ID alınacak
            var expenceTypes = await _httpClient.GetFromJsonAsync<IEnumerable<ExpenceType_VM>>($"https://localhost:7071/api/ExpenceType/List?companyId={companyId}");
            expenceForm.ExpenceTypes = new SelectList(expenceTypes, "ID", "ExpenceTypeName");

            expenceForm.CurrencyTypes = EnumHelper.GetSelectListFromEnum<CurrencyType>();
            Console.WriteLine("-----------EnumENUMENUM ");

            return View(expenceForm);
        }

        [HttpPost("AddExpence")] // Add Expence POST
        public async Task<IActionResult> AddExpence(AddExpenceForm_VM addExpence)
        {
            //if (!ModelState.IsValid)
            //{
            //    Console.WriteLine("_________________________________________________");
            //    Console.WriteLine("Valid Model değil");
            //    return View();
            //}

            // Oturumdan Token ve UserId alıyoruz
            /*var token = HttpContext.Session.GetString("JwtToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            var userId = HttpContext.Session.GetString("userId");

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "Oturum zaman aşımına uğradı veya kullanıcı kimliği bulunamadı.");
                return RedirectToAction("Index", "Login", new { area = "" });
            }*/


            try
            {
                // Dosya yüklendi mi kontrol ediliyor
                /*string filePath = null;
                if (addExpence.FilePath != null)
                {
                    Console.WriteLine("____________________________________ Dosya işlemi");
                    filePath = "folders/" + await Utilities.FileUtility.SaveFolderAsync(addExpence.FilePath);
                }
                */
                var token = HttpContext.Session.GetString("Token");
                var userId = HttpContext.Session.GetInt32("UserId");
                Console.WriteLine("----------------USERID = " + userId);

                if (string.IsNullOrEmpty(token))
                {
                    Console.WriteLine("LIST Action-Token -------------- : Hatalı------------------");
                    return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
                }

                // DTO oluşturuluyor
                var addExpenceDTO = new AddExpence_DTO
                {
                    AppUserID = (int)userId, // 1 yerine AppUserID olacak ve Token'dan gelecek
                    Currency = addExpence.Expence.Currency,//addExpence.CurrencyType,
                    ExpenceTypeID = addExpence.Expence.ExpenceTypeID,//addExpence.ExpenceTypeID,
                    Amount = addExpence.Expence.Amount,//addExpence.Amount,
                    FilePath = addExpence.Expence.FilePath.ToString(), //filePath,
                    Explanation = addExpence.Expence.Explanation// addExpence.Explanation


                };

                Console.WriteLine("userid from binding: " + userId);
                Console.WriteLine("currency from binding: " + addExpence.Expence.Currency);
                Console.WriteLine("amount: " + addExpence.Expence.Amount);
                Console.WriteLine("expence type id from binding: " + addExpence.Expence.ExpenceTypeID);
                Console.WriteLine("explanation from binding: " + addExpence.Expence.Explanation);
                Console.WriteLine("file path from binding: " + addExpence.Expence.FilePath.ToString());
                
                // API'ye istek için JWT ekleniyor
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // DTO'yu JSON formatında API'ye gönderiyoruz
                var json = JsonConvert.SerializeObject(addExpenceDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine("json verisi: " + json.ToString());

                // API'ye POST isteği gönderiliyor
                var response = await _httpClient.PostAsync("https://localhost:7071/api/Expence/AddExpence", content);
            

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Harcama talebi başarıyla oluşturuldu.";
                    return RedirectToAction("ListExpences", "Expence"); // Ana sayfaya yönlendirme
                }
                else
                {
                    Console.WriteLine("---------------bir hata oldu kayıt işleminde " + response.ReasonPhrase);
                    ViewBag.Message = "Bir hata oluştu: " + response.ReasonPhrase;
                    return View(); // Hata durumunda form geri döndürülüyor
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("___________________________________");
                Console.WriteLine("UI da bir hata oldu " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);

                ViewBag.Message = $"Bir hata oluştu UIIII: {ex.Message}";
                return RedirectToAction("ListExpences", "Expence");
            }
        }

        [HttpGet("ListExpences")]
        public async Task<IActionResult> ListExpences()
        {

            // Client oluşturmak için helperdan alıyor, buna ihtiyaç yok
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("LIST Action-Token -------------- : Hatalı------------------");
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }

            Console.WriteLine("LIST Action-Token -------------- : Token Alındı");
            Console.WriteLine("LIST Action-Token -------------- : Token " + token);
            var client = _httpClientFactory.CreateClient(); // CustomHttpClientHelper kullanılabilirq
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var userId = HttpContext.Session.GetInt32("UserId"); // Sessiondan Aktif user'ın ID bilgisi alınıyor
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine("\n");
            Console.WriteLine($"{_baseUrl}Expence/List?userId={userId.ToString()}");

            //var client = CustomHttpClientHelper.CreateClientWithToken(_httpClientFactory, _httpContextAccessor);
            //var userId = _httpContextAccessor.HttpContext.Session.GetInt32("userId");

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}Expence/List?userId={userId.ToString()}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            Console.WriteLine("LIST Action-----Authorization Header = Bearer " + token);

            var response = await client.SendAsync(request);

            Console.WriteLine("LIST Action-Token -------------- userId : " + userId);

            //var response = await client.GetAsync($"https://localhost:7071/api/Expence/List?userId={userId}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine("LIST Action-Token -------------- veriler alınamadı");

                // API'den hata dönerse burada loglama veya kullanıcıya bilgi gösterilebilir
                ViewBag.Error = "Veriler alınırken bir hata oluştu.";
                return View(new List<ListExpence_VM>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var expenses = JsonConvert.DeserializeObject<IEnumerable<ListExpence_VM>>(json);

            return View(expenses);



            // eski yapılan ve session olmadan çalışan
            /*var userId = 1; // token'dan alınacak
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<ListExpence_VM>>($"https://localhost:7071/api/Expence/List?userId={userId}");
            return View(response);
            */
        }
        [HttpPost("Cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            //Console.WriteLine("---------------------------");
            //// JWT token'ı alın
            //var jwtToken = HttpContext.Session.GetString("JWT_Token");

            //// HTTP istemcisi oluştur
            //var client = _httpClientFactory.CreateClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            //// API'ye PUT isteği gönder
            //var response = await client.PutAsync($"https://localhost:7071/api/Expence/Cancel/{id}", null);

            //if (response.IsSuccessStatusCode)
            //{
            //    return Json(new { success = true, message = "Harcama başarıyla silindi." });
            //}
            //else
            //{
            //    var errorMessage = await response.Content.ReadAsStringAsync();
            //    return Json(new { success = false, message = errorMessage });
            //}
            var response = await _httpClient.PutAsync($"https://localhost:7071/api/Expence/Cancel/{id}", null);
            return RedirectToAction("ListExpences", "Expence");
        }
    }
}
