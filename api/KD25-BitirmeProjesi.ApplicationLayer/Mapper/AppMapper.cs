using AutoMapper;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Advance_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.AppUser_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Department_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Company_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.LeaveRecord_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.LeaveRecordType_DTOs;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KD25_BitirmeProjesi.ApplicationLayer.Mapper
{
    public class AppMapper: Profile
    {
        public AppMapper() 
        {
            //AppUser
            CreateMap<AppUser,AppUser_DTO>().ReverseMap();
            CreateMap<AppUser,AppUserAdd_DTO>().ReverseMap();
            CreateMap<AppUser,AppUserUpdate_DTO>().ReverseMap();
            CreateMap<AppUser,AppUserList_DTO>().ReverseMap();

            // AppUser -> AppUserDetailDto
            CreateMap<AppUser, AppUserDetailDto>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.Id));

            // AppUserDetailDto -> AppUser
            CreateMap<AppUserDetailDto, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserID))
                // aşağıdaki alanlar Identity tarafından kontrol edilir, güncellenmesi sakıncalı olabilir
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedUserName, opt => opt.Ignore())
                .ForMember(dest => dest.NormalizedEmail, opt => opt.Ignore())
                .ForMember(dest => dest.EmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.SecurityStamp, opt => opt.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, opt => opt.Ignore());



            // Advance 
            CreateMap<SalaryAdvance, SalaryAdvance_DTO>().ReverseMap();
            CreateMap<SalaryAdvance, AddSalaryAdvance_DTO>().ReverseMap();
            CreateMap<SalaryAdvance, ListSalaryAdvance_DTO>().ReverseMap();
            CreateMap<SalaryAdvance, UpdateSalaryAdvance_DTO>().ReverseMap();

            //Expence
            CreateMap<Expence, AddExpence_DTO>().ReverseMap();
            CreateMap<Expence, UpdateExpence_DTO>().ReverseMap();
            CreateMap<Expence, ListExpences_DTO>().ReverseMap();

            //Expence Type
            CreateMap<ExpenceType, AddExpenceType_DTO>().ReverseMap();
            CreateMap<ExpenceType, UpdateExpenceType_DTO>().ReverseMap();
            CreateMap<ExpenceType, ListExpenceType_DTO>().ReverseMap();

            //LeaveRecord
            CreateMap<LeaveRecord, AddLeaveRecord_DTO>().ReverseMap();
            CreateMap<LeaveRecord, ListLeaveRecord_DTO>().ReverseMap();
            CreateMap<LeaveRecord, UpdateLeaveRecord_DTO>().ReverseMap();

            //LeaveRecordType
            CreateMap<LeaveRecordType, AddLeaveRecordType_DTO>().ReverseMap();
            CreateMap<LeaveRecordType, ListLeaveRecordType_DTO>().ReverseMap();
            CreateMap<LeaveRecordType, UpdateLeaveRecordType_DTO>().ReverseMap();

            //Company
            CreateMap<Company, CreateCompany_DTO>().ReverseMap();


            //Usersummary
            CreateMap<AppUser, UserSummary_DTO>().ReverseMap();

            //Department
            CreateMap<Department, Department_DTO>().ReverseMap();
            //CreateMap<AppUser, App_DTO>().ReverseMap();
        }
    }
}
