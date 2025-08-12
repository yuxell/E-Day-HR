using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.AppUser_DTOs;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.CompanyManagerServices
{
        public interface ICompanyManagerService
        {

        // Yeni bir Menejer ekler
        public Task AddCompanyManagerAsync(AppUserAdd_DTO addPersonal);

        // Şirkete bağlı tüm personelleri listeler. Opsiyonel olarak kaç kayıt alınacağı belirtilir.
        public Task<IEnumerable<AppUserList_DTO>> ListAllPersonels(int companyID, int takeNumber = 0);

        // Yöneticinin detaylarını alacak metod
        public Task<AppUser_DTO> GetCompanyManagerDetailsAsync(int id);

        public Task<bool> PassiveCompanyManagerAsync(int id);

        // Yöneticinin bilgilerini güncelleyecek metod
        public Task<bool> UpdateCompanyManagerAsync(int id, AppUserUpdate_DTO updateDTO);

    }
    
}
