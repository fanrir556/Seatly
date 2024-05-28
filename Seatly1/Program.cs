using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Seatly1.Data;
using Seatly1.Models;
using Seatly1.Controllers;
using Microsoft.AspNetCore.Identity.UI.Services;
using Seatly1.Areas.Identity;

var builder = WebApplication.CreateBuilder(args);

// Google 第三方登入
var services = builder.Services;
var configuration = builder.Configuration;

services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "546918205472-3p0reoblv7q9pcuqqftkl19ao5ufhd0h.apps.googleusercontent.com";
    googleOptions.ClientSecret = "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";
});

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<SeatlyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Seatly")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// 註冊發送驗證信
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddControllersWithViews();


// ���UDI�e��
builder.Services.AddDbContext<SeatlyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Seatly"));
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

//Session
builder.Services.AddSession(option =>
{
    option.Cookie.Name = ".Seatly.Session";
    option.IdleTimeout = TimeSpan.FromMinutes(30); //逾時時間
    option.Cookie.IsEssential = true; //必要
    option.Cookie.HttpOnly = true; //.NET6以後才有，防範document.cookie盜取資料
    option.Cookie.SecurePolicy = CookieSecurePolicy.Always; //要求Cookie必須透過HTTPS傳送
});

// 配置 防偽標籤 Antiforgery ~ 用於防止 CSRF 攻擊
builder.Services.AddAntiforgery(options =>
{
    // 指定在 HTML 表單中生成的隱藏欄位的名稱，該欄位將包含防偽標籤值。 <== 默認名稱可能會被猜出來
    options.FormFieldName = "__Antiforgery__Queuely";
    // 防偽標籤名稱
    options.HeaderName = "X-CSRF-TOKEN";
    // 防止 X-Frame-Options 標頭被禁用 <== 網頁不能被攻擊者嵌入到其他網站的 iframe 中
    options.SuppressXFrameOptionsHeader = false;
});

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

//CORS
string PolicyName = "AllowAny";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: PolicyName, policy =>
    {
        policy.WithOrigins("*").WithHeaders("*").WithMethods("*");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
};

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//Session
app.UseSession();

// 啟用 防偽標籤 cookie 服務
app.UseAntiforgery();

//CORS
app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "OrganizerRegister",
    pattern: "{controller=OrganizerRoute}/{action=OrganizerRegister}/{id?}");

app.MapControllerRoute(
    name: "OrganizerLogin",
    pattern: "{controller=OrganizerRoute}/{action=OrganizerLogin}/{id?}");

app.MapControllerRoute(
    name: "OrganizerInfo",
    pattern: "{controller=OrganizerRoute}/{action=OrganizerInfo}/{id?}");

app.MapControllerRoute(
    name: "Index",
    pattern: "{controller=OrganizerRoute}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "NotificationRecord",
    pattern: "{controller=OrganizerRoute}/{action=NotificationRecord}/{id?}");

app.MapControllerRoute(
    name: "ActivityCreate",
    pattern: "{controller=OrganizerRoute}/{action=ActivityCreate}/{id?}");

app.MapControllerRoute(
    name: "ActivityEdit",
    pattern: "{controller=OrganizerRoute}/{action=ActivityEdit}/{id?}");

app.MapControllerRoute(
    name: "Activity",
    pattern: "{controller=OrganizerRoute}/{action=Activity}/{id?}");

app.MapRazorPages();

app.Run();
