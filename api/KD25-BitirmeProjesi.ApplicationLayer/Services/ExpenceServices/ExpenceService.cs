using AutoMapper;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.CoreLayer.Enums;
using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.ExpenceServices
{
    public class ExpenceService : IExpenceService
    {
        private readonly IExpenceRepository _expenceRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public ExpenceService(IExpenceRepository expenceRepository, IMapper mapper, UserManager<AppUser> userManager)
        {
            _expenceRepository = expenceRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        // Yeni Harcama ekleme
        public async Task AddExpenceAsync(AddExpence_DTO addExpence)
        {
            var expence = new Expence();
            _mapper.Map(addExpence, expence);

            // Mapper olmadan:
            /*
            var expence = new Expence
            {
                CurrencyType = addExpence.Currency,
                Amount = addExpence.Amount,
                ExpenceTypeID = addExpence.ExpenceTypeID,
                RequestDate = DateTime.Now,
                ApprovalStatus = ApprovalStatus.Pending,
                FilePath = addExpence.FilePath,
                AppUserID = addExpence.AppUserID,
                Explanation = addExpence.Explanation
            };*/
            expence.CurrencyType = addExpence.Currency;
            Console.WriteLine("ExpenceService Application Layer _-_-__--__--_-_-_--_-___ Kayıt işlemi gerçekleşiyor");
            await _expenceRepository.CreateAsync(expence);
            Console.WriteLine("-------------expence eklerken currencyType: " + expence.CurrencyType);
            Console.WriteLine("-------------expence eklerken addCurrencyType: " + addExpence.Currency);
        }

        
        public async Task<bool> CancelExpenceByEmployeeAsync(int id)
        {
            // Harcama talebini al
            var expence = await _expenceRepository.SearchByIdAsync(id);

            if (expence == null)
            {
                throw new KeyNotFoundException("Harcama talebi bulunamadı.");
            }

            // Harcama talebinin ApprovalStatus'ı 3 (IsPending) olmalı
            if (expence.ApprovalStatus != ApprovalStatus.Pending)
            {
                throw new InvalidOperationException("Harcama talebi onaylanmış ya da reddedilmiş, iptal edilemez.");
            }

            // Harcama talebinin ApprovalStatus'unu 2 (IsCancelled) olarak güncelle
            expence.ApprovalStatus = ApprovalStatus.Cancelled;

            // Harcama talebinin Status'unu 0 (IsPassive) olarak güncelle
            expence.RecordStatus = RecordStatus.IsDeleted;

            // Güncellemeyi kaydet
            await _expenceRepository.UpdateAsync(expence);
            
            var result = true; // result'a UpdateAsync 'nin değerini ver
            //_____ düzeltme gerekli

            return result;
        }

        public async Task<bool> UpdateExpenceAsync(int id, UpdateExpence_DTO updateExpence)
        {
            var expence = await _expenceRepository.SearchByIdAsync(id);
            if (expence == null)
            {
                throw new KeyNotFoundException("Harcama bulunamadı.");
            }

            // Sadece IsPending durumunda güncellenebilir
            if (expence.ApprovalStatus != ApprovalStatus.Pending)
            {
                throw new InvalidOperationException("Bu harcama zaten onaylanmış veya reddedilmiştir.");
            }

            // ApprovalStatus ve ReplyDate güncellemesi yap
            expence.ApprovalStatus = updateExpence.ApprovalStatus;
            expence.ResponseDate = DateTime.Now;

            // Güncellemeyi kaydet
            //_____ düzeltme gerekli
            //return await _expenceRepository.UpdateAsync(expence);
            return true;
        }

        

        public async Task<IEnumerable<ListExpences_DTO>> GetAllExpencesAsync(int userId)
        {
            // burada mapper kullanımı biraz farklı, iyice öğrenip kullanabiliriz.

            // Kullanıcıya ait harcama taleplerini getir.
            var expences = await _expenceRepository.FilteredSearchAsync(
                select: expence => new ListExpences_DTO
                {
                    ID = expence.ID,
                    Amount = expence.Amount,
                    FilePath = expence.FilePath,
                    ApprovalStatus = expence.ApprovalStatus, // değişken, DTO içinde string ise string değerini alır, değil ise enum olarak
                    ApprovalDate = expence.ResponseDate,
                    ResponseDate = expence.ResponseDate,
                    ExpenceType = expence.ExpenceType.ExpenceTypeName,
                    Currency = expence.CurrencyType,
                    Explanation = expence.Explanation,
                    AppUser = $"{expence.AppUser.FirstName} {expence.AppUser.Surname}" // Ad ve soyadı
                },
                where: expence => expence.ApprovalStatus != ApprovalStatus.Cancelled && expence.AppUserID == userId,
                include: query => query.Include(a => a.AppUser).Include(exType => exType.ExpenceType) // İlişkili tabloları dahil et
            );
            Console.WriteLine("API_______________________________________--------------------_____________");
            foreach (var item in expences)
            {
                Console.WriteLine($"ID: {item.ID} - Currency: {item.Currency}");
            }
            return expences;
        }



        public async Task<IEnumerable<ListExpences_DTO>> GetLastExpencesAsync(int companyID, int takeNumber)
        {
            // burada mapper kullanımı biraz farklı, iyice öğrenip kullanabiliriz.

            // Kullanıcıya ait harcama taleplerini getir.
            var expences = await _expenceRepository.FilteredSearchAsync(
                select: expence => new ListExpences_DTO
                {
                    ID = expence.ID,
                    Amount = expence.Amount,
                    FilePath = expence.FilePath,
                    ApprovalStatus = expence.ApprovalStatus,
                    ApprovalDate = expence.ResponseDate,
                    ResponseDate = expence.ResponseDate,
                    ExpenceType = expence.ExpenceType.ExpenceTypeName,
                    Currency = expence.CurrencyType,
                    Explanation = expence.Explanation,
                    AppUser = $"{expence.AppUser.FirstName} {expence.AppUser.Surname}" // Ad ve soyadı
                },
                where: expence => expence.ApprovalStatus != ApprovalStatus.Cancelled && expence.AppUser.CompanyID == companyID,
                orderBY: query => query.OrderByDescending(x => x.ID),
                take: takeNumber,
                include: query => query.Include(a => a.AppUser).Include(exType => exType.ExpenceType) // İlişkili tabloları dahil et
            );
            Console.WriteLine("API___________________________Take işlemli-------------------_____________" + expences.Count());
            foreach (var item in expences)
            {
                Console.WriteLine($"ID: {item.ID} - Currency: {item.Currency}");
            }
            return expences;
        }


        public async Task<ListExpences_DTO> GetExpenceDetailsAsync(int id)
        {
            // Mapper eklenebilir ( bunun için turner if içeren kısmı haricen değer atamayı unutmadan)
            // DTO = detay için ayrı bir DTO EKlenebilir?
            //____ düzeltme gerekli

            // Tek bir izni, belirli ID'ye göre alıyoruz ve Employee bilgisini dahil ediyoruz
            var expence = await _expenceRepository.FilteredSearchAsync
            (
                select: expence => new ListExpences_DTO
                {
                    ID = expence.ID,
                    ApprovalDate = expence.ResponseDate,
                    FilePath = expence.FilePath,
                    ExpenceType = expence.ExpenceType.ExpenceTypeName,
                    Amount = expence.Amount,
                    ApprovalStatus = expence.ApprovalStatus,
                    ResponseDate = expence.ResponseDate,
                    Currency = expence.CurrencyType,
                    AppUser = expence.AppUser != null
                        ? $"{expence.AppUser.FirstName} {expence.AppUser.Surname}"
                        : "İsim Yok",
                    Explanation = expence.Explanation
                },
                where: expence => expence.ID == id && expence.RecordStatus != RecordStatus.IsDeleted,
                include: query => query.Include(u => u.AppUser).Include(eType => eType.ExpenceType) // ilişkili tablolar
            );

            // Eğer harcama bulunmazsa, null döner ve hata mesajı verir
            var expenceDetails = expence.FirstOrDefault();  // Harcama yok ise null

            if (expenceDetails == null)
            {
                throw new KeyNotFoundException("Harcama Talebi bulunamadı."); // Hata mesajı ile metottan çıkar
            }

            return expenceDetails;
        }



        // === Manager işlemleri ===
        
        // Approve Expence by Manager
        public async Task<bool> ApproveExpenceByManagerAsync(int id)
        {
            var expence = await _expenceRepository.SearchByIdAsync(id);
            if (expence == null)
            {
                throw new KeyNotFoundException("Harcama bulunamadı.");
            }

            if (expence.ApprovalStatus == ApprovalStatus.Approved)
            {
                throw new InvalidOperationException("Bu harcama zaten onaylanmış");
            }

            // ApprovalStatus ve ReplyDate güncellemesi yap
            expence.ApprovalStatus = ApprovalStatus.Approved;
            expence.ResponseDate = DateTime.Now;

            // Güncellemeyi kaydet
            //_____ düzeltme gerekli
            await _expenceRepository.UpdateAsync(expence);
            return true;
        }

        // Deny Expence by Manager
        public async Task<bool> DenyExpenceByManagerAsync(int id)
        {
            var expence = await _expenceRepository.SearchByIdAsync(id);
            if (expence == null)
            {
                throw new KeyNotFoundException("Harcama bulunamadı.");
            }

            if (expence.ApprovalStatus == ApprovalStatus.Denied)
            {
                throw new InvalidOperationException("Bu harcama zaten reddedilmiş");
            }

            expence.ApprovalStatus = ApprovalStatus.Denied;
            expence.ResponseDate = DateTime.Now;

            // Güncellemeyi kaydet
            //_____ düzeltme gerekli
            await _expenceRepository.UpdateAsync(expence);
            return true;
        }



    }
}
