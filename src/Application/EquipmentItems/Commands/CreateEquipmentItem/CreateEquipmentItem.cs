using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RentalSystem.Application.Common.Interfaces;
using RentalSystem.Domain.Entities;
using RentalSystem.Domain.Enum;
using RentalSystem.Domain.Enum.Extensions;

// https://github.com/LuckyPennySoftware/MediatR/wiki#:~:text=IRequest%3CTResponse%3E%20%2D%20the%20request%20returns%20a%20value
// IRequest<int>: the request return ids(Guid) of the created item

// https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/records#when-to-use-records
// Record immutability: use record because in no means you should ever mutate user request.
// Request as Record for pure data

namespace RentalSystem.Application.EquipmentItems.Commands.CreateEquipmentItem;

/// <summary>
/// Condition and status is nullable, because my designing thought every new equipment should be not 
/// rented and new from the start.
/// </summary>
public record CreateEquipmentItemCommand : IRequest<Guid>
{
    public decimal DepositFee { get; init; }
    public decimal FeePerDay { get; init; }
    public decimal LateFeePerDay { get; init; }
    public string Description { get; init; } = "";
    public string EquipmentCategory { get; init; } = "";
    public string Name { get; init; } = "";
    public string? Condition { get; init; } = "";
    public string? Status { get; init; } = "";
    public string SerialNumber { get; init; } = "";
    public string? ImagePath { get; init; }
    public string AssetUniqueIdentifier { get; init; } = "";
    public string AssetTag { get; init; } = "";
}

// https://github.com/LuckyPennySoftware/MediatR/wiki#:~:text=Each%20request%20type,return%20Task.
// IRequestHandler<CreateEquipmentItemCommand, Guid>: This is and handler of CreateEquipmentItemCommand and this handler return Guid.

/// <summary>
/// Handler for Create Equipment Item Api.
/// </summary>
public class CreateEquipmentItemCommandHandler : IRequestHandler<CreateEquipmentItemCommand, Guid>
{
    // This service should not be reassigned after initiated.
    public readonly IRentalSystemDbContext _dbContext;

    // Constructor to DI services you needed
    public CreateEquipmentItemCommandHandler(IRentalSystemDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // CancellationToken was used so we could terminate in the middle of the process.

    /// <summary>
    /// Handles CreateEquipmentItemCommand request
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<Guid> Handle(CreateEquipmentItemCommand request, CancellationToken cancellationToken)
    {
        // Get status when It is given
        // The status doesn't existed -> ValidationError
        EquipmentItemStatus equipmentItemStatus = (request.Status is null
                                                    ? EquipmentItemStatus.Available
                                                    : request.Status.ToStatus()) ??
                                                    throw new ValidationException("Invalid equipment status.");

        EquipmentItemCondition equipmentItemCondition = (request.Condition is null
                                                        ? EquipmentItemCondition.New
                                                        : request.Condition.ToCondition()) ??
                                                        throw new ValidationException("Invalid equipment condition.");

        EquipmentCategory equipmentCategory = await _dbContext.EquipmentCategories.FirstOrDefaultAsync(
            ec => ec.Name.ToLower() == request.EquipmentCategory
        ) ?? throw new ValidationException("Invalid equipment category.");

        // May cause argument error
        EquipmentItem equipmentItem = new(
            depositFee: request.DepositFee,
            feePerDay: request.FeePerDay,
            lateFeePerDay: request.LateFeePerDay,
            equipmentCategory: equipmentCategory,
            description: request.Description,
            name: request.Name,
            condition: equipmentItemCondition,
            status: equipmentItemStatus,
            serialNumber: request.SerialNumber,
            imagePath: request.ImagePath,
            assetUniqueIdentifier: request.AssetUniqueIdentifier
        );

        await _dbContext.EquipmentItems.AddAsync(equipmentItem, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return equipmentItem.Id;
    }
}

