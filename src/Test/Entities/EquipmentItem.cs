using Newtonsoft.Json.Linq;
using RentalSystem.Domain.Entities;
using Shouldly;
using Xunit;

namespace RentalSystem.Test.Entities;

public class EquipmentItemCreationTest
{
    // Source - https://stackoverflow.com/a/60473421
    public static IEnumerable<object[]> ValidEquipmentItemTestData()
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Entities", "Json", "equipment_item_valid_data.json");
        var json = File.ReadAllText(filePath);
        var jObject = JObject.Parse(json);
        var category = jObject["category"]?.ToObject<EquipmentCategoryTestData>();
        var item = jObject["equipmentItem"]?.ToObject<EquipmentItemTestData>();

        if (category is null || item is null)
        {
            throw new InvalidOperationException("Invalid equipment item test JSON.");
        }

        yield return new object[] { category, item };
    }

    [Theory]
    [MemberData(nameof(ValidEquipmentItemTestData))]
    public void Constructor_ValidEquipmentItem_EquipmentItem(EquipmentCategoryTestData categoryData, EquipmentItemTestData itemData)
    {
        var equipmentItem = CreateFlow(categoryData, itemData);

        equipmentItem.DepositFee.ShouldBe(itemData.DepositFee);
        equipmentItem.FeePerDay.ShouldBe(itemData.FeePerDay);
        equipmentItem.LateFeePerDay.ShouldBe(itemData.LateFeePerDay);
        equipmentItem.Description.ShouldBe(itemData.Description);
        equipmentItem.Name.ShouldBe(itemData.Name);
        equipmentItem.Condition.ShouldBe(itemData.Condition);
        equipmentItem.Status.ShouldBe(itemData.Status);
        equipmentItem.SerialNumber.ShouldBe(itemData.SerialNumber);
        equipmentItem.ImagePath.ShouldBe(itemData.ImagePath);
        equipmentItem.AssetTag.ShouldBe(itemData.AssetTag);

        var eec = equipmentItem.EquipmentCategory;
        eec.Name.ShouldBe(categoryData.Name);
        eec.NormalizedName.ShouldBe(categoryData.Name);
        eec.AssetTagPrefix.ShouldBe(categoryData.AssetTagPrefix);
        eec.EquipmentItems.Count.ShouldBe(1);

        var attachedItem = eec.EquipmentItems.Single();
        attachedItem.DepositFee.ShouldBe(itemData.DepositFee);
        attachedItem.FeePerDay.ShouldBe(itemData.FeePerDay);
        attachedItem.LateFeePerDay.ShouldBe(itemData.LateFeePerDay);
        attachedItem.Description.ShouldBe(itemData.Description);
        attachedItem.Name.ShouldBe(itemData.Name);
        attachedItem.Condition.ShouldBe(itemData.Condition);
        attachedItem.Status.ShouldBe(itemData.Status);
        attachedItem.SerialNumber.ShouldBe(itemData.SerialNumber);
        attachedItem.ImagePath.ShouldBe(itemData.ImagePath);
        attachedItem.AssetTag.ShouldBe(itemData.AssetTag);
    }

    [Theory]
    [MemberData(nameof(ValidEquipmentItemTestData))]
    public void Constructor_InvalidDepositFeeEquipmentItem_EquipmentItem(EquipmentCategoryTestData categoryData, EquipmentItemTestData itemData)
    {
        itemData.DepositFee = -1;
        var exception = Should.Throw<ArgumentException>(() => CreateFlow(categoryData, itemData));
        exception.Message.ShouldBe("EquipmentItem deposit fee value should be greater than 0.");
    }

    [Theory]
    [MemberData(nameof(ValidEquipmentItemTestData))]
    public void Constructor_InvalidLengthSerialNumberEquipmentItem_EquipmentItem(EquipmentCategoryTestData categoryData, EquipmentItemTestData itemData)
    {
        itemData.SerialNumber = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim. Donec pede justo, fringilla vel, aliquet nec, vulputate eget, arcu. In enim justo, rhoncus ut, imperdiet a, venenatis vitae, justo. Nullam dictum felis eu pede mollis pretium. Integer tincidunt. Cras dapibus. Vivamus elementum semper nisi. Aenean vulputate eleifend tellus. Aenean leo ligula, porttitor eu, consequat vitae, eleifend ac, enim. Aliquam lorem ante, dapibus in, viverra quis, feugiat a, tellus. Phasellus viverra nulla ut metus varius laoreet. Quisque rutrum. Aenean imperdiet. Etiam ultricies nisi vel augue. Curabitur ullamcorper ultricies nisi. Nam eget dui. Etiam rhoncus. Maecenas tempus, tellus eget condimentum rhoncus, sem quam semper libero, sit amet adipiscing sem neque sed ipsum. Nam quam nunc, blandit vel, luctus pulvinar, hendrerit id, lorem. Maecenas nec odio et ante tincidunt tempus. Donec vitae sapien ut libero venenatis faucibus. Nullam quis ante. Etiam sit amet orci eget eros faucibus tincidunt. Duis leo. Sed fringilla mauris sit amet nibh. Donec sodales sagittis magna. Sed consequat, leo eget bibendum sodales, augue velit cursus nunc";
        var exception = Should.Throw<ArgumentException>(() => CreateFlow(categoryData, itemData));
        exception.Message.ShouldBe("EquipmentItem serial number length shouldn't be larger than 200.");
    }

    [Theory]
    [MemberData(nameof(ValidEquipmentItemTestData))]
    public void Constructor_InvalidNullSerialNumberEquipmentItem_EquipmentItem(EquipmentCategoryTestData categoryData, EquipmentItemTestData itemData)
    {
        itemData.SerialNumber = "";
        var exception = Should.Throw<ArgumentException>(() => CreateFlow(categoryData, itemData));
        exception.Message.ShouldBe("EquipmentItem serial number shouldn't be whitespace, empty or null.");
    }

    public static EquipmentItem CreateFlow(EquipmentCategoryTestData categoryData, EquipmentItemTestData itemData)
    {
        var equipmentCategory = new EquipmentCategory(
            categoryData.Name,
            categoryData.CreatedByUserId,
            categoryData.AssetTagPrefix
        );

        var equipmentItem = new EquipmentItem(
            depositFee: itemData.DepositFee,
            feePerDay: itemData.FeePerDay,
            lateFeePerDay: itemData.LateFeePerDay,
            equipmentCategory: equipmentCategory,
            description: itemData.Description,
            name: itemData.Name,
            condition: itemData.Condition,
            status: itemData.Status,
            serialNumber: itemData.SerialNumber,
            imagePath: itemData.ImagePath,
            assetUniqueIdentifier: itemData.AssetUniqueIdentifier
        );

        return equipmentItem;
    }
}