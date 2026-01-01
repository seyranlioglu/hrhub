using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class certTypeAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContentsViewLogs_CurrAccs_CurrAccTrainingUserId",
                schema: "public",
                table: "UserContentsViewLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContentsViewLogs_UserContentsViewLogs_UserContentsViewL~",
                schema: "public",
                table: "UserContentsViewLogs");

            migrationBuilder.RenameColumn(
                name: "UserContentsViewLogId",
                schema: "public",
                table: "UserContentsViewLogs",
                newName: "CurrAccId");

            migrationBuilder.RenameColumn(
                name: "State",
                schema: "public",
                table: "UserContentsViewLogs",
                newName: "IsCompleted");

            migrationBuilder.RenameIndex(
                name: "IX_UserContentsViewLogs_UserContentsViewLogId",
                schema: "public",
                table: "UserContentsViewLogs",
                newName: "IX_UserContentsViewLogs_CurrAccId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedDate",
                schema: "public",
                table: "UserContentsViewLogs",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<string>(
                name: "GeneratedFilePath",
                schema: "public",
                table: "UserCertificates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                schema: "public",
                table: "CertificateTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinScore",
                schema: "public",
                table: "CertificateTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContentsViewLogs_CurrAccTrainingUsers_CurrAccTrainingUs~",
                schema: "public",
                table: "UserContentsViewLogs",
                column: "CurrAccTrainingUserId",
                principalSchema: "public",
                principalTable: "CurrAccTrainingUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContentsViewLogs_CurrAccs_CurrAccId",
                schema: "public",
                table: "UserContentsViewLogs",
                column: "CurrAccId",
                principalSchema: "public",
                principalTable: "CurrAccs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserContentsViewLogs_CurrAccTrainingUsers_CurrAccTrainingUs~",
                schema: "public",
                table: "UserContentsViewLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_UserContentsViewLogs_CurrAccs_CurrAccId",
                schema: "public",
                table: "UserContentsViewLogs");

            migrationBuilder.DropColumn(
                name: "GeneratedFilePath",
                schema: "public",
                table: "UserCertificates");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                schema: "public",
                table: "CertificateTypes");

            migrationBuilder.DropColumn(
                name: "MinScore",
                schema: "public",
                table: "CertificateTypes");

            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                schema: "public",
                table: "UserContentsViewLogs",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "CurrAccId",
                schema: "public",
                table: "UserContentsViewLogs",
                newName: "UserContentsViewLogId");

            migrationBuilder.RenameIndex(
                name: "IX_UserContentsViewLogs_CurrAccId",
                schema: "public",
                table: "UserContentsViewLogs",
                newName: "IX_UserContentsViewLogs_UserContentsViewLogId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CompletedDate",
                schema: "public",
                table: "UserContentsViewLogs",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContentsViewLogs_CurrAccs_CurrAccTrainingUserId",
                schema: "public",
                table: "UserContentsViewLogs",
                column: "CurrAccTrainingUserId",
                principalSchema: "public",
                principalTable: "CurrAccs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserContentsViewLogs_UserContentsViewLogs_UserContentsViewL~",
                schema: "public",
                table: "UserContentsViewLogs",
                column: "UserContentsViewLogId",
                principalSchema: "public",
                principalTable: "UserContentsViewLogs",
                principalColumn: "Id");
        }
    }
}
