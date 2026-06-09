using Microsoft.EntityFrameworkCore;
using RentalSystem.Infrastructure.Data;
using RentalSystem.Api.Infrastructure;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Identity;
using RentalSystem.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.AddInfrastructureServices();
builder.AddApplicationServices();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, NotEmailSender>();
builder.AddWebServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // Learn OpenAPI with Scalar with ASP.NET Core
    // https://medium.com/@FitoMAD/asp-net-core-openapi-with-scalar-c430051bbabf
    app.MapScalarApiReference(options =>
    {
        options.Title = "Rental System API";
        options.Theme = ScalarTheme.Kepler;
    });
    await app.InitializerDatabaseAsync();
}

app.UseHttpsRedirection();

app.MapEndpoints(typeof(Program).Assembly);

app.Run();