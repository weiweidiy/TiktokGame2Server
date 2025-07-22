using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Uid",
                table: "Samurais",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_Samurais_Uid_PlayerId",
                table: "Samurais",
                newName: "IX_Samurais_BusinessId_PlayerId");

            migrationBuilder.RenameColumn(
                name: "Uid",
                table: "LevelNodes",
                newName: "BusinessId");

            migrationBuilder.RenameIndex(
                name: "IX_LevelNodes_Uid_PlayerId",
                table: "LevelNodes",
                newName: "IX_LevelNodes_BusinessId_PlayerId");

            migrationBuilder.CreateTable(
                name: "Formations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FormationType = table.Column<int>(type: "integer", nullable: false),
                    FormationPoint = table.Column<int>(type: "integer", nullable: false),
                    SamuraiId = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Formations_Samurais_SamuraiId",
                        column: x => x.SamuraiId,
                        principalTable: "Samurais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Samurais_PlayerId",
                table: "Samurais",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Formations_FormationType_FormationPoint_SamuraiId",
                table: "Formations",
                columns: new[] { "FormationType", "FormationPoint", "SamuraiId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Formations_SamuraiId",
                table: "Formations",
                column: "SamuraiId");

            migrationBuilder.AddForeignKey(
                name: "FK_Samurais_Players_PlayerId",
                table: "Samurais",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Samurais_Players_PlayerId",
                table: "Samurais");

            migrationBuilder.DropTable(
                name: "Formations");

            migrationBuilder.DropIndex(
                name: "IX_Samurais_PlayerId",
                table: "Samurais");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "Samurais",
                newName: "Uid");

            migrationBuilder.RenameIndex(
                name: "IX_Samurais_BusinessId_PlayerId",
                table: "Samurais",
                newName: "IX_Samurais_Uid_PlayerId");

            migrationBuilder.RenameColumn(
                name: "BusinessId",
                table: "LevelNodes",
                newName: "Uid");

            migrationBuilder.RenameIndex(
                name: "IX_LevelNodes_BusinessId_PlayerId",
                table: "LevelNodes",
                newName: "IX_LevelNodes_Uid_PlayerId");
        }
    }
}
