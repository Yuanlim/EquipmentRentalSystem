using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RentalSystem.Domain.Constants;
using RentalSystem.Infrastructure.Data;
using RentalSystem.Infrastructure.Identity;

namespace RentalSystem.Test.Api.Infrastructure;

public static class TestDbUtilities
{
    public static void RemoveDbContext<T>(this IServiceCollection services) where T : DbContext
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<T>));
        if (descriptor != null) services.Remove(descriptor);
    }

    public static void EnsureDbReset<T>(this IServiceCollection services)
        where T : DbContext
    {
        // Ensure schema gets created
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;

        // Get service to use for testing
        var dbContext = scopedServices.GetRequiredService<T>();

        // For role creation/deletion...
        var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

        // For user creation/deletion...
        var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();

        // Always delete database, when db existed
        // restart db
        dbContext.Database.EnsureDeleted();
        dbContext.Database.Migrate();

        TestDbUtilities.SeedAsync(roleManager, userManager)
                            .GetAwaiter().GetResult();
    }

    private static async Task SeedAsync(
        RoleManager<IdentityRole> roleManager,
        UserManager<ApplicationUser> userManager
    )
    {
        var administratorRole = new IdentityRole(Roles.Administrator);
        var customerRole = new IdentityRole(Roles.Customer);

        await RoleSeedingAsync(administratorRole, roleManager);
        await RoleSeedingAsync(customerRole, roleManager);

        IList<ApplicationUser> admin = [
            new()
            {
                UserName = "AdministratorTestAtTomRental@gmail.com",
                Email = "AdministratorTestAtTomRental@gmail.com",
                DisplayUserName = "AdministratorTest",
                EmailConfirmed = true
            }
        ];

        IList<ApplicationUser> customer = [
            new()
            {
                UserName = "CustomerTestAtTomRental@gmail.com",
                Email = "CustomerTestAtTomRental@gmail.com",
                DisplayUserName = "CustomerTest",
                EmailConfirmed = true
            }
        ];

        await UserSeedingAsync(administratorRole, admin, userManager);
        await UserSeedingAsync(customerRole, customer, userManager);
    }

    private static async Task RoleSeedingAsync(
        IdentityRole identityRole,
        RoleManager<IdentityRole> roleManager
    )
    {
        if (roleManager.Roles.All(r => r.Name != identityRole.Name))
        {
            var result = await roleManager.CreateAsync(identityRole);

            if (!result.Succeeded)
            {
                throw new(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }

    private static async Task UserSeedingAsync(
        IdentityRole identityRole,
        IList<ApplicationUser> users,
        UserManager<ApplicationUser> userManager
    )
    {
        for (var i = 0; i < users.Count; i++)
        {
            var existedUser = await userManager.Users.FirstOrDefaultAsync(u => u.Email == users[i].Email);

            if (existedUser is null)
            {
                var result = await userManager.CreateAsync(users[i], $"{identityRole.Name}{i}@12345");

                if (!result.Succeeded)
                {
                    throw new(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                if (!string.IsNullOrWhiteSpace(identityRole.Name))
                {
                    await userManager.AddToRoleAsync(users[i], identityRole.Name);
                }
            }
        }
    }
}