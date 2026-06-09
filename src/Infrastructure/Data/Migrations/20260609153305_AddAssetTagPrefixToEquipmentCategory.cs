using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetTagPrefixToEquipmentCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "EquipmentItems",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "EquipmentCategories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AssetTagPrefix",
                table: "EquipmentCategories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentCategories_AssetTagPrefix",
                table: "EquipmentCategories",
                column: "AssetTagPrefix",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EquipmentCategories_AssetTagPrefix",
                table: "EquipmentCategories");

            migrationBuilder.DropColumn(
                name: "AssetTagPrefix",
                table: "EquipmentCategories");

            migrationBuilder.AlterColumn<string>(
                name: "SerialNumber",
                table: "EquipmentItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedByUserId",
                table: "EquipmentCategories",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
