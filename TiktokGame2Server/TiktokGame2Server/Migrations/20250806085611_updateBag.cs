using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class updateBag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bags");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.CreateTable(
                name: "BagSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: true),
                    PlayerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BagSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BagSlots_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BagItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemBusinessId = table.Column<string>(type: "text", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    BagSlotId = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BagItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BagItems_BagSlots_BagSlotId",
                        column: x => x.BagSlotId,
                        principalTable: "BagSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BagItems_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BagItems_BagSlotId",
                table: "BagItems",
                column: "BagSlotId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BagItems_PlayerId",
                table: "BagItems",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_BagSlots_PlayerId_ItemId",
                table: "BagSlots",
                columns: new[] { "PlayerId", "ItemId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BagItems");

            migrationBuilder.DropTable(
                name: "BagSlots");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    ItemBusinessId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemId = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bags_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bags_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bags_ItemId",
                table: "Bags",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Bags_PlayerId_ItemId",
                table: "Bags",
                columns: new[] { "PlayerId", "ItemId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_PlayerId",
                table: "Items",
                column: "PlayerId");
        }
    }
}
