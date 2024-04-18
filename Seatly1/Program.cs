using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Seatly1.Data;
using Seatly1.Models;
using Seatly1.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<SeatlyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Seatly")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// ���UDI�e��
builder.Services.AddDbContext<SeatlyContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("Seatly"));
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

//Session
builder.Services.AddSession(option =>
{
    option.Cookie.Name = ".Seatly.Session";
    option.IdleTimeout = TimeSpan.FromMinutes(15); //逾時時間
    option.Cookie.IsEssential = true; //必要
    option.Cookie.HttpOnly = true; //.NET6以後才有，防範document.cookie盜取資料
    option.Cookie.SecurePolicy = CookieSecurePolicy.Always; //要求Cookie必須透過HTTPS傳送
});

builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "OrganizerRegister",
    pattern: "{controller=Home}/{action=OrganizerRegister}/{id?}");

app.MapControllerRoute(
    name: "OrganizerLogin",
    pattern: "{controller=Home}/{action=OrganizerLogin}/{id?}");

app.MapControllerRoute(
    name: "OrganizerInfo",
    pattern: "{controller=Home}/{action=OrganizerInfo}/{id?}");

app.MapRazorPages();

app.Run();
