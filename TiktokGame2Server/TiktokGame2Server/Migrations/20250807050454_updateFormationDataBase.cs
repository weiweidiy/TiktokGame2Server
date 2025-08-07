using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiktokGame2Server.Migrations
{
    /// <inheritdoc />
    public partial class updateFormationDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Formations_FormationType_FormationPoint_SamuraiId",
                table: "Formations");

            migrationBuilder.CreateIndex(
                name: "IX_Formations_FormationType_FormationPoint_PlayerId",
                table: "Formations",
                columns: new[] { "FormationType", "FormationPoint", "PlayerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Formations_FormationType_FormationPoint_PlayerId",
                table: "Formations");

            migrationBuilder.CreateIndex(
                name: "IX_Formations_FormationType_FormationPoint_SamuraiId",
                table: "Formations",
                columns: new[] { "FormationType", "FormationPoint", "SamuraiId" },
                unique: true);
        }
    }
}
