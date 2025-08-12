using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.ExpenceServices
{
    public interface IExpenceTypeService
    {
        // Expence türlerini Şirket yönetici yönetebilmesi şirkete özgün harcama türleri girileceği için mantıklıdır

        /// <summary>
        /// Verilen şirket id'ye ait harcama türlerini getirir
        /// </summary>
        /// <param name="companyId">Harcama türlerini içeren Şirketin id'si</param>
        /// <returns></returns>
        Task<IEnumerable<ListExpenceType_DTO>> GetAllExpencesTypeAsync(int companyId);
        /// <summary>
        /// Harcama türünün güncellenmesi, RecordStatus'te update olduğu bilgisi de değişir.
        /// </summary>
        /// <param name="id">Güncellenecek harcama türünün id'si</param>
        /// <param name="updateExpence">Güncellenmiş harcama türü</param>
        /// <returns></returns>
        Task<bool> UpdateExpenceTypeAsync(int id, UpdateExpenceType_DTO updateExpenceType);

        /// <summary>
        /// Harcama türünde soft silme yapar
        /// </summary>
        /// <param name="id">Soft silme yapılacak harcama türüne ait id</param>
        /// <returns></returns>
        Task<bool> SoftDeleteExpenceTypeAsync(int id);

        /// <summary>
        /// Harcama türü ekler ve eklenen harcama türünün Id bilgisini verir
        /// </summary>
        /// <param name="addSpending">Eklenecek harcama türü nesnesi</param>
        /// <returns></returns>
        Task<int> AddExpenceTypeAsync(AddExpenceType_DTO addExpenceType);

        /// <summary>
        /// Harcama türü detayı
        /// </summary>
        /// <param name="id">Detayı istenen Harcama türünün id'si</param>
        /// <returns></returns>
        Task<ExpenceType_DTO> GetExpenceTypeDetailsAsync(int id);
    }
}
