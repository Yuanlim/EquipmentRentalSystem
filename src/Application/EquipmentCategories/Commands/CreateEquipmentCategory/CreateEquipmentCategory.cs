using MediatR;
using RentalSystem.Application.Common.Interfaces;
using RentalSystem.Domain.Entities;

namespace RentalSystem.Application.EquipmentCategories.Commands.CreateEquipmentCategory;

/// <summary>
/// Request obj for create equipment category
/// </summary>
public record CreateEquipmentCategoryCommand : IRequest<Guid>
{
    public string CategoryName { get; init; } = string.Empty;

    public string AssetTagPrefix { get; init; } = string.Empty;
}

/// <summary>
/// Handler for create equipment category Api.
/// </summary>
public class CreateEquipmentCategoryHandler : IRequestHandler<CreateEquipmentCategoryCommand, Guid>
{
    // for current user
    private readonly IUser _user;

    private readonly IRentalSystemDbContext _dbContext;

    public CreateEquipmentCategoryHandler(
        IUser user,
        IRentalSystemDbContext dbContext
    )
    {
        _user = user;
        _dbContext = dbContext;
    }

    public async Task<Guid> Handle(CreateEquipmentCategoryCommand command, CancellationToken cancellationToken)
    {
        var equipmentCategory = new EquipmentCategory(
            name: command.CategoryName,
            createdByUserId: _user.Id, // Get current user id
            assetTagPrefix: command.AssetTagPrefix
        );

        await _dbContext.EquipmentCategories.AddAsync(equipmentCategory, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return equipmentCategory.Id;
    }
}