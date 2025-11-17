using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookong.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Relace_uzivatele_a_vyukoveho_materialu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TeachingMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TeachingMaterials_UserId",
                table: "TeachingMaterials",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingMaterials_Users_UserId",
                table: "TeachingMaterials",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeachingMaterials_Users_UserId",
                table: "TeachingMaterials");

            migrationBuilder.DropIndex(
                name: "IX_TeachingMaterials_UserId",
                table: "TeachingMaterials");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TeachingMaterials");
        }
    }
}
