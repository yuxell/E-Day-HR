using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Advance_DTOs;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.SalaryAdvanceServices
{
	public interface ISalaryAdvanceService
	{
		Task<IEnumerable<ListSalaryAdvance_DTO>> GetAllSalaryAdvanceAsync(int userId); // Manager görecek onay/red olmadığı sürece
		Task<bool> UpdateSalaryAdvanceAsync(int id, UpdateSalaryAdvance_DTO updateSalaryAdvance); // Manager onay yada red verecek  ApprovalStatus
		// Task<bool> PassiveAdvanceAsync(int id); // Personel onay/red olmadığı sürece iptal edebilecek
		Task AddSalaryAdvanceAsync(AddSalaryAdvance_DTO addSalaryAdvance); // Personel'in başvuru yapabilmesi için 
		Task<ListSalaryAdvance_DTO> GetSalaryAdvanceDetailsAsync(int id); 
		Task<IEnumerable<ListSalaryAdvance_DTO>> GetSalaryAdvanceByManagerAsync(string companyId);

		Task<bool> PassiveSalaryAdvanceAsync(int id);  //Manager sınıfında kullanılacak ???
	}
}
