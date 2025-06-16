using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class curracctype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                schema: "public",
                table: "CurrAccType",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "public",
                table: "CurrAccType",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "CurrAccType",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "public",
                table: "CurrAccType",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                schema: "public",
                table: "CurrAccType");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "public",
                table: "CurrAccType");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "CurrAccType");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "public",
                table: "CurrAccType");
        }
    }
}
