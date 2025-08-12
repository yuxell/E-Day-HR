using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.AppUser_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.EmailServices;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;
using KD25_BitirmeProjesi.InfrastructureLayer.Repositories.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.AppUserServices
{
    public class AppUserService : IAppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IAppUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService; 
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppUserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IMapper mapper, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddPersonalAsync(AppUserAdd_DTO addPersonal)
        {
            var personal = new AppUser
            {
                IsActive = true  // Default olarak aktif
            };

            // DTO'yu employee nesnesine mapliyoruz
            _mapper.Map(addPersonal, personal);

            // Kullanıcıyı oluştur
            var createResult = await _userManager.CreateAsync(personal, addPersonal.Password);
            if (!createResult.Succeeded)
            {
                // Hata durumunda uygun bir işlem yapılabilir
                throw new Exception("Kullanıcı oluşturulurken bir hata oluştu.");
            }

            // Token'dan role bilgisini alıyoruz
            var role = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(role))
            {
                throw new Exception("Rol bilgisi alınamadı.");
            }

            // Kullanıcı rolüne göre atama yapıyoruz
            string roleName;
            if (role == "Admin")
            {
                // Admin rolü ise Manager rolü atıyoruz
                roleName = "Manager";
            }
            else if (role == "Manager")
            {
                // Manager rolü ise Personal rolü atıyoruz
                roleName = "Personal";
            }
            else
            {
                roleName = "Personal"; // Varsayılan olarak Personal rolü
            }

            // İlgili rol var mı diye kontrol ediyoruz
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                throw new Exception($"Belirtilen rol mevcut değil: {roleName}");
            }

            // Kullanıcıyı belirlenen role ekliyoruz
            await _userManager.AddToRoleAsync(personal, roleName);

            // Mail gönderimi
            string subject = "Hesabınız Aktifleştirildi";
            string body = $"Merhaba {personal.UserName}. Artık sisteme erişiminiz var. Lütfen giriş yapmak için kullanıcı adınızı ve şifrenizi kullanın. Kimlik bilgilerinizi yetkisi olmayan kişilerle paylaşmayın.";
            await _emailService.SendEmailAsync(personal.Email, subject, body);
        }

        public async Task<IEnumerable<AppUserList_DTO>> GetAllPersonalAsync(string companyID)
        {
            var personal = await _userRepository.FilteredSearchAsync<AppUserList_DTO>(
                select: p => new AppUserList_DTO
                {
                    UserID = p.Id,
                    Avatar = p.Avatar,
                    FirstName = p.FirstName,
                    SecondName = p.SecondName,
                    Surname = p.Surname,
                    SecondSurname = p.SecondSurname,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    Address = p.Address,
                    Proficiency = p.Proficiency,
                    Department = p.Department.DepartmentName
                },
                where: p => p.CompanyID == int.Parse(companyID) && p.IsActive == true,  //-------------------
                orderBY: null,
                include: null
            );

            return personal;
        }

        public async Task<AppUser_DTO> GetPersonalDetailsAsync(int id)
        {
            // Çalışanı repository'den ID'ye göre al
            var personal = await _userRepository.FindAsync(id);
            if (personal == null)
            {
                return null; // Çalışan bulunamadı
            }

            // Şirket bilgisini almak için FilteredSearchAsync'i kullan
            var companyInfo = await _userRepository.FilteredSearchAsync(
                select: p => new
                {
                    CompanyName = p.Company != null ? p.Company.CompanyName : "No Company" // Şirket adı, null ise varsayılan değer
                },
                where: p => p.Id == id, // Çalışan ID'ye göre filtreleniyor
                include: query => query.Include(p => p.Company) // Şirket bilgisini dahil et
            );

            if (companyInfo == null || !companyInfo.Any())
            {
                return null; // Şirket veya durum bilgileri alınamadı
            }

            // DTO'yu oluştur
            var personalDto = _mapper.Map<AppUser_DTO>(personal);

            // Şirket ve durum bilgilerini DTO'ya ekle
            var firstCompany = companyInfo.First();
            personalDto.Company = firstCompany.CompanyName; // Şirket adı

            // Rol bilgilerini al
            var roles = await _userManager.GetRolesAsync(personal);
            personalDto.Role = string.Join(", ", roles); // Birden fazla rol varsa, virgülle birleştir

            return personalDto;

        }

        public async Task<bool> PassivePersonalAsync(int id)
        {
            var personel = await _userRepository.FindAsync(id);
            if (personel == null) return false;

            personel.IsActive = false;
            return await _userRepository.UpdateAsync(personel);
        }

        public async Task<bool> UpdatePersonalAsync(int id, AppUserUpdate_DTO updatePersonal)
        {
            var personal = await _userRepository.FindAsync(id);

            if (personal == null || personal.IsActive != true)
            {
                return false; // Çalışan bulunamadı veya pasif
            }

            // Fotoğraf güncellemesi
            if (!string.IsNullOrEmpty(updatePersonal.Avatar) && personal.Avatar != updatePersonal.Avatar)
            {
                // Eski fotoğrafı sil (eğer varsa)
                if (!string.IsNullOrEmpty(personal.Avatar))
                {
                    DeleteAvatarFromDatabase(personal.Avatar);
                }

                // Yeni fotoğrafı ata
                //personal.Avatar = updatePersonal.Avatar;
            }

            // DTO'dan diğer verileri mevcut personel nesnesine uygula
            _mapper.Map(updatePersonal, personal);

            // Çalışanın status'ünü koru
            personal.IsActive = true;

            var updateResult = await _userRepository.UpdateAsync(personal);
            return updateResult;
        }

        private void DeleteAvatarFromDatabase(string avatarPath)
        {
            // Burada, dosyayı fiziksel olarak veya veri tabanından silebilirsiniz.
            // Örnek bir fiziksel silme işlemi:
            if (File.Exists(avatarPath))
            {
                File.Delete(avatarPath);
            }
        }



        public async Task<AppUser_DTO?> GetUserSummaryAsync(int userId)
        {
            var user = await _userRepository.GetUserSummaryAsync(userId);
            if (user == null)
                return null;

            return _mapper.Map<AppUser_DTO>(user);
        }

        public Task<IEnumerable<AppUserList_DTO>> GetManagersForAdminAsync()
        {
            throw new NotImplementedException();
        }
    }
}
