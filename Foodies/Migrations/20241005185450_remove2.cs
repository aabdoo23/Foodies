using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodies.Migrations
{
    /// <inheritdoc />
    public partial class remove2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuisineType",
                table: "RegisterationViewModel");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "RegisterationViewModel");

            migrationBuilder.DropColumn(
                name: "Hotline",
                table: "RegisterationViewModel");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RegisterationViewModel");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "RegisterationViewModel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CuisineType",
                table: "RegisterationViewModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "RegisterationViewModel",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Hotline",
                table: "RegisterationViewModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RegisterationViewModel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "RegisterationViewModel",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
