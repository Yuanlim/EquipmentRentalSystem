using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using RentalSystem.Api.Infrastructure;
using RentalSystem.Infrastructure.Identity;

namespace RentalSystem.Api.Endpoints;

public class Users : IEndpointGroup
{
    public static void Map(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapIdentityApi<ApplicationUser>();

        routeGroupBuilder.MapPost("logout", Logout);
    }

    [EndpointSummary("Log out")]
    [EndpointDescription("Logs out user by clearing authentication cookie.")]
    public static async Task<Ok> Logout(SignInManager<ApplicationUser> signInManager)
    {
        await signInManager.SignOutAsync();
        return TypedResults.Ok();
    }
}