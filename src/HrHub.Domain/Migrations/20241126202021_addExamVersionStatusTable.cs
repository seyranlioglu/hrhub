using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class addExamVersionStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Abbreviation",
                schema: "public",
                table: "ExamQuestions");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "public",
                table: "ExamQuestions");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "ExamQuestions");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "public",
                table: "ExamQuestions");

            migrationBuilder.DropColumn(
                name: "Abbreviation",
                schema: "public",
                table: "ExamOptions");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "public",
                table: "ExamOptions");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "ExamOptions");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "public",
                table: "ExamOptions");

            migrationBuilder.AddColumn<long>(
                name: "ExamVersionStatusId",
                schema: "public",
                table: "ExamVersions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ExamVersionStatuses",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamVersionStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamVersions_ExamVersionStatusId",
                schema: "public",
                table: "ExamVersions",
                column: "ExamVersionStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamVersions_ExamVersionStatuses_ExamVersionStatusId",
                schema: "public",
                table: "ExamVersions",
                column: "ExamVersionStatusId",
                principalSchema: "public",
                principalTable: "ExamVersionStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamVersions_ExamVersionStatuses_ExamVersionStatusId",
                schema: "public",
                table: "ExamVersions");

            migrationBuilder.DropTable(
                name: "ExamVersionStatuses",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_ExamVersions_ExamVersionStatusId",
                schema: "public",
                table: "ExamVersions");

            migrationBuilder.DropColumn(
                name: "ExamVersionStatusId",
                schema: "public",
                table: "ExamVersions");

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                schema: "public",
                table: "ExamQuestions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "public",
                table: "ExamQuestions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "ExamQuestions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "public",
                table: "ExamQuestions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Abbreviation",
                schema: "public",
                table: "ExamOptions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "public",
                table: "ExamOptions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "ExamOptions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "public",
                table: "ExamOptions",
                type: "text",
                nullable: true);
        }
    }
}
