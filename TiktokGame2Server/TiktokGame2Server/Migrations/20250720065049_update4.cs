using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class update4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LevelNodes_NodeId",
                table: "LevelNodes");

            migrationBuilder.CreateIndex(
                name: "IX_LevelNodes_NodeId_PlayerId",
                table: "LevelNodes",
                columns: new[] { "NodeId", "PlayerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LevelNodes_NodeId_PlayerId",
                table: "LevelNodes");

            migrationBuilder.CreateIndex(
                name: "IX_LevelNodes_NodeId",
                table: "LevelNodes",
                column: "NodeId",
                unique: true);
        }
    }
}
