using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using RentalSystem.Api.Infrastructure;
using RentalSystem.Application.EquipmentItems.Commands.CreateEquipmentItem;
using RentalSystem.Application.EquipmentItems.Queries.GetEquipmentItem;
using RentalSystem.Domain.Constants;

namespace RentalSystem.Api.Endpoints;

public class EquipmentItems : IEndpointGroup
{
    public static void Map(RouteGroupBuilder routeGroupBuilder)
    {
        routeGroupBuilder.MapPost(CreateEquipmentItem)
                        .RequireAuthorization(policy => policy.RequireRole(Roles.Administrator));

        routeGroupBuilder.MapGet(GetEquipmentItem)
                            .RequireAuthorization(policy =>
                            {
                                policy.RequireRole(Roles.Customer, Roles.Administrator);
                            });
    }

    [EndpointSummary("Create equipment item")]
    [EndpointDescription("Required role: Admin. Create equipment item by the request detail provided and returns the id after created.")]
    public static async Task<Created<Guid>> CreateEquipmentItem(CreateEquipmentItemCommand command, ISender sender, CancellationToken cancellationToken)
    {
        var id = await sender.Send(command, cancellationToken);

        return TypedResults.Created("/api/EquipmentItems/", id);
    }

    [EndpointSummary("Get equipment item")]
    [EndpointDescription("Required role: Admin, Customer. Get equipment item by the detail given and return equipment item dto.")]
    public static async Task<Ok<List<EquipmentItemDto>>> GetEquipmentItem(
        [AsParameters] GetEquipmentItemQuery query,
        ISender sender,
        CancellationToken cancellationToken
    )
    {
        var equipmentItems = await sender.Send(query, cancellationToken);

        return TypedResults.Ok(equipmentItems);
    }
}