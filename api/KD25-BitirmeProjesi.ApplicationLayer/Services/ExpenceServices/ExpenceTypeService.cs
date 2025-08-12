using AutoMapper;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.CoreLayer.Enums;
using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;
using KD25_BitirmeProjesi.InfrastructureLayer.Repositories.Concrete;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.ExpenceServices
{
    public class ExpenceTypeService : IExpenceTypeService
    {
        private readonly IExpenceTypeRepository _expenceTypeRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager; // Belirli Company için işlem yapılabilir diye eklendi. İşlem yapan kullanıcının companyId bilgisini almak için

        public ExpenceTypeService(IExpenceTypeRepository expenceTypeRepository, IMapper mapper, UserManager<AppUser> userManager)
        {
            _expenceTypeRepository = expenceTypeRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<int> AddExpenceTypeAsync(AddExpenceType_DTO addExpenceType)
        {
            var expenceType = new ExpenceType();
            _mapper.Map(addExpenceType, expenceType);

            return await _expenceTypeRepository.CreateAsync(expenceType);
        }

        /// <summary>
        /// Bir şirkete ait ExpenceType'ları liste olarak verir
        /// </summary>
        /// <param name="companyId">Expence Türleri istenen şirket Id bilgisi</param>
        /// <returns></returns>
        public async Task<IEnumerable<ListExpenceType_DTO>> GetAllExpencesTypeAsync(int companyId)
        {
            // expenceType listelerken belirli bir şirkete ait olması için düzeltme yapılabilir. Eğer ExpenceType'lar bir şirkete ait olacak ise
            var expenceTypes = await _expenceTypeRepository.FilteredSearchAsync(
                select: expenceType => new ListExpenceType_DTO
                {
                    ID = expenceType.ID,
                    ExpenceTypeName = expenceType.ExpenceTypeName,
                    CreatedAt = expenceType.CreatedAt,
                    RecordStatus = expenceType.RecordStatus
                },
                where: expenceType => expenceType.RecordStatus != RecordStatus.IsDeleted
            );

            return expenceTypes;
        }

        /// <summary>
        /// ExpenceType Detayını verir
        /// </summary>
        /// <param name="id">Detayı istenen ExpenceType Id bilgisi</param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<ExpenceType_DTO> GetExpenceTypeDetailsAsync(int id)
        {
            var expenceType = await _expenceTypeRepository.FilteredSearchAsync
            (
                select: expenceType => new ExpenceType_DTO
                {
                    ID = expenceType.ID,
                    ExpenceTypeName = expenceType.ExpenceTypeName,
                    CreatedAt = expenceType.CreatedAt,
                    RecordStatus = expenceType.RecordStatus
                },
                where: expenceType => expenceType.ID == id && expenceType.RecordStatus != RecordStatus.IsDeleted
            );

            // Eğer harcama türü bulunmazsa, null döner ve hata mesajı verir
            var expenceTypeDetails = expenceType.FirstOrDefault();  // Harcama Türü yok ise null

            if (expenceTypeDetails == null)
            {
                throw new KeyNotFoundException("Harcama Türü bulunamadı."); // Hata mesajı ile metottan çıkar
            }

            return expenceTypeDetails;
        }

        public async Task<bool> SoftDeleteExpenceTypeAsync(int id)
        {
            // Harcama türünü al
            var expenceType = await _expenceTypeRepository.SearchByIdAsync(id);

            if (expenceType == null)
            {
                throw new KeyNotFoundException("Harcama türü bulunamadı.");
            }

            // Harcama türünün RecordStatus ü zaten IsDeleted ise
            if (expenceType.RecordStatus == RecordStatus.IsDeleted)
            {
                throw new InvalidOperationException("Harcama türü zaten silinmiş, tekrar silinemez");
            }

            // Harcama türünün RecordStatus'ü IsDeleted yapılıyor
            expenceType.RecordStatus = RecordStatus.IsDeleted;

            // Güncellemeyi kaydet
            await _expenceTypeRepository.UpdateAsync(expenceType);

            var result = true; // result'a UpdateAsync 'nin değerini ver
            //_____ düzeltme gerekli

            return result;
        }

        public async Task<bool> UpdateExpenceTypeAsync(int id, UpdateExpenceType_DTO updateExpenceType)
        {
            var expenceType = await _expenceTypeRepository.SearchByIdAsync(id);
            if (expenceType == null)
            {
                throw new KeyNotFoundException("Harcama türü bulunamadı.");
            }

            // RecordStatus'ü update yap ve güncelle
            expenceType.RecordStatus = updateExpenceType.RecordStatus;

            // Güncellemeyi kaydet
            //_____ düzeltme gerekli
            //return await _expenceRepository.UpdateAsync(expenceType);
            return true;
        }
    }
}
