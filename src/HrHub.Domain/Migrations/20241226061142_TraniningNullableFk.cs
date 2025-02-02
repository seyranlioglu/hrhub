using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TraniningNullableFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_ForWhoms_ForWhomId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Instructors_InstructorId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Preconditions_PreconditionId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_PriceTiers_PriceTierId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TimeUnits_CompletionTimeUnitId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingCategories_CategoryId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingLevels_TrainingLevelId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingStatuses_TrainingStatusId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.AlterColumn<string>(
                name: "WelcomeMessage",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "TrainingStatusId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "TrainingLevelId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxRate",
                schema: "public",
                table: "Trainings",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "SubTitle",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "PriceTierId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "PreconditionId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Labels",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "InstructorId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ForWhomId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountRate",
                schema: "public",
                table: "Trainings",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentAmount",
                schema: "public",
                table: "Trainings",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "CourseImage",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CongratulationMessage",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "CompletionTimeUnitId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "CertificateOfParticipationRate",
                schema: "public",
                table: "Trainings",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "public",
                table: "Trainings",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_ForWhoms_ForWhomId",
                schema: "public",
                table: "Trainings",
                column: "ForWhomId",
                principalSchema: "public",
                principalTable: "ForWhoms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Instructors_InstructorId",
                schema: "public",
                table: "Trainings",
                column: "InstructorId",
                principalSchema: "public",
                principalTable: "Instructors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Preconditions_PreconditionId",
                schema: "public",
                table: "Trainings",
                column: "PreconditionId",
                principalSchema: "public",
                principalTable: "Preconditions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_PriceTiers_PriceTierId",
                schema: "public",
                table: "Trainings",
                column: "PriceTierId",
                principalSchema: "public",
                principalTable: "PriceTiers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TimeUnits_CompletionTimeUnitId",
                schema: "public",
                table: "Trainings",
                column: "CompletionTimeUnitId",
                principalSchema: "public",
                principalTable: "TimeUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingCategories_CategoryId",
                schema: "public",
                table: "Trainings",
                column: "CategoryId",
                principalSchema: "public",
                principalTable: "TrainingCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingLevels_TrainingLevelId",
                schema: "public",
                table: "Trainings",
                column: "TrainingLevelId",
                principalSchema: "public",
                principalTable: "TrainingLevels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingStatuses_TrainingStatusId",
                schema: "public",
                table: "Trainings",
                column: "TrainingStatusId",
                principalSchema: "public",
                principalTable: "TrainingStatuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_ForWhoms_ForWhomId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Instructors_InstructorId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Preconditions_PreconditionId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_PriceTiers_PriceTierId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TimeUnits_CompletionTimeUnitId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingCategories_CategoryId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingLevels_TrainingLevelId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingStatuses_TrainingStatusId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.AlterColumn<string>(
                name: "WelcomeMessage",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TrainingStatusId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "TrainingLevelId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "TaxRate",
                schema: "public",
                table: "Trainings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubTitle",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PriceTierId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PreconditionId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Labels",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "InstructorId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ForWhomId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountRate",
                schema: "public",
                table: "Trainings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentAmount",
                schema: "public",
                table: "Trainings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CourseImage",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CongratulationMessage",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CompletionTimeUnitId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CertificateOfParticipationRate",
                schema: "public",
                table: "Trainings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                schema: "public",
                table: "Trainings",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_ForWhoms_ForWhomId",
                schema: "public",
                table: "Trainings",
                column: "ForWhomId",
                principalSchema: "public",
                principalTable: "ForWhoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Instructors_InstructorId",
                schema: "public",
                table: "Trainings",
                column: "InstructorId",
                principalSchema: "public",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Preconditions_PreconditionId",
                schema: "public",
                table: "Trainings",
                column: "PreconditionId",
                principalSchema: "public",
                principalTable: "Preconditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_PriceTiers_PriceTierId",
                schema: "public",
                table: "Trainings",
                column: "PriceTierId",
                principalSchema: "public",
                principalTable: "PriceTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TimeUnits_CompletionTimeUnitId",
                schema: "public",
                table: "Trainings",
                column: "CompletionTimeUnitId",
                principalSchema: "public",
                principalTable: "TimeUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingCategories_CategoryId",
                schema: "public",
                table: "Trainings",
                column: "CategoryId",
                principalSchema: "public",
                principalTable: "TrainingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingLevels_TrainingLevelId",
                schema: "public",
                table: "Trainings",
                column: "TrainingLevelId",
                principalSchema: "public",
                principalTable: "TrainingLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingStatuses_TrainingStatusId",
                schema: "public",
                table: "Trainings",
                column: "TrainingStatusId",
                principalSchema: "public",
                principalTable: "TrainingStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
