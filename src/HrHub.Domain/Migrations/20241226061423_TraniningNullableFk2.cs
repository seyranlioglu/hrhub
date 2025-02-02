using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TraniningNullableFk2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingCategories_CategoryId",
                schema: "public",
                table: "Trainings");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingCategories_CategoryId",
                schema: "public",
                table: "Trainings",
                column: "CategoryId",
                principalSchema: "public",
                principalTable: "TrainingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingCategories_CategoryId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingCategories_CategoryId",
                schema: "public",
                table: "Trainings",
                column: "CategoryId",
                principalSchema: "public",
                principalTable: "TrainingCategories",
                principalColumn: "Id");
        }
    }
}
