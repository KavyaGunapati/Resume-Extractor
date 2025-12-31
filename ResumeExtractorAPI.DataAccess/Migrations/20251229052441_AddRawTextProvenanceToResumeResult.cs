using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeExtractorAPI.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddRawTextProvenanceToResumeResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Skills",
                table: "ResumeResults",
                newName: "SelectedSource");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "ResumeResults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RawText",
                table: "ResumeResults",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResumeResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skill_ResumeResults_ResumeResultId",
                        column: x => x.ResumeResultId,
                        principalTable: "ResumeResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skill_ResumeResultId",
                table: "Skill",
                column: "ResumeResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "ResumeResults");

            migrationBuilder.DropColumn(
                name: "RawText",
                table: "ResumeResults");

            migrationBuilder.RenameColumn(
                name: "SelectedSource",
                table: "ResumeResults",
                newName: "Skills");
        }
    }
}
