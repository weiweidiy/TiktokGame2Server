using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class update6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NodeId",
                table: "LevelNodes",
                newName: "NodeUid");

            migrationBuilder.RenameIndex(
                name: "IX_LevelNodes_NodeId_PlayerId",
                table: "LevelNodes",
                newName: "IX_LevelNodes_NodeUid_PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NodeUid",
                table: "LevelNodes",
                newName: "NodeId");

            migrationBuilder.RenameIndex(
                name: "IX_LevelNodes_NodeUid_PlayerId",
                table: "LevelNodes",
                newName: "IX_LevelNodes_NodeId_PlayerId");
        }
    }
}
