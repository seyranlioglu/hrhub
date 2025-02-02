using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class CurrAccTraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrAccTrainings_AspNetUsers_ConfirmUserId",
                schema: "public",
                table: "CurrAccTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrAccTrainings_CurrAccTrainingStatuses_StatusId",
                schema: "public",
                table: "CurrAccTrainings");

            migrationBuilder.DropIndex(
                name: "IX_CurrAccTrainings_StatusId",
                schema: "public",
                table: "CurrAccTrainings");

            migrationBuilder.DropColumn(
                name: "StatusId",
                schema: "public",
                table: "CurrAccTrainings");

            migrationBuilder.AlterColumn<int>(
                name: "LicenceCount",
                schema: "public",
                table: "CurrAccTrainings",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "ConfirmUserId",
                schema: "public",
                table: "CurrAccTrainings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmNotes",
                schema: "public",
                table: "CurrAccTrainings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "CartItemId",
                schema: "public",
                table: "CurrAccTrainings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.CreateIndex(
                name: "IX_CurrAccTrainings_CurrAccTrainingStatusId",
                schema: "public",
                table: "CurrAccTrainings",
                column: "CurrAccTrainingStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrAccTrainings_AspNetUsers_ConfirmUserId",
                schema: "public",
                table: "CurrAccTrainings",
                column: "ConfirmUserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrAccTrainings_CurrAccTrainingStatuses_CurrAccTrainingSta~",
                schema: "public",
                table: "CurrAccTrainings",
                column: "CurrAccTrainingStatusId",
                principalSchema: "public",
                principalTable: "CurrAccTrainingStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrAccTrainings_AspNetUsers_ConfirmUserId",
                schema: "public",
                table: "CurrAccTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrAccTrainings_CurrAccTrainingStatuses_CurrAccTrainingSta~",
                schema: "public",
                table: "CurrAccTrainings");

            migrationBuilder.DropIndex(
                name: "IX_CurrAccTrainings_CurrAccTrainingStatusId",
                schema: "public",
                table: "CurrAccTrainings");

            migrationBuilder.AlterColumn<int>(
                name: "LicenceCount",
                schema: "public",
                table: "CurrAccTrainings",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ConfirmUserId",
                schema: "public",
                table: "CurrAccTrainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmNotes",
                schema: "public",
                table: "CurrAccTrainings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CartItemId",
                schema: "public",
                table: "CurrAccTrainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StatusId",
                schema: "public",
                table: "CurrAccTrainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_CurrAccTrainings_StatusId",
                schema: "public",
                table: "CurrAccTrainings",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrAccTrainings_AspNetUsers_ConfirmUserId",
                schema: "public",
                table: "CurrAccTrainings",
                column: "ConfirmUserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrAccTrainings_CurrAccTrainingStatuses_StatusId",
                schema: "public",
                table: "CurrAccTrainings",
                column: "StatusId",
                principalSchema: "public",
                principalTable: "CurrAccTrainingStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
