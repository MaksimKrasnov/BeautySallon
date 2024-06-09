using BeautySaloon.Controllers;
using BeautySaloon.Email;
using BeautySaloon.Models;
using BeautySaloon.Repositoryes.Account;
using BeautySaloon.Repositoryes.AppointmentRep;
using BeautySaloon.Repositoryes.Home;
using BeautySaloon.Repositoryes.Login;
using BeautySaloon.Repositoryes.Registration;
using BeautySaloon.Repositoryes.Search;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddScoped<IAppointmentRepository,AppointmentRepository>();
builder.Services.AddScoped<IHomeRepository, HomeRepository>();
builder.Services.AddScoped<ISearchRepository, SearchRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IRegistrationRepository, RegistrationRepository>();


builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).
	AddCookie(options =>
	{
		options.LoginPath = new PathString("/Index/Login");
	});

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/Login/Index";
	options.AccessDeniedPath = "/Login/AccessDenied";
});
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = new[] { new CultureInfo("ru-RU") };
	options.DefaultRequestCulture = new RequestCulture("ru-RU");
	options.SupportedCultures = supportedCultures;
	options.SupportedUICultures = supportedCultures;
});
var app = builder.Build();


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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();








