using Microsoft.AspNetCore.Identity;
using RentalSystem.Application.Common.Models;

namespace RentalSystem.Infrastructure.Identity;

public static class IdentityResultExtensions
{
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded ? Result.Success() : Result.Failure(result.Errors.Select(e => e.Description));
    }
}