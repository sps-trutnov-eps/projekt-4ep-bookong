using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookong.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Entita_pro_zadost_o_pridani_knihy_do_maturitni_cetby : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaturitaBookAssignmentRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BookName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BookAuthor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestingUserId = table.Column<int>(type: "int", nullable: false),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProcessingUserId = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaturitaBookAssignmentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaturitaBookAssignmentRequests_Users_ProcessingUserId",
                        column: x => x.ProcessingUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MaturitaBookAssignmentRequests_Users_RequestingUserId",
                        column: x => x.RequestingUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaturitaBookAssignmentRequests_ProcessingUserId",
                table: "MaturitaBookAssignmentRequests",
                column: "ProcessingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaturitaBookAssignmentRequests_RequestingUserId",
                table: "MaturitaBookAssignmentRequests",
                column: "RequestingUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaturitaBookAssignmentRequests");
        }
    }
}
