using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodies.Migrations
{
    /// <inheritdoc />
    public partial class remove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Address",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Address",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
