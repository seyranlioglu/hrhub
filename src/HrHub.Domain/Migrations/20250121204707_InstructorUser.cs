using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class InstructorUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Instructors_UserId",
                schema: "public",
                table: "Instructors");

            migrationBuilder.AddColumn<string>(
                name: "InstructorCode",
                schema: "public",
                table: "Instructors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_UserId",
                schema: "public",
                table: "Instructors",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Instructors_UserId",
                schema: "public",
                table: "Instructors");

            migrationBuilder.DropColumn(
                name: "InstructorCode",
                schema: "public",
                table: "Instructors");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_UserId",
                schema: "public",
                table: "Instructors",
                column: "UserId");
        }
    }
}
