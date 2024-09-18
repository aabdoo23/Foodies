using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodies.Migrations
{
    /// <inheritdoc />
    public partial class all : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerMessage");

            migrationBuilder.DropTable(
                name: "CustomerRestaurant");

            migrationBuilder.DropTable(
                name: "MessageRestaurant");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerMessage",
                columns: table => new
                {
                    CustomersChatCustomerId = table.Column<int>(type: "int", nullable: false),
                    MessagesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerMessage", x => new { x.CustomersChatCustomerId, x.MessagesId });
                    table.ForeignKey(
                        name: "FK_CustomerMessage_Customers_CustomersChatCustomerId",
                        column: x => x.CustomersChatCustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerMessage_Messages_MessagesId",
                        column: x => x.MessagesId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerRestaurant",
                columns: table => new
                {
                    CustomerChatCustomerId = table.Column<int>(type: "int", nullable: false),
                    RestaurantChatRestaurantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerRestaurant", x => new { x.CustomerChatCustomerId, x.RestaurantChatRestaurantId });
                    table.ForeignKey(
                        name: "FK_CustomerRestaurant_Customers_CustomerChatCustomerId",
                        column: x => x.CustomerChatCustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerRestaurant_Restaurants_RestaurantChatRestaurantId",
                        column: x => x.RestaurantChatRestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MessageRestaurant",
                columns: table => new
                {
                    MessagesId = table.Column<int>(type: "int", nullable: false),
                    RestaurantsChatRestaurantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageRestaurant", x => new { x.MessagesId, x.RestaurantsChatRestaurantId });
                    table.ForeignKey(
                        name: "FK_MessageRestaurant_Messages_MessagesId",
                        column: x => x.MessagesId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageRestaurant_Restaurants_RestaurantsChatRestaurantId",
                        column: x => x.RestaurantsChatRestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerMessage_MessagesId",
                table: "CustomerMessage",
                column: "MessagesId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerRestaurant_RestaurantChatRestaurantId",
                table: "CustomerRestaurant",
                column: "RestaurantChatRestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageRestaurant_RestaurantsChatRestaurantId",
                table: "MessageRestaurant",
                column: "RestaurantsChatRestaurantId");
        }
    }
}
