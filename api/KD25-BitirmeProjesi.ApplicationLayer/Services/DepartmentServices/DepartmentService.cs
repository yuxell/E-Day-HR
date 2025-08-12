using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Department_DTOs;
using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.DepartmentServices
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IMapper _mapper;
        private readonly IDepartmentRepository _repository;

        public DepartmentService(IMapper mapper, IDepartmentRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<IEnumerable<Department_DTO>> GetAllDepartments()
        {
            List<Department_DTO> list = new List<Department_DTO>();
            var departments = await _repository.ListAsync();
            _mapper.Map(list, departments);
            return list;
        }
    }
}
