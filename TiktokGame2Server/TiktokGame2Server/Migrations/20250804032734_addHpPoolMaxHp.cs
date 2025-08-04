using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class addHpPoolMaxHp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxHp",
                table: "HpPools",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxHp",
                table: "HpPools");
        }
    }
}
