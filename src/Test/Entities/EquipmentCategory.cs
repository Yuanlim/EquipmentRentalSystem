using RentalSystem.Domain.Entities;
using Shouldly;
using Xunit;

namespace RentalSystem.Test.Entities;

public class EquipmentCategoryCreationTest
{
    // https://code-maze.com/aspnetcore-unit-testing-xunit/
    // [MethodWeTest_StateUnderTest_ExpectedBehavior]
    [Theory]
    [InlineData(" hEllo  world  ", "Hw", "hEllo  world", "Hello World", "HW")]
    [InlineData("CAMERA", "CAM", "CAMERA", "Camera", "CAM")]
    [InlineData("  cAMERa  ", "cAm", "cAMERa", "Camera", "CAM")]
    [InlineData("hard disk", "Hdd", "hard disk", "Hard Disk", "HDD")]
    [InlineData("HARD DISK", "HDD", "HARD DISK", "Hard Disk", "HDD")]
    [InlineData("lighting equipment", "LTE", "lighting equipment", "Lighting Equipment", "LTE")]
    public void Constructor_ValidCategoryName_NormalizedName(string name, string assetTagPrefix, string expectedName, string expectedNormalized, string expectedPrefix)
    {
        var equipmentCategory = CreateFlow(name, assetTagPrefix);

        // Assertion
        equipmentCategory.Name.ShouldBe(expectedName);
        equipmentCategory.NormalizedName.ShouldBe(expectedNormalized);
        equipmentCategory.AssetTagPrefix.ShouldBe(expectedPrefix);
    }

    // invalid when equipment category is null, empty or whitespace

    [Theory]
    [InlineData(null, "Hw")]
    [InlineData("", "Hw")]
    [InlineData("   ", "Hw")]
    public void Constructor_InvalidCategoryName_NormalizedName(string? name, string assetTagPrefix)
    {
        var exception = Should.Throw<ArgumentException>(() => CreateFlow(name, assetTagPrefix));
        exception.Message.ShouldBe("EquipmentCategory category name shouldn't be whitespace, empty or null.");
    }

    [Theory]
    [InlineData(" hEllo  world  ", null)]
    [InlineData(" hEllo  world  ", "")]
    [InlineData(" hEllo  world  ", " ")]
    public void Constructor_InvalidCategoryPrefix_NormalizedName(string name, string? assetTagPrefix)
    {
        var exception = Should.Throw<ArgumentException>(() => CreateFlow(name, assetTagPrefix));
        exception.Message.ShouldBe("EquipmentCategory asset tag prefix shouldn't be whitespace, empty or null.");
    }

    public static EquipmentCategory CreateFlow(string? name, string? assetTagPrefix)
    => new EquipmentCategory(name, null, assetTagPrefix);
}