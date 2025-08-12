using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.LeaveRecordType_DTOs;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.LeaveRecordTypeServices
{
    public interface ILeaveRecordTypeService
    {
        Task<IEnumerable<ListLeaveRecordType_DTO>> GetAllLeaveRecordTypesAsync();
        Task AddLeaveRecordTypeAsync(AddLeaveRecordType_DTO addLeaveRecordType);
        Task<bool> UpdateLeaveRecordTypeAsync(int id, UpdateLeaveRecordType_DTO updateLeaveRecordType);
        Task<bool> SoftDeleteLeaveRecordTypeAsync(int id);
    }
}