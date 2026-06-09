using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace RentalSystem.Api.Infrastructure;

/// <see cref="https://learn.openapis.org/specification/paths.html#the-operation-object"/>
// What it means is each "API documentation" was translate as "operation" by OpenApi,
// to interfere with the documentation we need to implement transformer that can configure
// as an OpenAPI transformer.
// To satisfy it is a "OpenApi transformer", we need to satisfy IOpenApiOperationTransformer
// contract implement method TransformAsync. So each operation can goes through each
// transformer and influence their documentation.

/// <summary>
/// Adds standard error responses to every OpenAPI operation. A 400 Bad Request is added to all
/// operations because every request passes through <c>ValidationBehaviour</c> in the MediatR
/// pipeline. 401 Unauthorized and 403 Forbidden are added only to operations that carry
/// <see cref="IAuthorizeData"/> metadata.
/// </summary>
internal sealed class ApiExceptionOperationTransformer : IOpenApiOperationTransformer
{
    public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
    {
        operation.Responses ??= [];
        operation.Responses.TryAdd("400", new OpenApiResponse { Description = "Bad Request" });

        var requiresAuth = context.Description.ActionDescriptor.EndpointMetadata
            .Any(m => m is IAuthorizeData);

        if (requiresAuth)
        {
            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });
        }

        return Task.CompletedTask;
    }
}