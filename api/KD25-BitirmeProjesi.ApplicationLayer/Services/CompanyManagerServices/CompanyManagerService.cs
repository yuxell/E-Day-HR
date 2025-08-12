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
using KD25_BitirmeProjesi.CoreLayer.Enums;
using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;
using KD25_BitirmeProjesi.InfrastructureLayer.Repositories.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.CompanyManagerServices
{
    public class CompanyManagerService : ICompanyManagerService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IAppUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ICompanyManagerRepository _companyManagerRepository;
        private readonly IHttpContextAccessor _httpContextAccessor; 

        public CompanyManagerService(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IAppUserRepository userRepository,
            IMapper mapper,
            IEmailService emailService,
            ICompanyManagerRepository companyManagerRepository,
            IHttpContextAccessor httpContextAccessor) 
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _mapper = mapper;
            _emailService = emailService;
            _companyManagerRepository = companyManagerRepository;
            _httpContextAccessor = httpContextAccessor;
        }




        #region Details
        public async Task<AppUser_DTO> GetCompanyManagerDetailsAsync(int id)
        {
            // FilteredSearchAsync kullanarak veriyi getirme
            var user = await _companyManagerRepository.FilteredSearchAsync(
                select: x => new AppUser_DTO
                {
                    UserID = x.Id,
                    FirstName = x.FirstName,
                    Surname = x.Surname,
                    Email = x.Email,
                    Address = x.Address,
                    PhoneNumber = x.PhoneNumber,
                    BirthDate = x.BirthDate,
                    Department = x.Department.DepartmentName,
                    Company = x.Company.CompanyName,
                    Proficiency = x.Proficiency,
                    Avatar = x.Avatar
                },
                where: x => x.Id == id && x.IsActive == true, // filtre
                include: x => x.Include(a => a.Department).Include(a => a.Company) // include işlemleri
            );

            // Eğer sonuç yoksa null döndür
            return user.FirstOrDefault();  // List dönüyor, ilk öğeyi alıyoruz
        }




        #endregion

        #region Update
        public async Task<bool> UpdateCompanyManagerAsync(int id, AppUserUpdate_DTO updateManagerDto)
        {
            var manager = await _userRepository.FindAsync(id);
            //employee = _mapper.Map<Employee>(updateEmployeeDto); // 

            if (manager == null || manager.IsActive != true)
            {
                return false; // Çalışan bulunamadı veya pasif
            }

            // Fotoğraf güncellemesi
            if (!string.IsNullOrEmpty(updateManagerDto.Avatar) && manager.Avatar != updateManagerDto.Avatar)
            {
                // Eski fotoğrafı sil (eğer varsa)
                if (!string.IsNullOrEmpty(manager.Avatar))
                {
                    DeletePhotoFromDatabase(manager.Avatar);
                }

                // Yeni fotoğrafı ata
                //employee.Photo = updateEmployeeDto.Photo;
            }

            // DTO'dan diğer verileri mevcut employee nesnesine uygula
            _mapper.Map(updateManagerDto, manager);

            // Çalışanın status'ünü koru
            manager.IsActive = true;

            var updateResult = await _userRepository.UpdateAsync(manager);
            return updateResult;
        }
        private void DeletePhotoFromDatabase(string photoPath)
        {
            // Burada, dosyayı fiziksel olarak veya veri tabanından silebilirsiniz.
            // Örnek bir fiziksel silme işlemi:
            if (File.Exists(photoPath))
            {
                File.Delete(photoPath);
            }
        }

        #endregion

        #region List
        public async Task<IEnumerable<AppUserList_DTO>> ListAllPersonels(int companyID, int takeNumber = 0)
        {

            var personels = await _userRepository.FilteredSearchAsync(
                select: personel => new AppUserList_DTO
                {
                    UserID = personel.Id,
                    Avatar = personel.Avatar,
                    FirstName = personel.FirstName,
                    Surname = personel.Surname,
                    Department = personel.Department.DepartmentName,
                    Proficiency = personel.Proficiency
                },
                where: user => user.IsActive == true && user.CompanyID == companyID,
                orderBY: query => query.OrderByDescending(x => x.StartDate),
                takeNumber: takeNumber,
                include: query => query.Include(a => a.Department)
            );

            Console.WriteLine("--------------- ListAllPersonels - CompanyManagerService");
            foreach (var item in personels)
            {
                Console.WriteLine(item.UserID);
                Console.WriteLine(item.FirstName);
                Console.WriteLine(item.Department);
            }

            return personels;
        }

        #endregion

        #region Pasif
        public async Task<bool> PassiveCompanyManagerAsync(int id)
        {
            var manager = await _userRepository.FindAsync(id);
            if (manager == null) return false;

            manager.IsActive = false;
            return await _userRepository.UpdateAsync(manager);
        }
        #endregion

        #region Add
        //public async Task AddCompanyManagerAsync(AppUserAdd_DTO addManager)
        //{
        //    var manager = new AppUser
        //    {
        //        IsActive = true
        //    };

        //    _mapper.Map(addManager, manager);
        //    var result = await _userManager.CreateAsync(manager, addManager.Password);
        //    if (!result.Succeeded) throw new Exception("Yönetici oluşturulamadı.");

        //    string roleName = "Manager";

        //    if (!await _roleManager.RoleExistsAsync(roleName))
        //        throw new Exception($"Rol bulunamadı: {roleName}");

        //    await _userManager.AddToRoleAsync(manager, roleName);

        //    string subject = "Hesabınız Aktif";
        //    string body = $"Sayın {manager.UserName}, hesabınız oluşturuldu. Lütfen sisteme giriş yapınız.";
        //    await _emailService.SendEmailAsync(manager.Email, subject, body);
        //}

        public async Task AddCompanyManagerAsync(AppUserAdd_DTO addPersonal)
        {
            var manager = new AppUser
            {
                IsActive = true  // Default olarak aktif
            };

            // DTO'yu employee nesnesine mapliyoruz
            _mapper.Map(addPersonal, manager);

            // Kullanıcıyı oluştur
            var createResult = await _userManager.CreateAsync(manager, addPersonal.Password);
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
            await _userManager.AddToRoleAsync(manager, roleName);

            // Mail gönderimi
            string subject = "Hesabınız Aktifleştirildi";
            string body = $"Merhaba {manager.UserName}. Artık sisteme erişiminiz var. Lütfen giriş yapmak için kullanıcı adınızı ve şifrenizi kullanın. Kimlik bilgilerinizi yetkisi olmayan kişilerle paylaşmayın.";
            await _emailService.SendEmailAsync(manager.Email, subject, body);
        }
        #endregion
    }
}

