using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Company_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.CompanyServices;
using KD25_BitirmeProjesi.ApplicationLayer.Services.ExpenceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // Admin İşlemleri
        //Create Company
        //[Authorize(Roles = "Admin")]
        [HttpPost("CreateCompany")]
        public async Task<IActionResult> CreateCompanyAsync([FromBody] CreateCompany_DTO createCompany)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("_______________________________________ WebApi Create Company Modeli Hatalı");
                return BadRequest(ModelState);
            }

            try
            {
                Console.WriteLine("_______________________________________ Create Company deneniyor");
                await _companyService.CreateCompanyAsync(createCompany);
                return Ok("Şirket ekleme işlemi başarılı.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("_______________________________________ WebApi bir hata oldu, Company eklenemedi " + ex.InnerException);
                return BadRequest($"Bir hata oluştu: {ex.Message}");
            }
        }

    }
}
