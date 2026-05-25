using RentalSystem.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace RentalSystem.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public EmployeeStatus? EmployeeStatus { get; set; }
    public bool? isBanned;
}