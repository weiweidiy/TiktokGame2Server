using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class changeTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Users_PlayerId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Users_PlayerId",
                table: "Chapters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Players");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Players_PlayerId",
                table: "Accounts",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Players_PlayerId",
                table: "Chapters",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Players_PlayerId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Players_PlayerId",
                table: "Chapters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "Users");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Users_PlayerId",
                table: "Accounts",
                column: "PlayerId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Users_PlayerId",
                table: "Chapters",
                column: "PlayerId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
