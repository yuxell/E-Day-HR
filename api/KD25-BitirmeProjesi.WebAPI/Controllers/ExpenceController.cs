using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.ExpenceServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenceController : ControllerBase
    {
        private readonly IExpenceService _expenceService;

        public ExpenceController(IExpenceService expenceService)
        {
            _expenceService = expenceService;
        }

        //ADD Expence
        //[Authorize(Roles = "Admin, CompanyManager, Personel")]
        [HttpPost("AddExpence")]
        public async Task<IActionResult> AddExpenceAsync([FromBody] AddExpence_DTO addExpence)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("_______________________________________ WebApi Harcama talep Model hatalı");
                return BadRequest(ModelState);
            }

            try
            {
                Console.WriteLine("_______________________________________ WebApi Harcama talebi deneniyor");
                await _expenceService.AddExpenceAsync(addExpence);
                return Ok("Harcama talebi başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("_______________________________________ WebApi bir hata oldu, harcama eklenemedi " + ex.InnerException);
                return BadRequest($"Bir hata oluştu: {ex.Message}");
            }
        }


        //LIST Expences
        [Authorize(Roles = "Admin, CompanyManager, Personel")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpGet("List")]
        public async Task<IActionResult> GetAllExpencesAsync([FromQuery] int userId)
        {
            var expences = await _expenceService.GetAllExpencesAsync(userId);
            return Ok(expences);
        }

        [Authorize(Roles = "Admin, CompanyManager, Personel")]
        [HttpGet("ListLast")]
        public async Task<IActionResult> GetLastExpencesAsync([FromQuery] int companyId, int takeNumber)
        {
            Console.WriteLine("Listeleme Authorize ------------------- Expenceleri admin rolüne sahip olan listeleyebiliyor şu anda");
            var expences = await _expenceService.GetLastExpencesAsync(companyId, takeNumber);
            return Ok(expences);
        }

        //DETAILS Expence
        [Authorize(Roles = "Admin, CompanyManager, Personel")]
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetExpenceDetails(int id)
        {
            var expence = await _expenceService.GetExpenceDetailsAsync(id);
            return Ok(expence);
        }


        //CANCEL Expence
        //[Authorize(Roles = "Admin, CompanyManager, Personel")]
        [HttpPut("Cancel/{id}")]
        public async Task<IActionResult> CancelExpence(int id)
        {
            try
            {
                // PassiveExpenceAsync servisini çağırarak harcama talebini pasifleştir
                var result = await _expenceService.CancelExpenceByEmployeeAsync(id);

                if (result)
                {
                    return Ok("Harcama talebi başarıyla iptal edildi.");
                }

                return BadRequest("Harcama talebi iptal edilemedi.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Harcama talebi bulunamadı.");
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Harcama talebi onaylanmış ya da reddedilmiş, iptal edilemez.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu hatası oluştu.");
            }
        }




        // === Manager İşlemleri ===
        [Authorize(Roles = "Admin, CompanyManager, Personel")]
        [HttpPut("Deny/{id}")]
        public async Task<IActionResult> DenyExpence(int id)
        {
            try
            {
                var result = await _expenceService.DenyExpenceByManagerAsync(id);

                if (result)
                {
                    return Ok("Harcama talebi başarıyla reddedildi edildi.");
                }

                return BadRequest("Harcama talebi reddedilemedi.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Harcama talebi bulunamadı.");
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Harcama talebi zaten reddedilmiş tekrar reddedilemez.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu hatası oluştu.");
            }

        }

        [Authorize(Roles = "Admin, CompanyManager, Personel")]
        [HttpPut("Approve/{id}")]
        public async Task<IActionResult> ApproveExpence(int id)
        {
            try
            {
                var result = await _expenceService.ApproveExpenceByManagerAsync(id);

                if (result)
                {
                    return Ok("Harcama talebi başarıyla onaylandı edildi.");
                }

                return BadRequest("Harcama talebi onaylanmadı.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Harcama talebi bulunamadı.");
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Harcama talebi silinmiş, onaylanamaz");
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu hatası oluştu.");
            }

        }
    }
}
