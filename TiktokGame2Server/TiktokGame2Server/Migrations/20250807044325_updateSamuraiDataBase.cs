using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class updateSamuraiDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Samurais_BusinessId_PlayerId",
                table: "Samurais");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Samurais_BusinessId_PlayerId",
                table: "Samurais",
                columns: new[] { "BusinessId", "PlayerId" },
                unique: true);
        }
    }
}
