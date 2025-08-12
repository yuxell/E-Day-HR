using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.ExpenceServices
{
    public interface IExpenceService
    {
        Task<IEnumerable<ListExpences_DTO>> GetAllExpencesAsync(int userId); // Verilen id'ye ait kullanıcının tüm harcamalarını getirir
        Task<IEnumerable<ListExpences_DTO>> GetLastExpencesAsync(int companyID, int takeNumber); // Verilen id'ye ait kullanıcının tüm harcamalarını getirir
        Task<bool> UpdateExpenceAsync(int id, UpdateExpence_DTO updateExpence); // ApprovalStatus içerisindeki onay durumu değiştirilecek
        Task<bool> CancelExpenceByEmployeeAsync(int id); // Personel harcamada onay/red olmadığı sürece iptal edebilecek
        Task AddExpenceAsync(AddExpence_DTO addExpence); // Personel başvuru yapabilecek
        Task<ListExpences_DTO> GetExpenceDetailsAsync(int id);

        // Manager için başka işlem gerekirse eklenecek
        Task<bool> ApproveExpenceByManagerAsync(int id);
        Task<bool> DenyExpenceByManagerAsync(int id);
    }
}
