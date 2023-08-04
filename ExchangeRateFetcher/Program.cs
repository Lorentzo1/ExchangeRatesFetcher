using ExchangeRateWebApi.Models;
using ExchangeRateWebApi.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
builder.Services.AddHostedService<ScopedBackgroundService>();
builder.Services.AddMemoryCache();
builder.Services.Configure<AddressOptions>(builder.Configuration.GetSection(AddressOptions.Address));
builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();


var cultureToUse = new CultureInfo("cs-CZ");
CultureInfo.DefaultThreadCurrentCulture = cultureToUse;

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
