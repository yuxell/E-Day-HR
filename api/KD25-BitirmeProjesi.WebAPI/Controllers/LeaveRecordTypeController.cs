using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.LeaveRecordType_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.LeaveRecordTypeServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LeaveRecordTypeController : ControllerBase
    {

        private readonly ILeaveRecordTypeService _leaveRecordTypeService;

        public LeaveRecordTypeController(ILeaveRecordTypeService leaveRecordTypeService)

        {
            _leaveRecordTypeService = leaveRecordTypeService;
        }

        // Add LeaveRecordType (POST)
        [Authorize(Roles = "Admin")]
        [HttpPost("AddLeaveRecordType")]

        public async Task<IActionResult> AddLeaveRecordType([FromBody] AddLeaveRecordType_DTO addLeaveRecordType)
        {
            try
            {
                // Yeni izin türü ekleme
                await _leaveRecordTypeService.AddLeaveRecordTypeAsync(addLeaveRecordType);

                return Ok("Leave Record Type başarıyla oluşturuldu.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu! : " + ex.Message);
            }
        }

        // List LeaveRecordTypes (GET)
        [Authorize(Roles = "Admin")]
        [HttpGet("List")]
        public async Task<IActionResult> GetLeaveRecordTypes()
        {
            try
            {
                var leaveRecordTypes = await _leaveRecordTypeService.GetAllLeaveRecordTypesAsync();
                return Ok(leaveRecordTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu! : " + ex.Message);
            }
        }

        // Update LeaveRecordType (PUT)
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateLeaveRecordType/{id}")]
        public async Task<IActionResult> UpdateLeaveRecordType(int id, [FromBody] UpdateLeaveRecordType_DTO updateLeaveRecordType)
        {
            try
            {
                var result = await _leaveRecordTypeService.UpdateLeaveRecordTypeAsync(id, updateLeaveRecordType);

                if (result)
                    return Ok("Leave Record Type başarıyla güncellendi.");

                return NotFound("Leave Record Type bulunamadı veya zaten silinmiş.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu! : " + ex.Message);
            }
        }

        // Soft Delete LeaveRecordType (DELETE)
        [Authorize(Roles = "Admin")]
        [HttpDelete("SoftDeleteLeaveRecordType/{id}")]
        public async Task<IActionResult> SoftDeleteLeaveRecordType(int id)
        {
            try
            {
                var result = await _leaveRecordTypeService.SoftDeleteLeaveRecordTypeAsync(id);

                if (result)
                    return Ok("Leave Record Type başarıyla pasif hale getirildi.");

                return NotFound("Leave Record Type bulunamadı veya zaten silinmiş.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu! : " + ex.Message);
            }
        }
    }
}





