using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentExam_ExamQuestions_ExamQuestionId",
                schema: "public",
                table: "ContentExam");

            migrationBuilder.DropColumn(
                name: "EffectiveFrom",
                schema: "public",
                table: "ExamVersions");

            migrationBuilder.DropColumn(
                name: "ExamTime",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "PassingScore",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "SuccesRate",
                schema: "public",
                table: "Exams");

            migrationBuilder.RenameColumn(
                name: "ViewQuestionCount",
                schema: "public",
                table: "Exams",
                newName: "InstructorId");

            migrationBuilder.RenameColumn(
                name: "ExamQuestionId",
                schema: "public",
                table: "ContentExam",
                newName: "ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_ContentExam_ExamQuestionId",
                schema: "public",
                table: "ContentExam",
                newName: "IX_ContentExam_ExamId");

            migrationBuilder.AddColumn<bool>(
                name: "IsMainUser",
                schema: "public",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExamTime",
                schema: "public",
                table: "ExamVersions",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<decimal>(
                name: "PassingScore",
                schema: "public",
                table: "ExamVersions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SuccessRate",
                schema: "public",
                table: "ExamVersions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalQuestionCount",
                schema: "public",
                table: "ExamVersions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VersionDescription",
                schema: "public",
                table: "ExamVersions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "QuestionCount",
                schema: "public",
                table: "ExamTopics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeqNumber",
                schema: "public",
                table: "ExamTopics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "CurrAccTypeId",
                schema: "public",
                table: "CurrAccs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "CurrAccType",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LangCode = table.Column<string>(type: "text", nullable: true),
                    EnterpriseAcc = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_CurrAccType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExamStatus",
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
                    table.PrimaryKey("PK_ExamStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exams_ExamStatusId",
                schema: "public",
                table: "Exams",
                column: "ExamStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_InstructorId",
                schema: "public",
                table: "Exams",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrAccs_CurrAccTypeId",
                schema: "public",
                table: "CurrAccs",
                column: "CurrAccTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentExam_Exams_ExamId",
                schema: "public",
                table: "ContentExam",
                column: "ExamId",
                principalSchema: "public",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrAccs_CurrAccType_CurrAccTypeId",
                schema: "public",
                table: "CurrAccs",
                column: "CurrAccTypeId",
                principalSchema: "public",
                principalTable: "CurrAccType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_ExamStatus_ExamStatusId",
                schema: "public",
                table: "Exams",
                column: "ExamStatusId",
                principalSchema: "public",
                principalTable: "ExamStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Users_InstructorId",
                schema: "public",
                table: "Exams",
                column: "InstructorId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentExam_Exams_ExamId",
                schema: "public",
                table: "ContentExam");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrAccs_CurrAccType_CurrAccTypeId",
                schema: "public",
                table: "CurrAccs");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_ExamStatus_ExamStatusId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Users_InstructorId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropTable(
                name: "CurrAccType",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ExamStatus",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_Exams_ExamStatusId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_InstructorId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_CurrAccs_CurrAccTypeId",
                schema: "public",
                table: "CurrAccs");

            migrationBuilder.DropColumn(
                name: "IsMainUser",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ExamTime",
                schema: "public",
                table: "ExamVersions");

            migrationBuilder.DropColumn(
                name: "PassingScore",
                schema: "public",
                table: "ExamVersions");

            migrationBuilder.DropColumn(
                name: "SuccessRate",
                schema: "public",
                table: "ExamVersions");

            migrationBuilder.DropColumn(
                name: "TotalQuestionCount",
                schema: "public",
                table: "ExamVersions");

            migrationBuilder.DropColumn(
                name: "VersionDescription",
                schema: "public",
                table: "ExamVersions");

            migrationBuilder.DropColumn(
                name: "QuestionCount",
                schema: "public",
                table: "ExamTopics");

            migrationBuilder.DropColumn(
                name: "SeqNumber",
                schema: "public",
                table: "ExamTopics");

            migrationBuilder.DropColumn(
                name: "CurrAccTypeId",
                schema: "public",
                table: "CurrAccs");

            migrationBuilder.RenameColumn(
                name: "InstructorId",
                schema: "public",
                table: "Exams",
                newName: "ViewQuestionCount");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                schema: "public",
                table: "ContentExam",
                newName: "ExamQuestionId");

            migrationBuilder.RenameIndex(
                name: "IX_ContentExam_ExamId",
                schema: "public",
                table: "ContentExam",
                newName: "IX_ContentExam_ExamQuestionId");

            migrationBuilder.AddColumn<DateTime>(
                name: "EffectiveFrom",
                schema: "public",
                table: "ExamVersions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExamTime",
                schema: "public",
                table: "Exams",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<decimal>(
                name: "PassingScore",
                schema: "public",
                table: "Exams",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "SuccesRate",
                schema: "public",
                table: "Exams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentExam_ExamQuestions_ExamQuestionId",
                schema: "public",
                table: "ContentExam",
                column: "ExamQuestionId",
                principalSchema: "public",
                principalTable: "ExamQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
