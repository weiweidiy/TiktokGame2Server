using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class refactorCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Currencies_PlayerId",
                table: "Currencies");

            migrationBuilder.RenameColumn(
                name: "Pan",
                table: "Currencies",
                newName: "CurrencyType");

            migrationBuilder.RenameColumn(
                name: "Coin",
                table: "Currencies",
                newName: "Count");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_PlayerId_CurrencyType",
                table: "Currencies",
                columns: new[] { "PlayerId", "CurrencyType" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Currencies_PlayerId_CurrencyType",
                table: "Currencies");

            migrationBuilder.RenameColumn(
                name: "CurrencyType",
                table: "Currencies",
                newName: "Pan");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "Currencies",
                newName: "Coin");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_PlayerId",
                table: "Currencies",
                column: "PlayerId",
                unique: true);
        }
    }
}
