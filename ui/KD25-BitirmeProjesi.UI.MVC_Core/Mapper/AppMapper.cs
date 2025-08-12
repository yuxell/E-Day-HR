using AutoMapper;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.SiteAdministrator.Models.DTOs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.SiteAdministrator.Models.ViewModels.Company_VMs;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Mapper
{
    public class AppMapper : Profile
    {
        public AppMapper()
        { 
            //Company
            CreateMap<CreateCompany_DTO, CreateCompany_VM>().ReverseMap();
        }
    }
}
