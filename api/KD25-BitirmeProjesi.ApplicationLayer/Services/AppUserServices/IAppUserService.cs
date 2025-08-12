using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.AppUser_DTOs;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.AppUserServices
{
    public interface IAppUserService
    {
        Task<IEnumerable<AppUserList_DTO>> GetAllPersonalAsync(string companyId); //list
        Task<IEnumerable<AppUserList_DTO>> GetManagersForAdminAsync(); //list (manager)
        Task<bool> UpdatePersonalAsync(int id, AppUserUpdate_DTO updatePersonal); //update
        Task<bool> PassivePersonalAsync(int id); //passive
        Task AddPersonalAsync(AppUserAdd_DTO addPersonal); //add
        Task<AppUser_DTO> GetPersonalDetailsAsync(int id); //detail


        Task<AppUser_DTO> GetUserSummaryAsync(int userId);
    }
}
