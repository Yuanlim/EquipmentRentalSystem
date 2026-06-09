using RentalSystem.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using RentalSystem.Application.Common.Interfaces;

namespace RentalSystem.Infrastructure.Identity;

/// <summary>
/// The whole point about this class, is that any handler request that requires
/// user based checks/creation/delete/update the handler can DI the interface.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

    private readonly IAuthorizationService _authorizationService;

    public IdentityService(
        // Managing user
        UserManager<ApplicationUser> userManager,
        // Claim principal is the information about a person, currently requesting.
        // Factory is creation handler.
        // IUserClaimsPrincipalFactory helps create ApplicationUser ClaimPrincipal
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        // Check user authorization permission
        IAuthorizationService authorizationService
    )
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user?.UserName;
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);

        return user is not null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        // Get user
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
        {
            return false;
        }

        // Create principal by user
        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(string email, string password)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) return Result.Failure(["No such user."]);

        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

}