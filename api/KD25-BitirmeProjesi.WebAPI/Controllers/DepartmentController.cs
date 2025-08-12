using KD25_BitirmeProjesi.ApplicationLayer.Services.DepartmentServices;
using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.WebAPI.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.GetAllDepartments();
            return View(departments);
        }


    }
}
