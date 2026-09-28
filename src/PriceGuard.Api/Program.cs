using Microsoft.EntityFrameworkCore;
using PriceGuard.Api.Data;
using PriceGuard.Api.Features.Stores;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapStoreEndpoints();

app.Run();