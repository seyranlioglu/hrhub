using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class ExamVersion_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamTopics_Exams_ExamId",
                schema: "public",
                table: "ExamTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_ExamAnswers_AnswerId",
                schema: "public",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExams_Exams_ExamId",
                schema: "public",
                table: "UserExams");

            migrationBuilder.DropTable(
                name: "ExamAnswers",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "Question",
                schema: "public",
                table: "ExamQuestions");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                schema: "public",
                table: "UserExams",
                newName: "ExamVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_UserExams_ExamId",
                schema: "public",
                table: "UserExams",
                newName: "IX_UserExams_ExamVersionId");

            migrationBuilder.RenameColumn(
                name: "AnswerId",
                schema: "public",
                table: "UserAnswers",
                newName: "OptionId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswers_AnswerId",
                schema: "public",
                table: "UserAnswers",
                newName: "IX_UserAnswers_OptionId");

            migrationBuilder.RenameColumn(
                name: "ExamId",
                schema: "public",
                table: "ExamTopics",
                newName: "ExamVersionId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamTopics_ExamId",
                schema: "public",
                table: "ExamTopics",
                newName: "IX_ExamTopics_ExamVersionId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "WhatYouWillLearns",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingStatuses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingSections",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingLevels",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingContents",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingCategories",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingAnnouncementsComments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingAnnouncements",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TimeUnits",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "Languages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "InstructorTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "Exams",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ExamStatusId",
                schema: "public",
                table: "Exams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "PassingScore",
                schema: "public",
                table: "Exams",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "public",
                table: "Exams",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "ExamQuestions",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QuestionText",
                schema: "public",
                table: "ExamQuestions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Score",
                schema: "public",
                table: "ExamQuestions",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "CurrAccTrainingStatuses",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "ContentTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "ContentExamAction",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "ContentComments",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "CertificateTypes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "CertificateTemplates",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "CartStatus",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ExamOptions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    OptionText = table.Column<string>(type: "text", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_ExamOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamOptions_ExamQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "public",
                        principalTable: "ExamQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamResults",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamId = table.Column<long>(type: "bigint", nullable: false),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TotalQuestions = table.Column<int>(type: "integer", nullable: false),
                    CorrectAnswers = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<decimal>(type: "numeric", nullable: false),
                    ResultDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    QualifiedCertificate = table.Column<bool>(type: "boolean", nullable: false),
                    CertificateTypeId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_ExamResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamResults_CertificateTypes_CertificateTypeId",
                        column: x => x.CertificateTypeId,
                        principalSchema: "public",
                        principalTable: "CertificateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamResults_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "public",
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExamResults_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamVersions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamId = table.Column<long>(type: "bigint", nullable: false),
                    VersionNumber = table.Column<int>(type: "integer", nullable: false),
                    IsPublished = table.Column<bool>(type: "boolean", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
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
                    table.PrimaryKey("PK_ExamVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamVersions_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "public",
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamOptions_QuestionId",
                schema: "public",
                table: "ExamOptions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_CertificateTypeId",
                schema: "public",
                table: "ExamResults",
                column: "CertificateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_ExamId",
                schema: "public",
                table: "ExamResults",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_UserId",
                schema: "public",
                table: "ExamResults",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamVersions_ExamId",
                schema: "public",
                table: "ExamVersions",
                column: "ExamId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamTopics_ExamVersions_ExamVersionId",
                schema: "public",
                table: "ExamTopics",
                column: "ExamVersionId",
                principalSchema: "public",
                principalTable: "ExamVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_ExamOptions_OptionId",
                schema: "public",
                table: "UserAnswers",
                column: "OptionId",
                principalSchema: "public",
                principalTable: "ExamOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExams_ExamVersions_ExamVersionId",
                schema: "public",
                table: "UserExams",
                column: "ExamVersionId",
                principalSchema: "public",
                principalTable: "ExamVersions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExamTopics_ExamVersions_ExamVersionId",
                schema: "public",
                table: "ExamTopics");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_ExamOptions_OptionId",
                schema: "public",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExams_ExamVersions_ExamVersionId",
                schema: "public",
                table: "UserExams");

            migrationBuilder.DropTable(
                name: "ExamOptions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ExamResults",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ExamVersions",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "ExamStatusId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "PassingScore",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "QuestionText",
                schema: "public",
                table: "ExamQuestions");

            migrationBuilder.DropColumn(
                name: "Score",
                schema: "public",
                table: "ExamQuestions");

            migrationBuilder.RenameColumn(
                name: "ExamVersionId",
                schema: "public",
                table: "UserExams",
                newName: "ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_UserExams_ExamVersionId",
                schema: "public",
                table: "UserExams",
                newName: "IX_UserExams_ExamId");

            migrationBuilder.RenameColumn(
                name: "OptionId",
                schema: "public",
                table: "UserAnswers",
                newName: "AnswerId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswers_OptionId",
                schema: "public",
                table: "UserAnswers",
                newName: "IX_UserAnswers_AnswerId");

            migrationBuilder.RenameColumn(
                name: "ExamVersionId",
                schema: "public",
                table: "ExamTopics",
                newName: "ExamId");

            migrationBuilder.RenameIndex(
                name: "IX_ExamTopics_ExamVersionId",
                schema: "public",
                table: "ExamTopics",
                newName: "IX_ExamTopics_ExamId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "WhatYouWillLearns",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingTypes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingStatuses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingSections",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "Trainings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingLevels",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingContents",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingCategories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingAnnouncementsComments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TrainingAnnouncements",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "TimeUnits",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "Languages",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "InstructorTypes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "ExamQuestions",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Question",
                schema: "public",
                table: "ExamQuestions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "CurrAccTrainingStatuses",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "ContentTypes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "ContentExamAction",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "ContentComments",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "CertificateTypes",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "CertificateTemplates",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "public",
                table: "CartStatus",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ExamAnswers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamAnswers_ExamQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "public",
                        principalTable: "ExamQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamAnswers_QuestionId",
                schema: "public",
                table: "ExamAnswers",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExamTopics_Exams_ExamId",
                schema: "public",
                table: "ExamTopics",
                column: "ExamId",
                principalSchema: "public",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_ExamAnswers_AnswerId",
                schema: "public",
                table: "UserAnswers",
                column: "AnswerId",
                principalSchema: "public",
                principalTable: "ExamAnswers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExams_Exams_ExamId",
                schema: "public",
                table: "UserExams",
                column: "ExamId",
                principalSchema: "public",
                principalTable: "Exams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
