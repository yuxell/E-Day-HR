using Microsoft.AspNetCore.Mvc;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Areas.Personel.Controllers
{
    [Area("Personel")]
    [Route("Personel")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login", new { area = "" }); // Token yoksa login'e yönlendir
            }
            return View();
        }

        

    }
}
