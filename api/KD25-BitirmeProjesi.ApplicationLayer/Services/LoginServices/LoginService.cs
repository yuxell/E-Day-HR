using KD25_BitirmeProjesi.CoreLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Login_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.EmailServices;
using Microsoft.AspNetCore.Http;
using static System.Collections.Specialized.BitVector32;
using System.Net;
using System.Security.Principal;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.LoginServices
{
    public class LoginService: ILoginService
    {
        
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public LoginService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config, IEmailService emailService, IHttpContextAccessor httpContextAccessor) // Değişiklik burada
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<LoginResponse_DTO> LoginAsync(Login_DTO dto)
        {
            //cookie tabanlı kimlik doğrulama(Identity +Session)
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new Exception("Invalid credentials");

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("UserId", user.Id.ToString()),
                new Claim("CompanyId", user.CompanyID?.ToString() ?? "0"), // yoksa sıfır verdim, kontrollerde kullanabilmek için
                new Claim("FullName", user.FirstName + " " + user.Surname),
            };

            // Burada role Claim'ini farklı alıyorum yoksa görmüyor
            //claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            claims.AddRange(roles.Select(role => new Claim("Role", role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:secretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:issuer"],        // "kolayik-api"
                audience: _config["JwtSettings:audience"],    // "kolayik-client"
                expires: dto.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1),
                claims: claims,
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // ************* Burada Session set etmeye gerek yok kaldırabiliriz SetString işlemlerini, session kaydetme UI tarafından gerçekleşiyor.
            // Session'a token kaydetme
            //_httpContext.Session.SetString("JWT_Token", tokenString);
            //_httpContext.Session.SetString("JWT_Token", new JwtSecurityTokenHandler().WriteToken(token));

            //_httpContextAccessor.HttpContext?.Session.SetString("Token", tokenString);
            //_httpContextAccessor.HttpContext?.Session.SetString("UserId", user.Id.ToString());
            //_httpContextAccessor.HttpContext?.Session.SetString("CompanyId", user.CompanyID?.ToString() ?? "");
            // ************* Burada Session set etmeye gerek yok kaldırabiliriz SetString işlemlerini


            return new LoginResponse_DTO
            {
                Token = tokenString,
                RoleName = roles.FirstOrDefault(),
                UserId = user.Id,
                CompanyId = user.CompanyID,
                FullName = user.FirstName + " " + user.Surname
            };
        }


        public async Task ForgotPasswordAsync(ForgotPassword_DTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception($"User not found for email: {dto.Email}");

            // 🧪 Test için e-posta onayını elle aktif et (sadece geliştirme aşamasında!)
            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetLink = $"{dto.ClientAppUrl}/reset-password?token={Uri.EscapeDataString(token)}&email={user.Email}";

            await _emailService.SendEmailAsync(user.Email, "Şifre Sıfırlama", $"<a href='{resetLink}'>Şifrenizi sıfırlamak için tıklayın</a>");
        }


        public async Task UpdatePasswordAsync(UpdatePassword_DTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) throw new Exception("User not found");

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
            if (!result.Succeeded)
                throw new Exception("Şifre sıfırlama başarısız.");

            throw new Exception("Şifreniz başarıyla sıfırlandı.");
        }

    }
}
