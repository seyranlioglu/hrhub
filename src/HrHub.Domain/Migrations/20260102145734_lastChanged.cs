using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class lastChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                schema: "public",
                table: "UserCertificates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Score",
                schema: "public",
                table: "UserCertificates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "public",
                table: "UserCertificates",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                schema: "public",
                table: "UserCertificates");

            migrationBuilder.DropColumn(
                name: "Score",
                schema: "public",
                table: "UserCertificates");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "public",
                table: "UserCertificates");
        }
    }
}
