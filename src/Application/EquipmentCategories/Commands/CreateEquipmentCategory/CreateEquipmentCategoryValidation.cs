using FluentValidation;
using RentalSystem.Application.Common.Validation;
using RentalSystem.Domain.Entities;

namespace RentalSystem.Application.EquipmentCategories.Commands.CreateEquipmentCategory;

/// <summary>
/// Validate incoming create equipment category request.
/// </summary>
public class CreateEquipmentCategoryCommandValidator : AbstractValidator<CreateEquipmentCategoryCommand>
{

    public CreateEquipmentCategoryCommandValidator()
    {
        Type classType = typeof(EquipmentCategory);

        RuleFor(v => v.CategoryName)
            .IsNotNullOrWhitespaceWithMessage(classType);

        RuleFor(v => v.AssetTagPrefix)
            .IsNotNullOrWhitespaceWithMessage(classType);
    }
}