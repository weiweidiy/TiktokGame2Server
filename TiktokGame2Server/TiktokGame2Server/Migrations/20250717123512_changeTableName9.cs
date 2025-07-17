using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class changeTableName9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_PlayerId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "UserUid",
                table: "Accounts");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PlayerId",
                table: "Accounts",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_PlayerId",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "UserUid",
                table: "Accounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PlayerId",
                table: "Accounts",
                column: "PlayerId",
                unique: true);
        }
    }
}
