using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.LeaveRecord_DTOs;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.CoreLayer.Enums;
using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;
using KD25_BitirmeProjesi.InfrastructureLayer.Repositories.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.LeaveRecordServices
{
    public class LeaveRecordService : ILeaveRecordService
    {
        private readonly ILeaveRecordRepository _leaveRecordRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public LeaveRecordService(ILeaveRecordRepository leaveRecordRepository, IMapper mapper, UserManager<AppUser> userManager)
        {
            _leaveRecordRepository = leaveRecordRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        //Add
        public async Task AddLeaveRecordAsync(AddLeaveRecord_DTO addLeaveRecord)
        {
            // StartDate ve EndDate arasındaki gün farkı...
            var requestedDays = (addLeaveRecord.EndDate - addLeaveRecord.StartDate).Days;
            if (requestedDays < 0)
            {
                throw new ArgumentException("Bitiş tarihi başlangıç tarihinden erken olamaz!");
            }

            // DTO'yu LeaveRecord entity'sine eşle ve varsayılan değerleri ayarla
            var leaveRecord = _mapper.Map<LeaveRecord>(addLeaveRecord);
            leaveRecord.RequestDate = DateTime.Now;                      // Varsayılan olarak mevcut tarih
            leaveRecord.TotalDays = requestedDays;                       // Talep edilen gün sayısı
            leaveRecord.ApprovalStatus = ApprovalStatus.Pending;         // Varsayılan olarak Pending
            leaveRecord.ResponseDate = DateTime.Now;                     // Henüz bir cevap olmadığı için boş bırak
            leaveRecord.AppUserID = addLeaveRecord.AppUserID;            // IdentityUser ID'sini AppUserID olarak ayarla

            leaveRecord.LeaveRecordTypeID = addLeaveRecord.LeaveRecordTypeID; // İzin türünü al ve ayarla

            // Yeni izni depoya ekle
            await _leaveRecordRepository.CreateAsync(leaveRecord);
        }

        //List
        public async Task<IEnumerable<ListLeaveRecord_DTO>> GetAllLeaveRecordAsync(int userId)
        {
            var leaveRecords = await _leaveRecordRepository.FilteredSearchAsync
            (
                select: leaveRecord => new ListLeaveRecord_DTO
                {
                    ID = leaveRecord.ID,
                    TotalDays = leaveRecord.TotalDays,
                    StartDate = leaveRecord.StartDate,
                    EndDate = leaveRecord.EndDate,
                    LeaveRecordType = leaveRecord.LeaveRecordType.LeaveRecordName,
                    ApprovalStatus = leaveRecord.ApprovalStatus,
                    ResponseDate = leaveRecord.ResponseDate,
                    AppUser = leaveRecord.AppUser != null
                    ? $"{leaveRecord.AppUser.FirstName} {leaveRecord.AppUser.Surname}"
                    : "No Name"
                },
                    where: leaveRecord => /*leaveRecord.AppUser.IsActive == true &&*/ leaveRecord.AppUserID == userId,
                    include: query => query.Include(x => x.AppUser)          //--------------------------------------------
            );
            
            return leaveRecords;
        }


        //Detail
        public async Task<ListLeaveRecord_DTO> GetLeaveRecordDetailsAsync(int id)
        {
            // Tek bir izni, belirli ID'ye göre alıyoruz ve AppUser bilgisini dahil ediyoruz
            var leaveRecord = await _leaveRecordRepository.FilteredSearchAsync
            (
                select: leaveRecord => new ListLeaveRecord_DTO
                {
                    ID = leaveRecord.ID,
                    TotalDays = leaveRecord.TotalDays,
                    StartDate = leaveRecord.StartDate,
                    EndDate = leaveRecord.EndDate,
                    LeaveRecordType = leaveRecord.LeaveRecordType.LeaveRecordName,
                    ApprovalStatus = leaveRecord.ApprovalStatus,
                    ResponseDate = leaveRecord.ResponseDate,
                    // AppUser bilgisini alıyoruz...
                    AppUser = leaveRecord.AppUser != null
                        ? $"{leaveRecord.AppUser.FirstName} {leaveRecord.AppUser.Surname}"
                        : "No Name"
                },
                where: leaveRecord => leaveRecord.ID == id /*&& leaveRecord.AppUser.IsActive == true*/, // Verilen ID'ye sahip izin
                include: query => query.Include(x => x.AppUser) // AppUser ile ilişkilendirme
            );

            // Eğer izin bulunmazsa, null döner ve hata veririz
            var leaveRecordDetails = leaveRecord.FirstOrDefault();  // İzin bulunmazsa null döner

            if (leaveRecordDetails == null)
            {
                throw new KeyNotFoundException("İzin bulunamadı.");
            }

            return leaveRecordDetails;
        }

        // Soft Delete
        public async Task<bool> SoftDeleteLeaveRecordAsync(int id)
        {
            // LeaveRecord'ü id'ye göre buluyoruz
            var leaveRecord = await _leaveRecordRepository.SearchByIdAsync(id);
            if (leaveRecord == null)
            {
                throw new KeyNotFoundException("Silinmek istenen izin kaydı bulunamadı.");
            }

            // Eğer zaten silinmişse, işlem yapmamıza gerek yok
            if (leaveRecord.RecordStatus == RecordStatus.IsDeleted)
            {
                throw new InvalidOperationException("Bu izin kaydı zaten silinmiş.");
            }

            // Soft delete işlemi: DeletedAt zamanını güncelle ve RecordStatus'u IsDeleted yap
            leaveRecord.DeletedAt = DateTime.UtcNow;
            leaveRecord.RecordStatus = RecordStatus.IsDeleted;

            // Veritabanında güncelleme yapıyoruz
            await _leaveRecordRepository.UpdateAsync(leaveRecord);

            return true;
        }

        //Update
        public async Task<bool> UpdateLeaveRecordAsync(int id, UpdateLeaveRecord_DTO updateLeaveRecord)
        {
            var leaveRecord = await _leaveRecordRepository.SearchByIdAsync(id);
            if (leaveRecord == null)
            {
                throw new KeyNotFoundException("İzin bulunamadı.");
            }

            // Sadece Pending durumunda güncellenebilir...
            if (leaveRecord.ApprovalStatus != ApprovalStatus.Pending)
            {
                throw new InvalidOperationException("Bu izin zaten onaylanmış veya reddedilmiştir.");
            }

            // ApprovalStatus ve ReplyDate güncellemesi yap
            leaveRecord.ApprovalStatus = updateLeaveRecord.ApprovalStatus;
            leaveRecord.ResponseDate = DateTime.Now;

            // Güncellemeyi kaydet
            await _leaveRecordRepository.UpdateAsync(leaveRecord);
            return true;  
        }




        // Manager

        //List for Manager
        public async Task<IEnumerable<ListLeaveRecord_DTO>> GetAllLeaveRecordsByCompanyAsync(int companyId, int takeNumber = 0)
        {
            var leaveRecords = await _leaveRecordRepository.FilteredSearchAsync
            (
                select: leaveRecord => new ListLeaveRecord_DTO
                {
                    ID = leaveRecord.ID,
                    TotalDays = leaveRecord.TotalDays,
                    StartDate = leaveRecord.StartDate,
                    EndDate = leaveRecord.EndDate,
                    LeaveRecordType = leaveRecord.LeaveRecordType.LeaveRecordName,
                    ApprovalStatus = leaveRecord.ApprovalStatus,
                    ResponseDate = leaveRecord.ResponseDate,
                    AppUser = leaveRecord.AppUser != null
                    ? $"{leaveRecord.AppUser.FirstName} {leaveRecord.AppUser.Surname}"
                    : "No Name"
                },
                    where: leaveRecord => leaveRecord.AppUser.CompanyID == companyId,
                    orderBY: query => query.OrderByDescending(x => x.CreatedAt),
                    take: takeNumber,
                    include: query => query.Include(x => x.AppUser)
            );
            return leaveRecords;
        }
    }
}
