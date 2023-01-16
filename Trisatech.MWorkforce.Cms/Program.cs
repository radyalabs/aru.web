using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Trisatech.MWorkforce.Business;
using Trisatech.MWorkforce.Cms;
using Trisatech.MWorkforce.Cms.Models;
using Trisatech.MWorkforce.Domain;
using Trisatech.MWorkforce.Infrastructure;
using Trisatech.MWorkforce.Persistence.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ApplicationSetting>(builder.Configuration.GetSection("ApplicationSetting"));

builder.Services.AddDatabasePostgreSQL(builder.Configuration, "DefaultConnection");

builder.Services.AddServiceCmsBusiness();

builder.Services.AddServiceCms();

builder.Services.AddTextReader();

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<MobileForceContext>()
    .SetApplicationName(".trisatech.Workforce.app");

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
            });

builder.Services.AddAuthorization();

builder.Services.AddMvc().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.DateFormatString = "dd/MM/yyyy";
    options.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddControllersWithViews()
.AddRazorRuntimeCompilation();

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
