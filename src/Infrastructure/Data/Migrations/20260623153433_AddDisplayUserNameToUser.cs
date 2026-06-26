using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentalSystem.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDisplayUserNameToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayUserName",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayUserName",
                table: "AspNetUsers");
        }
    }
}
