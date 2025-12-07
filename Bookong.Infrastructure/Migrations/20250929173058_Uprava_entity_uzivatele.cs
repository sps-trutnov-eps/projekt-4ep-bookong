using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookong.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Uprava_entity_uzivatele : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationalUnit",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationalUnit",
                table: "Users");
        }
    }
}
