using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using RentalSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.TestHost;

namespace RentalSystem.Test.Api.Infrastructure;

/// <summary>
/// <see href="https://kloudshift.net/blog/asp-net-core-integration-tests-with-test-containers-and-postgres/"> This is shared database among tests. It doesn't use original db.
/// </summary>
/// <typeparam name="TProgram"></typeparam>
public class CustomWebApplicationWebFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{

    // Gives a fixture an opportunity to configure the application before it gets built.
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(async services =>
        {
            // Remove AppDbContext
            services.RemoveDbContext<AppDbContext>();

            // Add DB context pointing to test container
            services.AddDbContext<AppDbContext>(options =>
            {
                var connString = Environment.GetEnvironmentVariable("TestConnString");

                if (connString is null)
                    throw new("Test db connection string wasn't given.");

                options.UseNpgsql(connString);
            });

            services.EnsureDbReset<AppDbContext>();
        });
    }
}