using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.DTOs.LeaveRecord_DTOs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.Expence_VMs;
using KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Models.ViewModels.SalaryAdvance_ViewModel;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.CompanyManager.Models.ViewModels
{
    public class CompanyManagerDashboard_VM
    {
        public IEnumerable<ListExpence_VM> Expences { get; set; }
        public IEnumerable<ListSalaryAdvance_VM> SalaryAdvances { get; set; }
        public IEnumerable<ListLeaveRecord_DTO> LeaveRecords { get; set; }
        public IEnumerable<ListPersonel_VM> Personels { get; set; }

    }
}
