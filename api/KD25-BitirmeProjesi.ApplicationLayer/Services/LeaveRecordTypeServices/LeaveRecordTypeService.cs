using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.LeaveRecordType_DTOs;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.CoreLayer.Enums;
using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.LeaveRecordTypeServices
{
    public class LeaveRecordTypeService : ILeaveRecordTypeService
    {
        private readonly ILeaveRecordTypeRepository _repository;
        private readonly IMapper _mapper;

        public LeaveRecordTypeService(ILeaveRecordTypeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        // Silinmemiş tüm izin türlerini DTO olarak getirir
        public async Task<IEnumerable<ListLeaveRecordType_DTO>> GetAllLeaveRecordTypesAsync()
        {
            var types = await _repository.FilteredSearchAsync(
                select: x => new ListLeaveRecordType_DTO
                {
                    ID = x.ID,
                    LeaveRecordName = x.LeaveRecordName,
                    RecordStatus = x.RecordStatus
                },
                where: x => x.RecordStatus != RecordStatus.IsDeleted // Sadece silinmemiş kayıtlar
            );
            return types;
        }

        // Yeni bir izin türü ekler
        public async Task AddLeaveRecordTypeAsync(AddLeaveRecordType_DTO dto)
        {
            var entity = _mapper.Map<LeaveRecordType>(dto); // DTO -> Entity dönüşümü
            entity.RecordStatus = RecordStatus.IsAdded;
            entity.CreatedAt = DateTime.UtcNow;
            await _repository.CreateAsync(entity);
        }

        // Var olan bir izin türünü günceller
        public async Task<bool> UpdateLeaveRecordTypeAsync(int id, UpdateLeaveRecordType_DTO dto)
        {
            var entity = await _repository.SearchByIdAsync(id);
            if (entity == null || entity.RecordStatus == RecordStatus.IsDeleted) return false;

            entity.LeaveRecordName = dto.LeaveRecordName;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.RecordStatus = RecordStatus.IsUpdated;

            await _repository.UpdateAsync(entity);
            return true;
        }

        // İzin türünü soft şekilde siler (veritabanında tutar ama pasif hale getirir)
        public async Task<bool> SoftDeleteLeaveRecordTypeAsync(int id)
        {
            var entity = await _repository.SearchByIdAsync(id);
            if (entity == null || entity.RecordStatus == RecordStatus.IsDeleted) return false;

            entity.RecordStatus = RecordStatus.IsDeleted;
            entity.DeletedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity);
            return true;
        }
    }
}
