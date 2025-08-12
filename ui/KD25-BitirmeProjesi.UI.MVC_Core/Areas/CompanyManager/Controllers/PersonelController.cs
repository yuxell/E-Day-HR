using System.Net.Http.Headers;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.CompanyManager.Models.ViewModels;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.CompanyManager.Models.DTOs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.CompanyManager.Models.ViewModels;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.CompanyManager.Controllers
{
    [Area("CompanyManager")]
    //[Route("Personel")]
    public class PersonelController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration; // API end pointini alabilmek için
        private readonly string _baseUrl; // API end pointini online'a geçince tek bir yerden değiştirmek için

        public PersonelController(IHttpClientFactory httpClientFactory, HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _baseUrl = _configuration["ApiSettings:BaseUrl"]; // <<< constructor'da bir kere okur
        }
        public IActionResult Index()
        {
            return View(); 
        }

        [HttpGet]
        public IActionResult AddPersonel()
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" });
            }
            //var model = new AddPersonelForm_VM
            //{
            //    Personel = new AddPersonel_VM(), // NULL HATASI BURADA ENGELLENİYOR

            //    Companies = new SelectList(GetCompanies(), "ID", "Name"),
            //    Departments = new SelectList(GetDepartments(), "ID", "Name"),
            //    Proficiencies = new SelectList(GetProficiencies(), "Value", "Text"),
            //    CurrencyTypes = new SelectList(GetCurrencyTypes(), "ID", "Name")
            //};

            return View(/*model*/);
        }

        //private IEnumerable GetCurrencyTypes()
        //{
        //    throw new NotImplementedException();  // Enum bu
        //}

        //private IEnumerable GetProficiencies()
        //{
        //    throw new NotImplementedException();  // backend tarafına istek atıp alınacak listesi dönecek
        //}

        //private IEnumerable GetDepartments()
        //{
        //    throw new NotImplementedException();
        //}

        //private IEnumerable GetCompanies()
        //{
        //    throw new NotImplementedException();
        //}

        [HttpPost]
        public async Task<IActionResult> AddPersonel(AddPersonel_VM addPersonel)
        {

            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" });
            }

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            if (!ModelState.IsValid)
            {
                return View(addPersonel);
            }

            //var client = _httpClientFactory.CreateClient();

            // Bearer token header ekleniyor
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // POST isteği gönderiliyor
            //var client = _httpClientFactory.CreateClient();

            //// Bearer token header ekleniyor
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //// POST isteği gönderiliyor
            //var response = await client.PostAsJsonAsync($"{_baseUrl}Personel/add", addPersonel);

            //if (response.IsSuccessStatusCode)
            //{
            //    return RedirectToAction("Index");
            //}

            //ModelState.AddModelError(string.Empty, "Kayıt sırasında bir hata oluştu.");
            //return View(addPersonel);

            // Fotoğraf kaydetme işlemi
            string avatarPath = null;
            if (addPersonel.Avatar != null)
            {
                avatarPath = await Utilities.FileUtility.SaveFileAsync(addPersonel.Avatar);
            }

            // ViewModel'i DTO'ya dönüştürme
            var personelDTO = new AddPersonel_DTO
            {
                FirstName = addPersonel.FirstName,
                SecondName = addPersonel.SecondName,
                Surname = addPersonel.Surname,
                SecondSurname = addPersonel.SecondSurname,
                Avatar = avatarPath, // Kaydedilen fotoğrafın yolu
                BirthDate = addPersonel.BirthDate,
                BirthPlace = addPersonel.BirthPlace,
                NationalID = addPersonel.NationalID,
                StartDate = addPersonel.StartDate,
                EndDate = addPersonel.EndDate,
                CompanyID = addPersonel.CompanyID,
                Proficiency = addPersonel.Proficiency,
                DepartmentID = addPersonel.DepartmentID,
                Email = addPersonel.Email,
                Address = addPersonel.Address,
                PhoneNumber = addPersonel.PhoneNumber,
                Salary = addPersonel.Salary,
                CurrencyTypeID = addPersonel.CurrencyTypeID,
                UserName = addPersonel.UserName,
                Password = addPersonel.Password,
                PasswordConfirmed = addPersonel.PasswordConfirmed
            };

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // API'ye gönderim işlemi
            var response = await client.PostAsJsonAsync($"{_baseUrl}Personel", addPersonel);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Manager"); // Başarılı işlem sonrası yönlendirme
            }

            ModelState.AddModelError(string.Empty, "Kayıt sırasında bir hata oluştu.");
            return View(addPersonel);
        }

    }
}
        

