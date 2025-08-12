using AutoMapper;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.Expence_DTOs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.Enums;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.SiteAdministrator.Models.DTOs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.SiteAdministrator.Models.ViewModels.Company_VMs;
using KD25_BitirmeProjesi.UI.MVC_Core.Controllers;
using KD25_BitirmeProjesi.UI.MVC_Core.Filters;
using KD25_BitirmeProjesi.UI.MVC_Core.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.SiteAdministrator.Controllers
{
    [Area("SiteAdministrator")]
    [Route("Admin/Company/")]
    [SessionAuthorizeAttribute]
    //[Authorize(Roles = "Admin")]
    public class CompanyController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public CompanyController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IHttpClientFactory httpClientFactory, HttpClient httpClient, IWebHostEnvironment env, IMapper mapper) : base(httpClientFactory)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
            _baseUrl = _configuration["ApiSettings:BaseUrl"]; // API Base URL
            _env = env;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpGet("Create")] // Create Company GET
        public async Task<IActionResult> CreateCompany()
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" });
            }

            return View();
        }

        [HttpPost("Create")] // Create Company POST
        public async Task<IActionResult> CreateCompany(CreateCompany_VM model)
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" });
            }

            if (!ModelState.IsValid) return View(model);
            try 
            { 
            string logoPath = null;

            if (model.LogoFile != null && model.LogoFile.Length > 0)
            {
                string wwwrootPath = _env.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.LogoFile.FileName);
                string fullPath = Path.Combine(wwwrootPath, "uploads", fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.LogoFile.CopyToAsync(stream);
                }

                logoPath = "/uploads/" + fileName;
            }

                var dto = new CreateCompany_DTO();
                _mapper.Map(model, dto);
                dto.Logo = logoPath;

                var json = JsonConvert.SerializeObject(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Console.WriteLine("json verisi: " + json.ToString());

                // API'ye POST isteği gönderiliyor
                var response = await _httpClient.PostAsync($"{_baseUrl}Company/CreateCompany", content);


                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Şirket başarıyla oluşturuldu.";
                    return RedirectToAction("Index", "Company"); // Ana sayfaya yönlendirme
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
                return RedirectToAction("Index", "Admin");
            }
        }

    }
}
