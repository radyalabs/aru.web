using Trisatech.MWorkforce.Api;
using Trisatech.MWorkforce.Api.Model;
using Trisatech.MWorkforce.Business;
using Trisatech.MWorkforce.Infrastructure;
using Trisatech.MWorkforce.Persistence.PostgreSQL;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using Trisatech.MWorkforce.Domain;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<ApplicationSetting>(builder.Configuration.GetSection("ApplicationSetting"));
builder.Services.Configure<TwilioSetting>(builder.Configuration.GetSection("TwilioSetting"));

builder.Services.AddDatabasePostgreSQL(builder.Configuration, "DefaultConnection");

builder.Services.AddServiceApiBusiness();
builder.Services.AddServiceApi();

builder.Services.AddGoogleMapApi(builder.Configuration);
builder.Services.AddAzureStorageAccount(builder.Configuration);

builder.Services.AddDataProtection()
    .PersistKeysToDbContext<MobileForceContext>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddMvc().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
