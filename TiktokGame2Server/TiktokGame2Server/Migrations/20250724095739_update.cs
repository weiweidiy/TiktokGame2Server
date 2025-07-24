using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SoldierUid",
                table: "Samurais",
                type: "text",
                nullable: false,
                defaultValue: "2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoldierUid",
                table: "Samurais");
        }
    }
}
