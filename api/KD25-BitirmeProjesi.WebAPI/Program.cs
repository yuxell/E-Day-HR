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
.AddDefaultTokenProviders(); // Bu sat�r token �retimi i�in �art! 



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
//Scoped;Her bir HTTP iste�inde bir kez �rnek olu�turur. (Yani EmailService nesnesi, istek boyunca hep ayn�d�r.)
//Transient;Her �a�r�ld���nda yeni bir nesne olu�turur. (Yani 1 istek i�inde bile 2 farkl� EmailService olabilir.)

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
//// appsettings.json dosyas�ndan "JwtSettings" k�sm�n� �eker
//var jwtSettings = builder.Configuration.GetSection("JwtSettings");
//// Bu k�s�mdan secretKey (gizli anahtar) al�n�r. Bu anahtar token olu�tururken ve do�rularken kullan�l�r.
//var secretKey = jwtSettings["secretKey"];

//// Uygulamaya Authentication (Kimlik Do�rulama) servisini ekler
//builder.Services.AddAuthentication(opt =>
//{
//    // Varsay�lan olarak JWT Bearer Authentication kullan�laca��n� belirtir
//    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(options => {
//    // Token'�n middleware'de saklanmas�na izin verir
//    options.SaveToken = true;
//    // HTTPS zorunlulu�unu devre d��� b�rak�r (geli�tirme ortam� i�in uygundur, production i�in true yap�lmal�d�r)
//    options.RequireHttpsMetadata = false;
//    // Token��n nas�l do�rulanaca��n� belirleyen ayarlar
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        // Token i�inde belirtilen 'issuer' (da��t�c�) bilgisinin do�rulan�p do�rulanmayaca��n� belirtir (false = kontrol edilmez)
//        ValidateIssuer = false,
//        // Token i�inde belirtilen 'audience' (hedef kitle) bilgisinin do�rulan�p do�rulanmayaca��n� belirtir (false = kontrol edilmez)
//        ValidateAudience = false,
//        // Token'�n imzas�n� kontrol etmek i�in kullan�lacak g�venlik anahtar�
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
//    };
//});



// JWT Ayarlar�
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
    options.RequireHttpsMetadata = true; // Production'da MUTLAKA true olmal�

    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "Role",
        ValidateIssuer = true, // ?? Issuer kontrol� aktif // JWT sorununu ��zmek i�in ekledim
        ValidateAudience = true, // ?? Audience kontrol� aktif // JWT sorununu ��zmek i�in ekledim
        ValidateLifetime = true, // ?? Token s�resi kontrol�
        ClockSkew = TimeSpan.Zero, // ?? S�re tolerans�n� s�f�rla

        ValidIssuer = issuer, // JWT sorununu ��zmek i�in ekledim
        ValidAudience = audience, // JWT sorununu ��zmek i�in ekledim
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };

    // JWT sorununu ��zmek i�in ekledim
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
            // �stersen burada kullan�c� bilgileriyle ekstra kontroller yapabilirsin
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

// Session servisini ekleyin (builder.Services k�sm�na)
builder.Services.AddDistributedMemoryCache(); // Memory cache kullan�m�
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout s�resi
    options.Cookie.HttpOnly = true; // XSS korumas�
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

app.UseRouting(); // JWT sorununu ��zmek i�in ekledim

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
