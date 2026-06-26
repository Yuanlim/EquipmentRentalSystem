using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RentalSystem.Infrastructure.Data;
using RentalSystem.Infrastructure.Identity;
using Xunit;

namespace RentalSystem.Test.Api.Users;

/// <summary>
/// <see href="https://kloudshift.net/blog/asp-net-core-integration-tests-with-test-containers-and-postgres/">
/// Login tests using WebApplicationFactory + TestContainer.
/// </summary>
public class LoginTests :
    IClassFixture<IntegrationTestFactory<Program, AppDbContext>>
{
    private readonly IntegrationTestFactory<Program, AppDbContext> _factory;

    public LoginTests(IntegrationTestFactory<Program, AppDbContext> factory)
    => _factory = factory;

    private sealed class LoginResponse
    {
        public string TokenType { get; set; } = "";
        public string AccessToken { get; set; } = "";
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; } = "";
    }

    /// <summary>
    /// Admin login but is success
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task Post_LoginAsAdmin_Ok()
    {
        using var scope = _factory.Services.CreateScope();

        var userManager = scope.ServiceProvider
                                .GetRequiredService<UserManager<ApplicationUser>>();

        var admin = await userManager.FindByEmailAsync("AdministratorTestAtTomRental@gmail.com");

        Assert.NotNull(admin);

        var passwordOk = await userManager.CheckPasswordAsync(
            admin,
            "Administrator0@12345"
        );

        Assert.True(passwordOk);
        Assert.True(admin.EmailConfirmed);
        Assert.False(admin.LockoutEnd.HasValue && admin.LockoutEnd > DateTimeOffset.UtcNow);

        // Create simulation instance
        var client = _factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/Users/login",
            new
            {
                email = "AdministratorTestAtTomRental@gmail.com",
                password = "Administrator0@12345"
            }
        );

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Deserialized
        var responseJson = await response.Content.ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(responseJson);
        Assert.False(string.IsNullOrWhiteSpace(responseJson.AccessToken));
    }
}