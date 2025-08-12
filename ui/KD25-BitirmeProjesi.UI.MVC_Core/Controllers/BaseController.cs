using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IHttpClientFactory _httpClientFactory;

        public BaseController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Kullanıcı adını her sayfa için tek tek göndermemek için BaseController'ı kalıtım ile alan Controllerlar ViewBag ile kullanıcı adını gönderir
            var client = _httpClientFactory.CreateClient();
            var fullName = HttpContext.Session.GetString("FullName"); // Session'dan kullanıcı adını alıyor

            ViewBag.FullName = fullName ?? "Kullanıcı";
            base.OnActionExecuting(context);
        }
    }
}
