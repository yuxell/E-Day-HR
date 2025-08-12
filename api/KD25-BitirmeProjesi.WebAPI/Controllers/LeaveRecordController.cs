using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.LeaveRecord_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.LeaveRecordServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveRecordController : Controller
    {
        private readonly ILeaveRecordService _leaveRecordService;

        public LeaveRecordController(ILeaveRecordService leaveRecordService)
        {
            _leaveRecordService = leaveRecordService;
        }

        //Add 
        //[Authorize(Roles = "Personel")]
        [HttpPost("AddLeaveRecord")]
        public async Task<IActionResult> AddLeaveRecord([FromBody] AddLeaveRecord_DTO addLeaveRecord)
        {
            try
            {
                // İzin talebini ekliyor
                await _leaveRecordService.AddLeaveRecordAsync(addLeaveRecord);
                return Ok("İzin talebi başarıyla oluşturuldu.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Bir hata oluştu! : " + ex.Message);
            }
        }

        //List
        //[Authorize(Roles = "Personel")]
        [HttpGet("List")]
        public async Task<IActionResult> GetAllLeaveRecords( int userId)
        {
            var leaveRecords = await _leaveRecordService.GetAllLeaveRecordAsync(userId);
            return Ok(leaveRecords);
        }

        //Details
        //[Authorize(Roles = "Personel")]
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> GetLeaveRecordDetails(int id)
        {
            var leaveRecord = await _leaveRecordService.GetLeaveRecordDetailsAsync(id);
            return Ok(leaveRecord);
        }

        //Soft Delete
        //[Authorize(Roles = "Personel")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> SoftDeleteLeaveRecord(int id)
        {
            try
            {
                var result = await _leaveRecordService.SoftDeleteLeaveRecordAsync(id);
                if (result)
                    return Ok("İzin başarıyla silindi (pasif hale getirildi).");

                return BadRequest("Silme işlemi başarısız.");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Silinmek istenen izin kaydı bulunamadı.");
            }
        }




        // Manager İşlemleri
        //[Authorize(Roles = "Admin, CompanyManager, Personel")]
        [HttpGet("ListByCompany")]
        public async Task<IActionResult> GetAllLeaveRecordsByCompany([FromQuery] int companyId, int takeNumber)
        {
            var leaveRecords = await _leaveRecordService.GetAllLeaveRecordsByCompanyAsync(companyId, takeNumber);
            return Ok(leaveRecords);
        }
    }
}



