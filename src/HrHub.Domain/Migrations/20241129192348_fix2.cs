using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class fix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentExam",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ContentExamAction",
                schema: "public");

            migrationBuilder.AlterColumn<int>(
                name: "PageCount",
                schema: "public",
                table: "TrainingContents",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "MinReadTimeThreshold",
                schema: "public",
                table: "TrainingContents",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<bool>(
                name: "AllowSeeking",
                schema: "public",
                table: "TrainingContents",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<long>(
                name: "ExamId",
                schema: "public",
                table: "TrainingContents",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ActionId",
                schema: "public",
                table: "Exams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ExamAction",
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
                    table.PrimaryKey("PK_ExamAction", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingContents_ExamId",
                schema: "public",
                table: "TrainingContents",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ActionId",
                schema: "public",
                table: "Exams",
                column: "ActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamAction_ActionId",
                schema: "public",
                table: "Exams",
                column: "ActionId",
                principalSchema: "public",
                principalTable: "ExamAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingContents_Exams_ExamId",
                schema: "public",
                table: "TrainingContents",
                column: "ExamId",
                principalSchema: "public",
                principalTable: "Exams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamAction_ActionId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingContents_Exams_ExamId",
                schema: "public",
                table: "TrainingContents");

            migrationBuilder.DropTable(
                name: "ExamAction",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_TrainingContents_ExamId",
                schema: "public",
                table: "TrainingContents");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ActionId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ExamId",
                schema: "public",
                table: "TrainingContents");

            migrationBuilder.DropColumn(
                name: "ActionId",
                schema: "public",
                table: "Exams");

            migrationBuilder.AlterColumn<int>(
                name: "PageCount",
                schema: "public",
                table: "TrainingContents",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MinReadTimeThreshold",
                schema: "public",
                table: "TrainingContents",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "AllowSeeking",
                schema: "public",
                table: "TrainingContents",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ContentExamAction",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentExamAction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentExam",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentExamActionId = table.Column<long>(type: "bigint", nullable: false),
                    ExamId = table.Column<long>(type: "bigint", nullable: false),
                    TrainingContentId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentExam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentExam_ContentExamAction_ContentExamActionId",
                        column: x => x.ContentExamActionId,
                        principalSchema: "public",
                        principalTable: "ContentExamAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentExam_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "public",
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentExam_TrainingContents_TrainingContentId",
                        column: x => x.TrainingContentId,
                        principalSchema: "public",
                        principalTable: "TrainingContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentExam_ContentExamActionId",
                schema: "public",
                table: "ContentExam",
                column: "ContentExamActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentExam_ExamId",
                schema: "public",
                table: "ContentExam",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentExam_TrainingContentId",
                schema: "public",
                table: "ContentExam",
                column: "TrainingContentId");
        }
    }
}
