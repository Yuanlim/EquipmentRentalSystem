using System.Reflection;

namespace RentalSystem.Api.Infrastructure;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Discovers all <see cref="IEndpointGroup"/> implementations in <paramref name="assembly"/>
    /// and registers each as a route group with a matching OpenAPI tag. The route prefix defaults
    /// to <c>/api/{ClassName}</c> but can be overridden via <see cref="IEndpointGroup.RoutePrefix"/>.
    /// </summary>
    public static WebApplication MapEndpoints(this WebApplication app, Assembly assembly)
    {
        // Goes through assembly getting each things metadata that matches who is implemented
        // with IEndpointGroup.

        // IsAssignableTo: current variable can be convert to target type or not
        var endpointGroupTypes = assembly.GetExportedTypes()
                                        .Where(
                                            type => type.IsAssignableTo(typeof(IEndpointGroup))
                                                    && !type.IsInterface // exclude interface
                                                    && !type.IsAbstract // exclude incomplete
                                        );


        foreach (var type in endpointGroupTypes)
        {
            var groupName = type.Name;  // Access class name
            var routePrefix = type.GetProperty(nameof(IEndpointGroup.RoutePrefix))?
                                    .GetValue(null) // get the values but default to null when not given
                                    as string // Convert as string
                                    ?? $"/api/{groupName}"; // if null default to /api/{groupName}
            var group = app.MapGroup(routePrefix).WithTags(groupName);
            type.GetMethod(nameof(IEndpointGroup.Map))!.Invoke(null, [group]);
        }

        return app;
    }
}