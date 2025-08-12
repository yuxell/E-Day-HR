using KD25_BitirmeProjesi.UI.MVC_Core.Mapper;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Eklediðimiz Servisler
//builder.Services.AddHttpClient();
builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7071");
});

//AutoMapper
builder.Services.AddAutoMapper(typeof(AppMapper));

//CustomHttpClientHelper için
builder.Services.AddHttpContextAccessor();



// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session süresi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//builder.Services.AddAuthentication("Identity.Application")
//    .AddCookie("Identity.Application", options =>
//    {
//        options.LoginPath = "/Login/Login";         // Giriþ yapýlmamýþsa yönlendirilecek URL
//        options.AccessDeniedPath = "/AccessDenied"; // Rol yetkisi yoksa yönlendirilecek URL
//    });

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Login/Login"; // Giriþ yapýlmamýþsa yönlendirme
//        options.AccessDeniedPath = "/Login/AccessDenied"; // Yetkisiz eriþimde yönlendirme
//    });

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Login/Login/"; // Login sayfasýnýn yolu
//    options.AccessDeniedPath = "/AccessDenied"; // Yetkisi olmayan kullanýcýlar için
//});


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login";
        options.LogoutPath = "/Login/Logout";

    });



var app = builder.Build();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");












// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
// Add session middleware
app.UseSession();
app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();

// Area'larý etkinleþtirmek için
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseCors(x => x.AllowAnyHeader()
          .AllowAnyOrigin()
          .AllowAnyMethod()
          );

app.Run();
