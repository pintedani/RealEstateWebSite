using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using Imobiliare.Repositories;
using Imobiliare.Entities;
using Imobiliare.UI.Models;
using Imobiliare.ServiceLayer.EmailService;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.ServiceLayer.Interfaces;
using Imobiliare.ServiceLayer.ExternalSiteContentParser;
using Imobiliare.ServiceLayer;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

//builder.Services.AddScoped<IImobilRepository, MockImobilRepository>();

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IAnunturiRepository, AnunturiRepository>();
builder.Services.AddScoped<IJudetRepository, JudetRepository>();
builder.Services.AddScoped<IOrasRepository, OrasRepository>();
builder.Services.AddScoped<ICartierRepository, CartierRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<ILogsRepository, LogsRepository>();
builder.Services.AddScoped<ISystemSettingsRepository, SystemSettingsRepository>();
builder.Services.AddScoped<IStiriRepository, StiriRepository>();
builder.Services.AddScoped<IAgentiiRepository, AgentiiRepository>();
builder.Services.AddScoped<IConstructoriRepository, ConstructoriRepository>();
builder.Services.AddScoped<IMesajRepository, MesajRepository>();
builder.Services.AddScoped<IAnsambluriRepository, AnsambluriRepository>();
builder.Services.AddScoped<IAuditTrailRepository, AuditTrailRepository>();
builder.Services.AddScoped<IEmailManagerService, EmailManagerService>();
builder.Services.AddScoped<IExternalSiteParserService, ExternalSiteParserService>();
builder.Services.AddScoped<IRecaptchaValidator, RecaptchaValidator>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration["ConnectionStrings:DefaultConnection"]);
});

//builder.Services.AddDefaultIdentity<UserProfile>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<UserProfile, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/LogOff";
    });
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options => {
//        options.LoginPath = "/Identity/Account/Login";
//        // Other options here
//    });


var app = builder.Build();

app.UseStaticFiles();
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapDefaultControllerRoute();

app.MapRazorPages();
DBInitializer.Seed(app);

app.Run();
