using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Login_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.LoginServices;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.WebAPI.Controllers
{
    /// <summary>
    /// Kullanıcı kimlik doğrulama işlemlerini yöneten API controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController] 
    public class LoginController : ControllerBase
    {
        
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login_DTO dto)
        {
            var result = await _loginService.LoginAsync(dto);
            return Ok(result);
        }

        //[HttpGet("check-session")]
        //public IActionResult CheckSession()
        //{
        //    var token = HttpContext.Session.GetString("JWT_Token");
        //    return Ok(new { sessionToken = token ?? "Session boş!" });
        //}
        [HttpGet("check-session")]
        public IActionResult CheckSession()
        {
            return Ok(new
            {
                Token = HttpContext.Session.GetString("Token") ?? "not_found",
                UserId = HttpContext.Session.GetString("UserId") ?? "not_found",
                CompanyId = HttpContext.Session.GetString("CompanyId") ?? "not_found"
            });
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword_DTO dto)
        {
            await _loginService.ForgotPasswordAsync(dto);
            return Ok("Şifre sıfırlama bağlantısı gönderildi.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassword_DTO dto)
        {
            await _loginService.UpdatePasswordAsync(dto);
            return Ok("Şifre başarıyla güncellendi.");
        }

    }
}

