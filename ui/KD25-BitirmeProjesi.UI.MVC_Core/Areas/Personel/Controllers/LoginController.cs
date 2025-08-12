using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Controllers
{
    [Area("Personel")]
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
