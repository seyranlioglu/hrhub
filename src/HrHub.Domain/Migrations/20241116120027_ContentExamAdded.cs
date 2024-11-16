using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class ContentExamAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "UserExams",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "UserCertificates",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "UserAnswers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "TrainingAmounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "Reviews",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "ExamTopics",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "Exams",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "CurrAccTrainings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "CurrAccs",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "ContentNotes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "CommentVotes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "Carts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "CartItem",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ContentExamAction",
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
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
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
                    ExamQuestionId = table.Column<long>(type: "bigint", nullable: false),
                    TrainingContentId = table.Column<long>(type: "bigint", nullable: false),
                    ContentExamActionId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
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
                        name: "FK_ContentExam_ExamQuestions_ExamQuestionId",
                        column: x => x.ExamQuestionId,
                        principalSchema: "public",
                        principalTable: "ExamQuestions",
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
                name: "IX_ContentExam_ExamQuestionId",
                schema: "public",
                table: "ContentExam",
                column: "ExamQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentExam_TrainingContentId",
                schema: "public",
                table: "ContentExam",
                column: "TrainingContentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentExam",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ContentExamAction",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "UserCertificates");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "TrainingAmounts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "ExamTopics");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "CurrAccTrainings");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "CurrAccs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "ContentNotes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "CommentVotes");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "CartItem");
        }
    }
}
