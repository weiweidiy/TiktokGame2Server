using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class changeAttr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Users_UserId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "PlayerId",
                table: "Accounts",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PlayerId",
                table: "Accounts",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Users_PlayerId",
                table: "Accounts",
                column: "PlayerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Users_PlayerId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_PlayerId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Accounts");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserId",
                table: "Accounts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Users_UserId",
                table: "Accounts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
