using Microsoft.EntityFrameworkCore;
using PriceGuard.Api.Data;
using PriceGuard.Api.Features.Stores;
using Scalar.AspNetCore;
using PriceGuard.Api.Features.PriceChecks;
using PriceGuard.Api.Scraping;
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration
    .GetConnectionString("PriceGuardDb")
    ?? throw new InvalidOperationException(
        "Connection string 'PriceGuardDb' was not found.");

builder.Services.AddDbContext<PriceGuardDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IPriceSource, FakeStorePriceSource>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // API sözleşmesini JSON olarak yayınlar:
    // /openapi/v1.json
    app.MapOpenApi();

    // OpenAPI sözleşmesini interaktif bir arayüzde gösterir:
    // /scalar
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("PriceGuard API")
            .DisableAgent();
    });
}

app.UseHttpsRedirection();
app.MapStoreEndpoints();
app.MapPriceCheckEndpoints();

app.Run();