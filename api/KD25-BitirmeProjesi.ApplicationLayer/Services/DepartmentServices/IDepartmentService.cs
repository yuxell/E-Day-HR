using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Department_DTOs;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.DepartmentServices
{
    public interface IDepartmentService
    {
        Task<IEnumerable<Department_DTO>> GetAllDepartments();
    }
}
