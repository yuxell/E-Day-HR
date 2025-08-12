using KD25_BitirmeProjesi.UI.MVC_Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace KD25_BitirmeProjesi.UI.MVC_Core.Controllers
{
    /// <summary>
    /// Kullanıcı girişi, şifre sıfırlama ve çıkış işlemlerini yöneten controller sınıfı.
    /// </summary>
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// IHttpClientFactory nesnesini enjekte ederek API çağrıları yapmak için kullanılır.
        /// </summary>
        /// <param name="httpClientFactory">HttpClient üretimi için kullanılan fabrika nesnesi.</param>
        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Giriş sayfasını döndürür.
        /// </summary>
        /// <returns>Login View sayfası.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Kullanıcının giriş bilgilerini API'ye göndererek doğrulama işlemi yapar.
        /// Başarılı ise token ve kullanıcı bilgilerini session'a kaydeder.
        /// </summary>
        /// <param name="model">Kullanıcı adı ve şifre bilgilerini içeren model.</param>
        /// <returns>Başarılıysa ana sayfaya, değilse Login View'a geri döner.</returns>
        //[HttpPost]
        //public async Task<IActionResult> Login(Login_VM model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    var client = _httpClientFactory.CreateClient();
        //    var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

        //    var response = await client.PostAsync("https://localhost:7071/api/Login", content); // API URL

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre.");
        //        return View(model);
        //    }

        //    var json = await response.Content.ReadAsStringAsync();
        //    var result = JsonConvert.DeserializeObject<LoginResponse_VM>(json);

        //    // Token ve diğer bilgiler session'a kaydedilir
        //    HttpContext.Session.SetString("token", result.Token);
        //    HttpContext.Session.SetString("role", result.RoleName);
        //    HttpContext.Session.SetInt32("userId", result.UserId);
        //    HttpContext.Session.SetInt32("companyId", result.CompanyId ?? 0);

        //    return RedirectToAction("Index", "Personel");
        //}

        [HttpPost]
        public async Task<IActionResult> Login(Login_VM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            // JSON gönderimini debuglamak için
            var jsonToSend = JsonConvert.SerializeObject(model);
            System.Diagnostics.Debug.WriteLine("Gönderilen JSON: " + jsonToSend);

            

            var content = new StringContent(jsonToSend, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7071/api/Login/Login", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("API Hatası: " + error);

                ModelState.AddModelError("", "Geçersiz kullanıcı adı veya şifre."); // veya $"API Hatası: {error}"
                return View(model);
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LoginResponse_VM>(json);

            HttpContext.Session.SetString("Token", result.Token);
            HttpContext.Session.SetString("Role", result.RoleName);
            HttpContext.Session.SetString("FullName", result.FullName ?? "");
            HttpContext.Session.SetInt32("UserId", result.UserId);
            HttpContext.Session.SetInt32("CompanyId", result.CompanyId ?? 0);


            //// Claims oluştur (Authorization için gerekli)
            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, result.FullName ?? "Kullanıcı"),
            //    new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString()),
            //    new Claim(ClaimTypes.Role, result.RoleName),
            //    new Claim("CompanyId", result.CompanyId?.ToString() ?? "0")
            //};
            //// ClaimsIdentity ve ClaimsPrincipal oluştur
            //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var principal = new ClaimsPrincipal(identity);
            //// Kullanıcıyı sign in et (cookie oluşturulur)
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);



            Console.WriteLine("\n");
            Console.WriteLine("------------- LoginController session'dan ");
            Console.WriteLine("------------- Role = " + HttpContext.Session.GetString("Role"));
            Console.WriteLine("------------- UserID = " + HttpContext.Session.GetInt32("UserId"));
            Console.WriteLine("------------- CompanyID = " + HttpContext.Session.GetInt32("CopanyId"));
            Console.WriteLine("------------- FullName = " + HttpContext.Session.GetString("FullName"));
            Console.WriteLine("------------- TOKEN = " + HttpContext.Session.GetString("Token"));

            switch (result.RoleName)
            {
                case "CompanyManager":
                    return Redirect(Url.Action("Index", "CompanyManager") + "/");
                case "Personel":
                    return Redirect(Url.Action("Index", "Personel") + "/");
                case "Admin":
                    return Redirect(Url.Action("Index", "Admin") + "/");
                default:
                    return Redirect(Url.Action("Index", "Home") + "/");

                    // + "/" ekleniyor çünkü template menüleri bu olmazsa açık geliyor
            }
        }




        /// <summary>
        /// Şifremi unuttum sayfasını döndürür.
        /// </summary>
        /// <returns>ForgotPassword View sayfası.</returns>
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        /// <summary>
        /// Kullanıcının e-posta adresine şifre sıfırlama bağlantısı gönderir.
        /// </summary>
        /// <param name="model">E-posta adresini içeren model.</param>
        /// <returns>Başarılıysa bilgi mesajı ile aynı sayfaya döner; değilse hata mesajı gösterilir.</returns>
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPassword_VM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();

            // 🔽 API'nin ihtiyaç duyduğu veriyi burada hazırlıyorsun
            var dto = new ForgotPassword_VM
            {
                Email = model.Email,
                ClientAppUrl = "https://localhost:7071/api/Login/forgot-password" // 🔁 MVC'deki ResetPassword sayfanın tam adresi
            };

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7071/api/Login/update-password", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "E-posta gönderildi. Lütfen kontrol edin.";
                return View();
            }

            ModelState.AddModelError("", "Hata oluştu.");
            return View(model);
        }



        /// <summary>
        /// Şifre sıfırlama formunu döndürür.
        /// Token ve e-posta adresi query string ile alınır.
        /// </summary>
        /// <param name="token">Şifre sıfırlama token'ı.</param>
        /// <param name="email">Kullanıcının e-posta adresi.</param>
        /// <returns>ResetPassword View sayfası.</returns>
        [HttpGet]
        public IActionResult UpdatePassword(string token, string email)
        {
            var model = new UpdatePassword_VM { Token = token, Email = email };
            return View(model);
        }

        /// <summary>
        /// Kullanıcının yeni şifresini alarak API üzerinden şifre sıfırlama işlemi gerçekleştirir.
        /// </summary>
        /// <param name="model">Yeni şifre, token ve e-posta bilgilerini içeren model.</param>
        /// <returns>Başarılıysa Login sayfasına yönlendirir; değilse hata mesajı gösterilir.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePassword_VM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7016/api/Login/update-password", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Şifreniz başarıyla sıfırlandı.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Şifre sıfırlama başarısız.");
            return View(model);
        }

        /// <summary>
        /// Kullanıcının oturumunu sonlandırır ve Login sayfasına yönlendirir.
        /// </summary>
        /// <returns>Login View sayfasına yönlendirme.</returns>
        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Clear();
        //    return RedirectToAction("Login");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Oturumu sonlandır
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Session'ı temizle
            HttpContext.Session.Clear();

            // Login sayfasına yönlendir
            return RedirectToAction("Index", "Home");
        }
    }

}

