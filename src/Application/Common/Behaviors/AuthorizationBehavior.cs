using System.Reflection;
using MediatR;
using RentalSystem.Application.Common.Exceptions;
using RentalSystem.Application.Common.Interfaces;
using RentalSystem.Application.Common.Security;

namespace RentalSystem.Application.Common.Behaviors;

/// <summary>
/// Check wether the user permission suffice the endpoint requirement.
/// As a part of the pipeline
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUser _user;

    private readonly IIdentityService _identityService;

    public AuthorizationBehavior(IUser user, IIdentityService identityService)
    {
        _user = user;
        _identityService = identityService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
        {
            if (_user.Id == null)
            {
                throw new UnauthorizedAccessException();
            }

            var authorizeRoles = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Roles));

            if (authorizeRoles.Any())
            {
                var authorized = false;

                foreach (var roles in authorizeRoles.Select(a => a.Roles.Split(',')))
                {
                    foreach (var r in roles)
                    {
                        var isInRole = _user.Roles?.Any(x => r == x) ?? false;

                        if (isInRole)
                        {
                            authorized = true;
                            break;
                        }
                    }
                }

                if (!authorized) throw new ForbiddenAccessException();
            }

            var authorizePolicy = authorizeAttributes.Where(a => !string.IsNullOrWhiteSpace(a.Policy));

            if (authorizePolicy.Any())
            {
                foreach (var policies in authorizePolicy.Select(a => a.Policy))
                {
                    var authorized = await _identityService.AuthorizeAsync(_user.Id, policies);

                    if (!authorized) throw new ForbiddenAccessException();
                }
            }
        }

        return await next(cancellationToken);
    }
}