using KD25_BitirmeProjesi.CoreLayer.Entities;
using KD25_BitirmeProjesi.InfrastructureLayer.DAL;
using Microsoft.EntityFrameworkCore;
using KD25_BitirmeProjesi.ApplicationLayer.Mapper;
using KD25_BitirmeProjesi.ApplicationLayer.Services.AppUserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using KD25_BitirmeProjesi.CoreLayer.Repositories.Abstract;
using KD25_BitirmeProjesi.InfrastructureLayer.Repositories.Concrete;
using KD25_BitirmeProjesi.ApplicationLayer.Services.EmailServices;
using KD25_BitirmeProjesi.ApplicationLayer.Services.ExpenceServices;
using KD25_BitirmeProjesi.ApplicationLayer.Services.LeaveRecordServices;
using KD25_BitirmeProjesi.ApplicationLayer.Services.SalaryAdvanceServices;
using KD25_BitirmeProjesi.ApplicationLayer.Services.AdvanceServices;
using KD25_BitirmeProjesi.ApplicationLayer.Services.LeaveRecordTypeServices;
using KD25_BitirmeProjesi.ApplicationLayer.Services.LoginServices;
using Microsoft.AspNetCore.Identity;
using KD25_BitirmeProjesi.ApplicationLayer.Services.CompanyManagerServices;
using KD25_BitirmeProjesi.ApplicationLayer.Services.CompanyServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



//Context
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlServer());

//Identity
builder.Services.AddIdentity<AppUser, AppRole>(x => x.SignIn.RequireConfirmedEmail = false)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddRoles<AppRole>()
.AddDefaultTokenProviders(); // Bu satýr token üretimi için þart! 



//AutoMapper
builder.Services.AddAutoMapper(typeof(AppMapper));

//AddScoped
builder.Services.AddScoped<ILoginService, LoginService>();

//Services 
//AppUser;
builder.Services.AddTransient<IAppUserRepository,AppUserRepository>();
builder.Services.AddTransient<IAppUserService,AppUserService>();
//Email;
builder.Services.AddScoped<IEmailService, EmailService>();
//Scoped;Her bir HTTP isteðinde bir kez örnek oluþturur. (Yani EmailService nesnesi, istek boyunca hep aynýdýr.)
//Transient;Her çaðrýldýðýnda yeni bir nesne oluþturur. (Yani 1 istek içinde bile 2 farklý EmailService olabilir.)

//Expence;
builder.Services.AddTransient<IExpenceRepository, ExpenceRepository>();
builder.Services.AddTransient<IExpenceService, ExpenceService>();

//ExpenceType;
builder.Services.AddTransient<IExpenceTypeRepository, ExpenceTypeRepository>();
builder.Services.AddTransient<IExpenceTypeService, ExpenceTypeService>();

//SalaryAdvance
builder.Services.AddTransient<ISalaryAdvaceRepository, SalaryAdvanceRepository>();
builder.Services.AddTransient<ISalaryAdvanceService, SalaryAdvanceService>();
//LeaveRecord;
builder.Services.AddTransient<ILeaveRecordRepository, LeaveRecordRepository>();
builder.Services.AddTransient<ILeaveRecordService, LeaveRecordService>();

//LeaveRecordType;
builder.Services.AddTransient<ILeaveRecordTypeRepository, LeaveRecordTypeRepository>();
builder.Services.AddTransient<ILeaveRecordTypeService, LeaveRecordTypeService>();

//CompanyManager;
builder.Services.AddTransient<ICompanyManagerRepository, CompanyManagerRepository>();
builder.Services.AddTransient<ICompanyManagerService, CompanyManagerService>();

//Company;
builder.Services.AddTransient<ICompanyRepository, CompanyRepository>();
builder.Services.AddTransient<ICompanyService, CompanyService>();



builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<ILoginService, LoginService>();



////JWT*
//// appsettings.json dosyasýndan "JwtSettings" kýsmýný çeker
//var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//// Bu kýsýmdan secretKey (gizli anahtar) alýnýr. Bu anahtar token oluþtururken ve doðrularken kullanýlýr.
//var secretKey = jwtSettings["secretKey"];

//// Uygulamaya Authentication (Kimlik Doðrulama) servisini ekler
//builder.Services.AddAuthentication(opt =>
//{
//    // Varsayýlan olarak JWT Bearer Authentication kullanýlacaðýný belirtir
//    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options => {
//    // Token'ýn middleware'de saklanmasýna izin verir
//    options.SaveToken = true;
//    // HTTPS zorunluluðunu devre dýþý býrakýr (geliþtirme ortamý için uygundur, production için true yapýlmalýdýr)
//    options.RequireHttpsMetadata = false;
//    // Token’ýn nasýl doðrulanacaðýný belirleyen ayarlar
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        // Token içinde belirtilen 'issuer' (daðýtýcý) bilgisinin doðrulanýp doðrulanmayacaðýný belirtir (false = kontrol edilmez)
//        ValidateIssuer = false,
//        // Token içinde belirtilen 'audience' (hedef kitle) bilgisinin doðrulanýp doðrulanmayacaðýný belirtir (false = kontrol edilmez)
//        ValidateAudience = false,
//        // Token'ýn imzasýný kontrol etmek için kullanýlacak güvenlik anahtarý
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
//    };
//});



// JWT Ayarlarý
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["secretKey"];
var issuer = jwtSettings["issuer"];
var audience = jwtSettings["audience"];

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true; // Production'da MUTLAKA true olmalý

    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "Role",
        ValidateIssuer = true, // ?? Issuer kontrolü aktif // JWT sorununu çözmek için ekledim
        ValidateAudience = true, // ?? Audience kontrolü aktif // JWT sorununu çözmek için ekledim
        ValidateLifetime = true, // ?? Token süresi kontrolü
        ClockSkew = TimeSpan.Zero, // ?? Süre toleransýný sýfýrla

        ValidIssuer = issuer, // JWT sorununu çözmek için ekledim
        ValidAudience = audience, // JWT sorununu çözmek için ekledim
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };

    // JWT sorununu çözmek için ekledim
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            // Ýstersen burada kullanýcý bilgileriyle ekstra kontroller yapabilirsin
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Session servisini ekleyin (builder.Services kýsmýna)
builder.Services.AddDistributedMemoryCache(); // Memory cache kullanýmý
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout süresi
    options.Cookie.HttpOnly = true; // XSS korumasý
    options.Cookie.IsEssential = true; // GDPR uyumu
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting(); // JWT sorununu çözmek için ekledim

app.UseCors("AllowAll");

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowMVC", policy =>
//        policy.WithOrigins("https://localhost:[MVC_PORT]"));
//});


app.Run();
