using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeExtractorAPI.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLatestSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "WorkExperiences",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YearsOfExperience",
                table: "WorkExperiences",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "PersonalInfos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "WorkExperiences");

            migrationBuilder.DropColumn(
                name: "YearsOfExperience",
                table: "WorkExperiences");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "PersonalInfos");
        }
    }
}
