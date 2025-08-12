using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using KD25_BitirmeProjesi.ApplicationLayer.Models.DTOs.Advance_DTOs;
using KD25_BitirmeProjesi.ApplicationLayer.Services.SalaryAdvanceServices;
using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.CoreLayer.Enums;
using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KD25_BitirmeProjesi.ApplicationLayer.Services.AdvanceServices
{
	public class SalaryAdvanceService : ISalaryAdvanceService
	{
		private readonly IMapper _mapper;
		private readonly ISalaryAdvaceRepository _salaryAdvanceRepository;
		private readonly UserManager<AppUser> _userManager;

		public SalaryAdvanceService(IMapper mapper, ISalaryAdvaceRepository salaryAdvanceRepository, UserManager<AppUser> userManager)
		{
			_mapper = mapper;
			_salaryAdvanceRepository = salaryAdvanceRepository;
			_userManager = userManager;
		}
		#region AddSalaryAdvanceAsync
		public async Task AddSalaryAdvanceAsync(AddSalaryAdvance_DTO addSalaryAdvance)
		{
			// Sisteme giriş yapan kullanıcıyı alıyoruz.
			var currentUser = await _userManager.FindByIdAsync(addSalaryAdvance.AppUserID.ToString());
			if (currentUser == null)
				throw new Exception("Sisteme giriş yapan kullanıcı bulunamadı.");

			// Maaş kontrolü: Avans tutarı maaşın 3 katını geçemez.
			if (addSalaryAdvance.Amount > (currentUser.Salary * 3))
				throw new Exception("Avans tutarı maaşınızın 3 katını geçemez.");

			// Yeni avans talebi.
			var salaryAdvance = new SalaryAdvance
			{
				Amount = addSalaryAdvance.Amount,
				Explanation = addSalaryAdvance.Explanation,
				RequestDate = DateTime.UtcNow, 
				ResponseDate = DateTime.Now, // .Now neden?   
				ApprovalStatus = ApprovalStatus.Pending, 
				SalaryAdvanceType = addSalaryAdvance.SalaryAdvanceType,
				CurrencyType = addSalaryAdvance.CurrencyType,
				AppUserID = addSalaryAdvance.AppUserID, 
				// Kullanıcı IsActive / Passive gibi bir sey olması lazım. enum olabilir?
		};

			// Avans talebi veritabanına ekleniyor.
			await _salaryAdvanceRepository.CreateAsync(salaryAdvance);
		}

		#endregion

		#region GetAdvanceByManagerAsync

		public async Task<IEnumerable<ListSalaryAdvance_DTO>> GetSalaryAdvanceByManagerAsync(string companyId)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region GetSalaryAdvanceDetailsAsync
		public async Task<ListSalaryAdvance_DTO> GetSalaryAdvanceDetailsAsync(int id)
		{
			var salaryAdvance = await _salaryAdvanceRepository.FilteredSearchAsync
			(
				select: salaryAdvance => new ListSalaryAdvance_DTO
				{
					ID = salaryAdvance.ID,
					RequestDate = salaryAdvance.RequestDate,
					SalaryAdvanceType = salaryAdvance.SalaryAdvanceType,
					Explanation = salaryAdvance.Explanation,
					Amount = salaryAdvance.Amount,
					ApprovalStatus = salaryAdvance.ApprovalStatus,
					ResponseDate = salaryAdvance.ResponseDate,
					CurrencyType = salaryAdvance.CurrencyType,
					AppUser = salaryAdvance.AppUser != null
						? $"{salaryAdvance.AppUser.FirstName} {salaryAdvance.AppUser.Surname}"
						: "No Name"
				},
				where: salaryAdvance => salaryAdvance.ID == id && salaryAdvance.ApprovalStatus != ApprovalStatus.Cancelled,
				include: query => query.Include(p => p.AppUser) // ✅ SADECE navigation property
			);

			var salaryAdvanceDetails = salaryAdvance.FirstOrDefault();

			if (salaryAdvanceDetails == null)
			{
				throw new KeyNotFoundException("İzin bulunamadı.");
			}

			return salaryAdvanceDetails;
		}

		#endregion

		#region GetAllSalaryAdvanceAsync
		public async Task<IEnumerable<ListSalaryAdvance_DTO>> GetAllSalaryAdvanceAsync(int userId)
		{
			// Kullanıcıya ait avans taleplerini getir.
			var salaryAdvances = await _salaryAdvanceRepository.FilteredSearchAsync(
				select: salaryAdvance => new ListSalaryAdvance_DTO
				{
					ID = salaryAdvance.ID,
					Amount = salaryAdvance.Amount,
					Explanation = salaryAdvance.Explanation,
					ApprovalStatus = salaryAdvance.ApprovalStatus,
					RequestDate = salaryAdvance.RequestDate,
					ResponseDate = salaryAdvance.RequestDate,
					SalaryAdvanceType = salaryAdvance.SalaryAdvanceType,
					CurrencyType = salaryAdvance.CurrencyType,
					AppUser = $"{salaryAdvance.AppUser.FirstName} {salaryAdvance.AppUser.Surname}" // Ad ve soyadı
				},
				where: salaryAdvance => salaryAdvance.ApprovalStatus != ApprovalStatus.Cancelled && salaryAdvance.AppUserID == userId,
				include: query => query.Include(a => a.AppUser) //.Include(x => x.CurrencyType).Include(y => y.SalaryAdvanceType) // AppUser bilgilerini dahil et.
			);

			return salaryAdvances;

		}
		#endregion

		#region UpdateSalaryAdvanceAsync
		public async Task<bool> UpdateSalaryAdvanceAsync(int id, UpdateSalaryAdvance_DTO updateSalaryAdvance)
		{
			var salaryAdvance = await _salaryAdvanceRepository.SearchByIdAsync(id);
			if (salaryAdvance == null)
			{
				throw new KeyNotFoundException("Update edilecek Avans talebi bulunamadı.");
			}

			// Sadece IsPending durumunda güncellenebilir
			if (salaryAdvance.ApprovalStatus != ApprovalStatus.Pending)
			{
				throw new InvalidOperationException("Talebiniz çoktan onaylandı veya Reddedildi");
			}

			salaryAdvance.ApprovalStatus = updateSalaryAdvance.ApprovalStatus;
			salaryAdvance.ResponseDate = DateTime.Now;

			await _salaryAdvanceRepository.UpdateAsync(salaryAdvance);
			return true;
		}
		#endregion

		#region PassiveAdvanceAsync

		public async Task<bool> PassiveSalaryAdvanceAsync(int id)
		{
            var salaryAdvance = await _salaryAdvanceRepository.SearchByIdAsync(id);

            if (salaryAdvance == null)
                throw new KeyNotFoundException("Avans talebi bulunamadı.");

            // Eğer zaten onaylanmış ya da reddedilmişse, iptal edilemez
            if (salaryAdvance.ApprovalStatus != ApprovalStatus.Pending)
                throw new InvalidOperationException("Avans talebi onaylanmış ya da reddedilmiş, iptal edilemez.");

            // Enum üzerinden Cancelled olarak işaretle
            salaryAdvance.ApprovalStatus = ApprovalStatus.Cancelled;
            salaryAdvance.ResponseDate = DateTime.Now;

            await _salaryAdvanceRepository.UpdateAsync(salaryAdvance);
            return true;
        }

		#endregion
	}
}
