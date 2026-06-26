using RentalSystem.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace RentalSystem.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public EmployeeStatus? EmployeeStatus { get; set; }
    public bool? isBanned;

    // Username as email which is very weird, same data storing
    // For login it use Username, but it also means email can be absences
    // use this to store real username
    public string DisplayUserName { get; set; } = "";
}