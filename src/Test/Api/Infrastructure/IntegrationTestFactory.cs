using Testcontainers.PostgreSql;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using RentalSystem.Test.Api.Infrastructure;

/// <summary>
/// <see href="https://kloudshift.net/blog/asp-net-core-integration-tests-with-test-containers-and-postgres/">
/// </summary>
/// <typeparam name="TProgram"></typeparam>
/// <typeparam name="TDbContext"></typeparam>
public class IntegrationTestFactory<TProgram, TDbContext>
    : WebApplicationFactory<TProgram>, IAsyncLifetime
    where TProgram : class where TDbContext : DbContext
{
    private readonly PostgreSqlContainer _container;

    [Obsolete]
    public IntegrationTestFactory()
    {
        _container = new PostgreSqlBuilder()
            .WithDatabase("test_db")
            .WithImage("postgres:16")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .WithCleanUp(true)
            .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveDbContext<TDbContext>();
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseNpgsql(_container.GetConnectionString());
            });
            services.EnsureDbReset<TDbContext>();
        });
    }

    public async Task InitializeAsync() => await _container.StartAsync();

    public new async Task DisposeAsync() => await _container.DisposeAsync();
}