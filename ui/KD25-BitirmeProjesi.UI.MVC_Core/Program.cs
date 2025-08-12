using KD25_BitirmeProjesi.UI.MVC_Core.Mapper;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Ekledi�imiz Servisler
//builder.Services.AddHttpClient();
builder.Services.AddHttpClient("APIClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:7071");
});

//AutoMapper
builder.Services.AddAutoMapper(typeof(AppMapper));

//CustomHttpClientHelper i�in
builder.Services.AddHttpContextAccessor();



// Add session services
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session s�resi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//builder.Services.AddAuthentication("Identity.Application")
//    .AddCookie("Identity.Application", options =>
//    {
//        options.LoginPath = "/Login/Login";         // Giri� yap�lmam��sa y�nlendirilecek URL
//        options.AccessDeniedPath = "/AccessDenied"; // Rol yetkisi yoksa y�nlendirilecek URL
//    });

//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.LoginPath = "/Login/Login"; // Giri� yap�lmam��sa y�nlendirme
//        options.AccessDeniedPath = "/Login/AccessDenied"; // Yetkisiz eri�imde y�nlendirme
//    });

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Login/Login/"; // Login sayfas�n�n yolu
//    options.AccessDeniedPath = "/AccessDenied"; // Yetkisi olmayan kullan�c�lar i�in
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

// Area'lar� etkinle�tirmek i�in
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
