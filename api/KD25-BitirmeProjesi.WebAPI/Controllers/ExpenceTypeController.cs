using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.ExpenceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenceTypeController : ControllerBase
    {
        private readonly IExpenceTypeService _expenceTypeService;

        public ExpenceTypeController(IExpenceTypeService expenceTypeService)
        {
            _expenceTypeService = expenceTypeService;
        }

        //ADD Expence
        [Authorize(Roles = "Personel")]
        [HttpPost("AddExpenceType")]
        public async Task<IActionResult> AddExpenceTypeAsync([FromBody] AddExpenceType_DTO addExpenceType)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _expenceTypeService.AddExpenceTypeAsync(addExpenceType);
                return Ok("Harcama türü başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu: {ex.Message}");
            }
        }


        //LIST Expences
        //[Authorize(Roles = "Personel")]
        [HttpGet("List")]
        public async Task<IActionResult> GetAllExpenceTypesAsync([FromQuery] int companyId)
        {
            var expenceTypes = await _expenceTypeService.GetAllExpencesTypeAsync(companyId);
            return Ok(expenceTypes);
        }

        //DETAILS Expence
        [Authorize(Roles = "Personel")]
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetExpenceTypeDetails(int id)
        {
            var expenceType = await _expenceTypeService.GetExpenceTypeDetailsAsync(id);
            return Ok(expenceType);
        }


        //CANCEL Expence
        [Authorize(Roles = "Personel")]
        [HttpPut("Delete/{id}")]
        public async Task<IActionResult> CancelExpence(int id)
        {
            try
            {
                var result = await _expenceTypeService.SoftDeleteExpenceTypeAsync(id);

                if (result)
                {
                    return Ok("Harcama türü başarıyla silindi");
                }

                return BadRequest("Harcama türü silinemedi.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Harcama türü bulunamadı.");
            }
            catch (Exception)
            {
                return StatusCode(500, "Sunucu hatası oluştu.");
            }
        }
    }
}
