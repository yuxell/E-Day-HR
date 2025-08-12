using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Login_DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.LoginServices
{
    public interface ILoginService
    {
        //Task<string?> AuthenticateAsync(string username, string password);
        //Task<bool> ChangePasswordAsync(string userId, ChangePassword_DTO model);

        //Task<LoginResponse_DTO> LoginAsync(Login_DTO loginDto);
        Task<LoginResponse_DTO> LoginAsync(Login_DTO dto);
        Task ForgotPasswordAsync(ForgotPassword_DTO model);
        Task UpdatePasswordAsync(UpdatePassword_DTO model);

    }
}
