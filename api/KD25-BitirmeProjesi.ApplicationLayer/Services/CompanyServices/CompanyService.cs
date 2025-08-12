using AutoMapper;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Company_DTOs;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.CompanyServices
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper, UserManager<AppUser> userManager)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        // Admin İşlemleri
        public async Task<int> CreateCompanyAsync(CreateCompany_DTO createCompany)
        {
            var company = new Company();
            _mapper.Map(createCompany, company);

            return await _companyRepository.CreateAsync(company);
        }
    }
}
