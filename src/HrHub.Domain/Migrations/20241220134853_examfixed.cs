using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class examfixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_ExamOptions_OptionId",
                schema: "public",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_OptionId",
                schema: "public",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "OptionId",
                schema: "public",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "SuccessRate",
                schema: "public",
                table: "UserAnswers");

            migrationBuilder.AlterColumn<decimal>(
                name: "ExamScore",
                schema: "public",
                table: "UserExams",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentTopicSeqNumber",
                schema: "public",
                table: "UserExams",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                schema: "public",
                table: "UserExams",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccess",
                schema: "public",
                table: "UserExams",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PassingScore",
                schema: "public",
                table: "UserExams",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SuccessRate",
                schema: "public",
                table: "UserExams",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalScore",
                schema: "public",
                table: "UserExams",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "SelectedOptionId",
                schema: "public",
                table: "UserAnswers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeqNumber",
                schema: "public",
                table: "UserAnswers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedDate",
                schema: "public",
                table: "ExamVersions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_SelectedOptionId",
                schema: "public",
                table: "UserAnswers",
                column: "SelectedOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_ExamOptions_SelectedOptionId",
                schema: "public",
                table: "UserAnswers",
                column: "SelectedOptionId",
                principalSchema: "public",
                principalTable: "ExamOptions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_ExamOptions_SelectedOptionId",
                schema: "public",
                table: "UserAnswers");

            migrationBuilder.DropIndex(
                name: "IX_UserAnswers_SelectedOptionId",
                schema: "public",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "CurrentTopicSeqNumber",
                schema: "public",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                schema: "public",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "IsSuccess",
                schema: "public",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "PassingScore",
                schema: "public",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "SuccessRate",
                schema: "public",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "TotalScore",
                schema: "public",
                table: "UserExams");

            migrationBuilder.DropColumn(
                name: "SelectedOptionId",
                schema: "public",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "SeqNumber",
                schema: "public",
                table: "UserAnswers");

            migrationBuilder.DropColumn(
                name: "PublishedDate",
                schema: "public",
                table: "ExamVersions");

            migrationBuilder.AlterColumn<long>(
                name: "ExamScore",
                schema: "public",
                table: "UserExams",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OptionId",
                schema: "public",
                table: "UserAnswers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "SuccessRate",
                schema: "public",
                table: "UserAnswers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_OptionId",
                schema: "public",
                table: "UserAnswers",
                column: "OptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_ExamOptions_OptionId",
                schema: "public",
                table: "UserAnswers",
                column: "OptionId",
                principalSchema: "public",
                principalTable: "ExamOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
