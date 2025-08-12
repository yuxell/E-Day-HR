using AutoMapper;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.AppUser_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.AppUserServices;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.CoreLayer.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace KD25_BitirmeProjesi.WebAPI.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    
    //[Authorize(Roles = "Personel,Admin,CompanyManager")]

    public class SiteAdministratorController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public SiteAdministratorController(IAppUserService appUserService, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager, IMapper mapper)
        {
            _appUserService = appUserService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpGet("summary/{userId}")]
        public async Task<IActionResult> GetUserSummary(int userId)
        {
          
            // JWT claimlerinden kullanıcı ID'sini al
            //var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            //if (userIdClaim == null) return Unauthorized();

             //userId = int.Parse(userIdClaim.Value);

            var userSummary = await _userManager.FindByIdAsync(userId.ToString());
            if (userSummary == null) return NotFound("Kullanıcı bulunamadı.");

            return Ok(userSummary);
        }

        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetUserDetail(int id)
        {
            var user = await _userManager.Users.Include(x => x.Department).Include(x => x.Company)
                                               .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return NotFound();

            var dto = _mapper.Map<AppUserDetailDto>(user);

            return Ok(dto);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(AppUserDetailDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserID.ToString());

            if (user == null)
                return NotFound();

            // Değerleri güncelle
            user.FirstName = dto.FirstName;
            user.SecondName = dto.SecondName;
            user.Surname = dto.Surname;
            user.SecondSurname = dto.SecondSurname;
            user.BirthDate = dto.BirthDate;
            user.BirthPlace = dto.BirthPlace;
            user.NationalID = dto.NationalID;
            user.StartDate = dto.StartDate;
            user.EndDate = dto.EndDate;
            user.Proficiency = dto.Proficiency;
            user.Address = dto.Address;
            user.Salary = dto.Salary;
            user.CurrencyType = Enum.TryParse<CurrencyType>(dto.CurrencyType, out var parsed) ? parsed : CurrencyType.TurkLirası;
            user.PhoneNumber = dto.PhoneNumber;
            user.Email = dto.Email;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return StatusCode(500, "Kullanıcı güncellenirken bir hata oluştu.");
            }

            return Ok("Kullanıcı başarıyla güncellendi.");
        }




    }

}

