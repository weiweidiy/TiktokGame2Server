using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class changeName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Users_UserId",
                table: "Chapters");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Chapters",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Chapters_UserId",
                table: "Chapters",
                newName: "IX_Chapters_PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Users_PlayerId",
                table: "Chapters",
                column: "PlayerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Users_PlayerId",
                table: "Chapters");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "Chapters",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Chapters_PlayerId",
                table: "Chapters",
                newName: "IX_Chapters_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Users_UserId",
                table: "Chapters",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
