using RentalSystem.Test.Api.Infrastructure;
using Xunit;

namespace RentalSystem.Test.Api.EquipmentItems;

public class CreateEquipmentItemTest
    : IClassFixture<CustomWebApplicationWebFactory<Program>>
{
    private readonly CustomWebApplicationWebFactory<Program> _factory;
}