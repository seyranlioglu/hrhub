using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Pdf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DocumentFileSize",
                schema: "public",
                table: "ContentLibraries",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DocumentPageCount",
                schema: "public",
                table: "ContentLibraries",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentFileSize",
                schema: "public",
                table: "ContentLibraries");

            migrationBuilder.DropColumn(
                name: "DocumentPageCount",
                schema: "public",
                table: "ContentLibraries");
        }
    }
}
