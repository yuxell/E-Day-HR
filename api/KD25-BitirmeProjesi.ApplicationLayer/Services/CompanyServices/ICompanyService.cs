using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Company_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Expence_DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.CompanyServices
{
    public interface ICompanyService
    {
        // Admin İşlemleri
        Task<int> CreateCompanyAsync(CreateCompany_DTO createCompany); // Admin için yeni şirket ekleme metodu
    }
}
