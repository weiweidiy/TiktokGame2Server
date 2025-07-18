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
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Players_PlayerId",
                table: "Chapters");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Chapters",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChapterId",
                table: "Chapters",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_ChapterId",
                table: "Chapters",
                column: "ChapterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Players_PlayerId",
                table: "Chapters",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chapters_Players_PlayerId",
                table: "Chapters");

            migrationBuilder.DropIndex(
                name: "IX_Chapters_ChapterId",
                table: "Chapters");

            migrationBuilder.DropColumn(
                name: "ChapterId",
                table: "Chapters");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Chapters",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Chapters_Players_PlayerId",
                table: "Chapters",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
