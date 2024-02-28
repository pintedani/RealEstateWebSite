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
using ScheduledTasks;
using Crosscutting;
using Imobiliare.UI.Utils;
using Imobiliare.UI.ScheduledTasks;
using Imobiliare.UI.ScheduledTasks.Jobs;
using Microsoft.Data.Sqlite;


var builder = WebApplication.CreateBuilder(args);

ConnectionStrings.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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
builder.Services.AddSingleton<IEnvironmentService, EnvironmentService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<AnunturiAdaugateCheckerJob>();
builder.Services.AddScoped<AnunturiExpirateCheckerJob>();
builder.Services.AddScoped<TrimitereRapoarteAdminJob>();

builder.Services.AddScoped<IShoppingCart, ShoppingCart>(sp => ShoppingCart.GetCart(sp));
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

//https://stackoverflow.com/questions/72060349/form-field-is-required-even-if-not-defined-so
builder.Services.AddControllersWithViews(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}
    ).AddRazorRuntimeCompilation()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddRazorPages();

//For SQLServer - need new migration files
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    options.UseSqlServer(
//        builder.Configuration["ConnectionStrings:DefaultConnection"]);
//});

//For SQLite - need new migration files
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});
SetCaseInsensitiveSQLiteDb(builder);

//builder.Services.AddDefaultIdentity<UserProfile>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<UserProfile, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/LogOff";
    });

//Enable when ready
//var path = GetRequiredService<IWebHostEnvironment>();
//builder.Services.AddSingleton<HostingConfiguration>();
builder.Services.AddHostedService<ScheduledTasksBackgroundService>();

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

//My custom file logger exception
app.UseGlobalExceptionHandler();

app.MapDefaultControllerRoute();

app.MapRazorPages();
DBInitializer.Seed(app);

app.Run();





// ----------------------- Helper methods -------------------------

static void SetCaseInsensitiveSQLiteDb(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"]; // Replace with your actual connection string
    var connection = new SqliteConnection(connectionString);

    connection.Open();

    // Set SQLite to be case-insensitive
    using (var command = connection.CreateCommand())
    {
        command.CommandText = "PRAGMA case_sensitive_like = OFF";
        command.ExecuteNonQuery();
    }
}