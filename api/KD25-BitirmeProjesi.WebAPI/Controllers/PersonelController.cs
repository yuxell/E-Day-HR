using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.AppUser_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.AppUserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonelController : Controller
    {
        private readonly IAppUserService _userService;

        public PersonelController(IAppUserService userService)
        {
            _userService = userService;
        }

        //[Authorize(Roles = "Manager")]
        [HttpGet("list")]
        public async Task<IActionResult> GetAllPersonel(string companyId)
        {
            var personelList = await _userService.GetAllPersonalAsync(companyId);
            return Ok(personelList);
        }

        //[Authorize(Roles = "Manager")]
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetPersonalDetails(int id)
        {
            var personal = await _userService.GetPersonalDetailsAsync(id);
            if (personal == null)
                return NotFound(new { message = "Personel bulunamadı." });

            return Ok(personal);
        }

        //[Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> AddPersonal(AppUserAdd_DTO newPersonal)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.AddPersonalAsync(newPersonal);
            return Ok("Kullanıcı başarıyla eklendi.");
        }

        //[Authorize(Roles = "Manager")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdatePersonal(int id, AppUserUpdate_DTO updatePersonal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Geçersiz model doğrulaması
            }

            var result = await _userService.UpdatePersonalAsync(id, updatePersonal);
            if (!result)
                return NotFound("Güncellenecek personel bulunamadı veya pasif durumda.");

            return Ok("Personel başarıyla güncellendi.");
        }

        //[Authorize(Roles = "Manager")]
        [HttpDelete("passive/{id}")]
        public async Task<IActionResult> PassivePersonal(int id)
        {
            try
            {
                var result = await _userService.PassivePersonalAsync(id);
                if (!result)
                    return BadRequest("Personel pasif hale getirilemedi.");

                return Ok("Personel başarıyla pasif hale getirildi.");
            }
            catch (NotImplementedException)
            {
                return StatusCode(501, "Bu özellik henüz uygulanmamış.");
            }
        }
    }
}
