using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodies.Migrations
{
    /// <inheritdoc />
    public partial class deletemaxmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPrice",
                table: "Restaurants");

            migrationBuilder.DropColumn(
                name: "MinPrice",
                table: "Restaurants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxPrice",
                table: "Restaurants",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinPrice",
                table: "Restaurants",
                type: "int",
                nullable: true);
        }
    }
}
