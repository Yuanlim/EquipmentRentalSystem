using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RentalSystem.Domain.Constants;
using RentalSystem.Domain.Entities;
using RentalSystem.Infrastructure.Identity;

namespace RentalSystem.Infrastructure.Data;

public static class InitializerExtensions
{
    public static async Task InitializerDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<DbInitializer>();

        await initializer.InitializeAsync();
        await initializer.SeedAsync();
    }
}


public class DbInitializer
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<DbInitializer> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DbInitializer(ILogger<DbInitializer> logger, AppDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        try
        {
            // await _dbContext.Database.EnsureDeletedAsync();
            // await _dbContext.Database.EnsureCreatedAsync();

            // await _dbContext.Database.MigrateAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occur while initializing database.");
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // role seeding
        var administratorRole = new IdentityRole(Roles.Administrator);
        var technicianRole = new IdentityRole(Roles.Technician);
        var staffRole = new IdentityRole(Roles.Staff);
        var customerRole = new IdentityRole(Roles.Customer);

        await RoleSeeding(administratorRole);
        await RoleSeeding(technicianRole);
        await RoleSeeding(staffRole);
        await RoleSeeding(customerRole);

        // user seeding -> create user based on this role
        IList<ApplicationUser> adminUsers = [
            new()
            {
                DisplayUserName = "AdministratorJohn",
                UserName = "AdministratorJohnAtTomRental@gmail.com",
                Email = "AdministratorJohnAtTomRental@gmail.com"
            }
        ];

        IList<ApplicationUser> technicianUsers = [
            new()
            {
                DisplayUserName = "TechnicianMichael",
                UserName = "TechnicianMichaelAtTomRental@gmail.com",
                Email = "TechnicianMichaelAtTomRental@gmail.com"
            },
            new()
            {
                DisplayUserName = "TechnicianDavid",
                UserName = "TechnicianDavidAtTomRental@gmail.com",
                Email = "TechnicianDavidAtTomRental@gmail.com",
            }
        ];

        IList<ApplicationUser> staffUsers = [
            new()
            {
                DisplayUserName = "StaffJames",
                UserName = "StaffJamesAtTomRental@gmail.com",
                Email = "StaffJamesAtTomRental@gmail.com"
            },
            new() {
                DisplayUserName = "StaffSarah",
                UserName = "StaffSarahAtTomRental@gmail.com",
                Email = "StaffSarahAtTomRental@gmail.com"
            }
        ];

        IList<ApplicationUser> customerUsers = [
            new()
            {
                DisplayUserName = "Robert",
                UserName = "RobertHelloWorld@gmail.com",
                Email = "RobertHelloWorld@gmail.com"
            },
            new()
            {
                DisplayUserName = "Alex",
                UserName = "AlexHelloWorld@gmail.com",
                Email = "AlexHelloWorld@gmail.com"
            },
            new()
            {
                DisplayUserName = "Jenny",
                UserName = "JennyHelloWorld@gmail.com",
                Email = "JennyHelloWorld@gmail.com"
            }
        ];

        await UserSeeding(administratorRole, adminUsers);
        await UserSeeding(technicianRole, technicianUsers);
        await UserSeeding(staffRole, staffUsers);
        await UserSeeding(customerRole, customerUsers);

        // Category
        var equipmentCategories = new List<(string Name, string Prefix)>
        {
            ("Camera", "CAM"),
            ("Lens", "LEN"),
            ("Tripod", "TRI"),
            ("Laptop", "LAP"),
            ("Projector", "PJT"),
            ("Speaker", "SPK"),
            ("Microphone", "MCP"),
            ("Lighting Equipment", "LTE"),
            ("Hard Disk", "HDD"),
            ("Monitor", "MN"),
            ("Drone", "DR"),
            ("Construction Tool", "CST")
        };

        await CategorySeeding(equipmentCategories);
    }

    /// <summary>
    /// Initial roles to database 
    /// </summary>
    /// <param name="identityRole"></param>
    /// <param name="users"></param>
    /// <returns></returns>
    public async Task RoleSeeding(IdentityRole identityRole)
    {
        if (_roleManager.Roles.All(r => r.Name != identityRole.Name))
        {
            var result = await _roleManager.CreateAsync(identityRole);

            if (!result.Succeeded)
            {
                throw new(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }

    /// <summary>
    /// Initial some user to database with different role
    /// </summary>
    /// <param name="identityRole"></param>
    /// <param name="users"></param>
    /// <returns></returns>
    public async Task UserSeeding(IdentityRole identityRole, IList<ApplicationUser> users)
    {
        for (var i = 0; i < users.Count; i++)
        {
            var existedUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == users[i].Email);

            if (existedUser is null)
            {
                var result = await _userManager.CreateAsync(users[i], $"{identityRole.Name}{i}@12345");

                if (!result.Succeeded)
                {
                    throw new(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                if (!string.IsNullOrWhiteSpace(identityRole.Name))
                {
                    await _userManager.AddToRoleAsync(users[i], identityRole.Name);
                }
            }
        }
    }

    /// <summary>
    /// Seed initial database with some categories
    /// </summary>
    /// <param name="categories"></param>
    /// <returns></returns>
    public async Task CategorySeeding(IList<(string Name, string Prefix)> categories)
    {
        foreach (var (Name, Prefix) in categories)
        {
            var equipmentCategory = new EquipmentCategory(Name, null, Prefix);

            // Get category
            var existedEquipmentCategory =
                await _dbContext.EquipmentCategories.FirstOrDefaultAsync(ec =>
                    ec.NormalizedName == equipmentCategory.NormalizedName
                );

            if (existedEquipmentCategory is null)
            {
                await _dbContext.EquipmentCategories.AddAsync(equipmentCategory);
            }
            else if (string.IsNullOrWhiteSpace(existedEquipmentCategory.AssetTagPrefix))
            {
                existedEquipmentCategory.SetAssetTagPrefix(Prefix);
            }
        }

        await _dbContext.SaveChangesAsync();
    }
}