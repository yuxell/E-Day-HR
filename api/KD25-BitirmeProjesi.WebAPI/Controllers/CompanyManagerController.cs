using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.AppUser_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.CompanyManagerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyManagerController : ControllerBase
    {
        private readonly ICompanyManagerService _companyManagerService;

        public CompanyManagerController(ICompanyManagerService companyManagerService)
        {
            _companyManagerService = companyManagerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Company Manager API");
        }

        [Authorize(Roles = "Admin, CompanyManager, Personel")]
        [HttpGet("ListPersonels")]
        public async Task<IActionResult> ListPersonels([FromQuery] int companyId, int takeNumber)
        {
            var result = await _companyManagerService.ListAllPersonels(companyId, takeNumber);
            return Ok(result);
        }

        // Belirli bir yöneticinin detaylarını getirir
        //[Authorize(Roles = "Admin, CompanyManager Personel")]
        [HttpGet("ManagerDetails/{id}")]
        public async Task<IActionResult> GetManagerDetails(int id)
        {
            var manager = await _companyManagerService.GetCompanyManagerDetailsAsync(id);
            if (manager == null)
                return NotFound("Yönetici bulunamadı.");

            return Ok(manager);
        }

        // Yöneticiyi pasif hale getirir (soft delete)
        //[Authorize(Roles = "Admin, CompanyManager Personel")]
        [HttpPut("PassiveManager/{id}")]
        public async Task<IActionResult> PassiveManager(int id)
        {
            var result = await _companyManagerService.PassiveCompanyManagerAsync(id);
            if (!result)
                return NotFound("Yönetici bulunamadı veya zaten pasif.");

            return Ok("Yönetici pasif hale getirildi.");
        }

        // Yöneticiyi günceller
        //[Authorize(Roles = "Admin, CompanyManager Personel")]
        [HttpPut("UpdateManager/{id}")]
        public async Task<IActionResult> UpdateManager(int id, [FromBody] AppUserUpdate_DTO updateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _companyManagerService.UpdateCompanyManagerAsync(id, updateDto);
            if (!result)
                return NotFound("Güncelleme başarısız. Kullanıcı bulunamadı veya pasif.");

            return Ok("Yönetici bilgileri güncellendi.");
        }

        // Yeni yönetici ekler
        //[Authorize(Roles = "Admin, CompanyManager Personel")]
        [HttpPost("AddManager")]
        public async Task<IActionResult> AddManager([FromBody] AppUserAdd_DTO addDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _companyManagerService.AddCompanyManagerAsync(addDto);
                return Ok("Yönetici başarıyla eklendi.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Yönetici eklenemedi: {ex.Message}");
            }
        }
    }
    // 

    // Personel Ekleme (Companyde çalışan)
    // Personelleri Listeleme (Companyde çalışan)
    // Personel Silme


    // izin listele action (company için tüm istekler)
    // izin onay action 

    // harcama listele action (company için tüm istekler)
    // harcama onay action 

    // avans listele action (company için tüm istekler)
    // avans onay action 

    // Bilgilerimi göster action (Yönetici yani AppUserId)
    // Bilgilerimi Düzenle Action

    /*Adımlar:
        İnfrastructure > CompanyManagerRepo.cs
        Application > CompanyManagerServis klasörü > ICompanyManagerService.cs, CompanyManagerService.cs

    Yapılacak işlem için adımlar;
    -Gerekiyorsa DTO ekle
    -ICompanyManagerServis metod ismi
    -CompanyManagerServis metod içeriği
    -Gerekiyorsa ICompanyManagerRepo metod ismi
    -Gerekiyorsa CompanyManagerRepo metod içeriği
    -API Controller'da aksiyon yazılacak

     * */


}

