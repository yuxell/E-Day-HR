using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.LeaveRecord_DTOs;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.LeaveRecordServices
{
    public interface ILeaveRecordService
    {
        Task<IEnumerable<ListLeaveRecord_DTO>> GetAllLeaveRecordAsync(int userId);
        Task AddLeaveRecordAsync(AddLeaveRecord_DTO addLeaveRecord);
        Task<bool> UpdateLeaveRecordAsync(int id, UpdateLeaveRecord_DTO updateLeaveRecord); 
        Task<ListLeaveRecord_DTO> GetLeaveRecordDetailsAsync(int id);
        Task<bool> SoftDeleteLeaveRecordAsync(int id);

        // Manager Kısmı
        Task<IEnumerable<ListLeaveRecord_DTO>> GetAllLeaveRecordsByCompanyAsync(int companyId, int takeNumber = 0);
        
    }
}
