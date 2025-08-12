//using AutoMapper;
//using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.AppUser_DTOs;
//using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs;
//using KD25_BitirmeProjesi.ApplicationLayer.Services.CompanyManagerServices;
//using KD25_BitirmeProjesi.CoreLayer.Entities;
//using KD25_BitirmeProjesi.CoreLayer.Enums;
//using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;
//using KD25_BitirmeProjesi.InfrastructureLayer.Repositories.Concrete;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace KD25_BitirmeProjesi.ApplicationLayer.Services.ExpenceServices
//{
//    public class CompanyManagerService : ICompanyManagerService
//    {
//        private readonly ICompanyManagerRepository _companyManagerRepository;
//        private readonly IMapper _mapper;
//        private readonly UserManager<AppUser> _userManager;
//        private readonly IAppUserRepository _appUserRepository;

//        public CompanyManagerService(ICompanyManagerRepository companyManagerRepository, IMapper mapper, UserManager<AppUser> userManager, IAppUserRepository appUserRepository)
//        {
//            _companyManagerRepository = companyManagerRepository;
//            _mapper = mapper;
//            _userManager = userManager;
//            _appUserRepository = appUserRepository;
//        }

//        public Task AddCompanyManagerAsync(AppUserAdd_DTO addPersonal)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<AppUser_DTO> GetCompanyManagerDetailsAsync(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public async Task<IEnumerable<AppUserList_DTO>> ListAllPersonels(int companyID, int takeNumber = 0)
//        {

//            var personels = await _appUserRepository.FilteredSearchAsync(
//                select: personel => new AppUserList_DTO
//                {
//                    UserID = personel.Id,
//                    Avatar = personel.Avatar,
//                    FirstName = personel.FirstName,
//                    Surname = personel.Surname,
//                    Department = personel.Department.DepartmentName,
//                    Proficiency = personel.Proficiency
//                },
//                where: user => user.IsActive == true && user.CompanyID == companyID,
//                orderBY: query => query.OrderByDescending(x => x.StartDate),
//                takeNumber: takeNumber,
//                include: query => query.Include(a => a.Department)
//            );

//            Console.WriteLine("--------------- ListAllPersonels - CompanyManagerService");
//            foreach (var item in personels)
//            {
//                Console.WriteLine(item.UserID);
//                Console.WriteLine(item.FirstName);
//                Console.WriteLine(item.Department);
//            }

//            return personels;
//        }

//        public Task<bool> PassiveCompanyManagerAsync(int id)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<bool> UpdateCompanyManagerAsync(int id, AppUserUpdate_DTO updateDTO)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}