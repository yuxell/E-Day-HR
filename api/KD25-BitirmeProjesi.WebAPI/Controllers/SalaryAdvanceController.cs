using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Advance_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.SalaryAdvanceServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.WebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SalaryAdvanceController : Controller
	{
        
        [ApiExplorerSettings(IgnoreApi = true)]

		[HttpGet]
        public IActionResult Index()
		{
			return View();
		}

		private readonly ISalaryAdvanceService _salaryAdvanceService;

		public SalaryAdvanceController(ISalaryAdvanceService salaryAdvanceService)
		{
			_salaryAdvanceService = salaryAdvanceService;
		}

		//[Authorize(Roles = "Personel")]
		[HttpPost("AddSalaryAdvance")]
		public async Task<IActionResult> AddSalaryAdvance([FromBody] AddSalaryAdvance_DTO AddSalaryAdvance)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			try
			{
				await _salaryAdvanceService.AddSalaryAdvanceAsync(AddSalaryAdvance);
				return Ok("Avans talebi başarıyla oluşturuldu.");
			}
			catch (Exception ex)
			{
				return BadRequest($"Bir hata oluştu: {ex.Message}");
			}
		}

		//[Authorize(Roles = "Personel")]
		[HttpGet("List")]
		public async Task<IActionResult> GetAllSalaryAdvances([FromQuery] int userId)
		{
			var salaryAdvances = await _salaryAdvanceService.GetAllSalaryAdvanceAsync(userId);
			return Ok(salaryAdvances);
		}

		//[Authorize(Roles = "Personel")]
		[HttpGet("Details/{id}")]
		public async Task<IActionResult> GetSalaryAdvanceDetails(int id) 
		{
            Console.WriteLine("---------------------GetSalaryAdvanceDetails: " + id);
			var advance = await _salaryAdvanceService.GetSalaryAdvanceDetailsAsync(id);
			return Ok(advance);
		}

		//[Authorize(Roles = "Personel")]
		[HttpPut("Passive/{id}")]
		public async Task<IActionResult> PassiveSalaryAdvance(int id)
		{
			try
			{
				var result = await _salaryAdvanceService.PassiveSalaryAdvanceAsync(id);

				if (result)
				{
					return Ok("Avans talebi başarıyla iptal edildi.");
				}

				return BadRequest("Avans talebi iptal edilemedi.");
			}
			catch (KeyNotFoundException)
			{
				return NotFound("Avans talebi bulunamadı.");
			}
			catch (InvalidOperationException)
			{
				return BadRequest("Avans talebi onaylanmış ya da reddedilmiş, bu yüzden iptal edilemez.");
			}
			catch (Exception)
			{
				return StatusCode(500, "Sunucu hatası oluştu.");
			}
		}

        //[Authorize(Roles = "Personel")]
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateSalaryAdvance(int id, UpdateSalaryAdvance_DTO updateSalaryAdvance_DTO)
		{
            var result = await _salaryAdvanceService.UpdateSalaryAdvanceAsync (id, updateSalaryAdvance_DTO);
            if (result)
            {
                return Ok("Avans talebi başarıyla güncellendi.");
            }

            return BadRequest("Avans talebi güncellenemedi.");
        }

    }
}
