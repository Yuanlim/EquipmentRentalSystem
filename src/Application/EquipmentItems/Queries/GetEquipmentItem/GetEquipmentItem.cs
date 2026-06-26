using Ardalis.GuardClauses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalSystem.Application.Common.Interfaces;
using RentalSystem.Domain.Entities;
using RentalSystem.Domain.Enum.Extensions;

namespace RentalSystem.Application.EquipmentItems.Queries.GetEquipmentItem;

public record GetEquipmentItemQuery : IRequest<List<EquipmentItemDto>>
{
    public string? Description { get; init; }
    public string? EquipmentCategory { get; init; }
    public string? Name { get; init; }
    public string? Condition { get; init; }
    public string? Status { get; init; }
}

public class GetEquipmentItemHandler : IRequestHandler<GetEquipmentItemQuery, List<EquipmentItemDto>>
{
    // This service should not be reassigned after initiated.
    public readonly IRentalSystemDbContext _dbContext;

    // Constructor to DI services you needed
    public GetEquipmentItemHandler(IRentalSystemDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<EquipmentItemDto>> Handle(GetEquipmentItemQuery query, CancellationToken cancellationToken)
    {
        IQueryable<EquipmentItem> equipmentItemQuery = _dbContext.EquipmentItems.AsNoTracking();

        if (query.EquipmentCategory is not null)
        {
            var equipmentCategory = await _dbContext.EquipmentCategories.FirstOrDefaultAsync(
                ec => EF.Functions.Like(ec.Name, query.EquipmentCategory),
                cancellationToken
            );

            // Guard.Against.NotFound(SearchedValue, Result);
            Guard.Against.NotFound(query.EquipmentCategory, equipmentCategory);

            equipmentItemQuery = equipmentItemQuery.Where(ec =>
                ec.EquipmentCategory.Id == equipmentCategory.Id
            );
        }

        if (query.Condition is not null)
        {
            var equipmentItemCondition = query.Condition.ToCondition() ??
                                        throw new ArgumentException("Invalid equipment condition.");

            equipmentItemQuery = equipmentItemQuery.Where(ec =>
                ec.Condition == equipmentItemCondition
            );
        }

        if (query.Status is not null)
        {
            var equipmentItemStatus = query.Status.ToStatus() ??
                                        throw new ArgumentException("Invalid equipment status.");

            equipmentItemQuery = equipmentItemQuery.Where(ec =>
                ec.Status == equipmentItemStatus
            );
        }

        var equipmentItems = await equipmentItemQuery
            .Where(ec =>
                (string.IsNullOrWhiteSpace(query.Name) || ec.Name.Contains(query.Name)) &&
                (string.IsNullOrWhiteSpace(query.Description) || ec.Description.Contains(query.Description))
            )
            .Select(ec => ec.ToDto())
            .ToListAsync(cancellationToken);

        return equipmentItems;
    }
}