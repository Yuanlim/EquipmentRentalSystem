using FluentValidation;
using RentalSystem.Application.Common.Validation;
using RentalSystem.Domain.Entities;

namespace RentalSystem.Application.EquipmentItems.Commands.CreateEquipmentItem;

/// <summary>
/// Validate incoming create equipment item request.
/// </summary>
public class CreateEquipmentItemCommandValidator : AbstractValidator<CreateEquipmentItemCommand>
{

    public CreateEquipmentItemCommandValidator()
    {
        Type classType = typeof(EquipmentItem);

        RuleFor(v => v.DepositFee)
            .GreaterThanWithMessage(0, classType);

        RuleFor(v => v.FeePerDay)
            .GreaterThanWithMessage(0, classType);

        RuleFor(v => v.LateFeePerDay)
            .GreaterThanWithMessage(0, classType);

        RuleFor(v => v.Description)
            .MaxLengthWithMessage(2000, classType);

        RuleFor(v => v.Name)
            .MaxLengthWithMessage(200, classType)
            .IsNotNullOrWhitespaceWithMessage(classType);

        RuleFor(v => v.SerialNumber)
            .MaxLengthWithMessage(200, classType)
            .IsNotNullOrWhitespaceWithMessage(classType);

        RuleFor(v => v.AssetUniqueIdentifier)
            .MaxLengthWithMessage(200, classType)
            .NotEmpty()
            .WithMessage($"{classType.Name} assets unique identifier should be given; Because assets tag should be category asset tag prefix followed by some unique identifier. Ex:CAM-001, CAM-A001");
    }
}