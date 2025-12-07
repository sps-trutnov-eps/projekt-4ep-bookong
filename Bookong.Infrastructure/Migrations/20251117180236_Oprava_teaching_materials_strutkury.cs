using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookong.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Oprava_teaching_materials_strutkury : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "TeachingMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubjectId",
                table: "TeachingMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "TeachingMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TeachingMaterials_BranchId",
                table: "TeachingMaterials",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingMaterials_SubjectId",
                table: "TeachingMaterials",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TeachingMaterials_UserId",
                table: "TeachingMaterials",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingMaterials_Branches_BranchId",
                table: "TeachingMaterials",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TeachingMaterials_Subjects_SubjectId",
                table: "TeachingMaterials",
                column: "SubjectId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_TeachingMaterials_Branches_BranchId",
                table: "TeachingMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachingMaterials_Subjects_SubjectId",
                table: "TeachingMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_TeachingMaterials_Users_UserId",
                table: "TeachingMaterials");

            migrationBuilder.DropIndex(
                name: "IX_TeachingMaterials_BranchId",
                table: "TeachingMaterials");

            migrationBuilder.DropIndex(
                name: "IX_TeachingMaterials_SubjectId",
                table: "TeachingMaterials");

            migrationBuilder.DropIndex(
                name: "IX_TeachingMaterials_UserId",
                table: "TeachingMaterials");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "TeachingMaterials");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "TeachingMaterials");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TeachingMaterials");
        }
    }
}
