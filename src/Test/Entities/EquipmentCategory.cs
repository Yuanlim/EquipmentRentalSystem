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
        string? createdByUserId = null;

        var equipmentCategory = new EquipmentCategory(name, createdByUserId, assetTagPrefix);

        // Assertion
        equipmentCategory.Name.ShouldBe(expectedName);
        equipmentCategory.NormalizedName.ShouldBe(expectedNormalized);
        equipmentCategory.AssetTagPrefix.ShouldBe(expectedPrefix);
    }
}