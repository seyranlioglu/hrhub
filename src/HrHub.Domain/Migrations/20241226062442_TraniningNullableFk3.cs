using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TraniningNullableFk3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_EducationLevels_EducationLevelId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.AlterColumn<string>(
                name: "Trailer",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "EducationLevelId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_EducationLevels_EducationLevelId",
                schema: "public",
                table: "Trainings",
                column: "EducationLevelId",
                principalSchema: "public",
                principalTable: "EducationLevels",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_EducationLevels_EducationLevelId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.AlterColumn<string>(
                name: "Trailer",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "EducationLevelId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_EducationLevels_EducationLevelId",
                schema: "public",
                table: "Trainings",
                column: "EducationLevelId",
                principalSchema: "public",
                principalTable: "EducationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
