using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TrainingLanguageFK2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingLanguages_Trainings_TrainingId",
                schema: "public",
                table: "TrainingLanguages");

            migrationBuilder.DropIndex(
                name: "IX_TrainingLanguages_TrainingId",
                schema: "public",
                table: "TrainingLanguages");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                schema: "public",
                table: "TrainingLanguages");

            migrationBuilder.AddColumn<long>(
                name: "TrainingLanguageId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_TrainingLanguageId",
                schema: "public",
                table: "Trainings",
                column: "TrainingLanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingLanguages_TrainingLanguageId",
                schema: "public",
                table: "Trainings",
                column: "TrainingLanguageId",
                principalSchema: "public",
                principalTable: "TrainingLanguages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingLanguages_TrainingLanguageId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_TrainingLanguageId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "TrainingLanguageId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.AddColumn<long>(
                name: "TrainingId",
                schema: "public",
                table: "TrainingLanguages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingLanguages_TrainingId",
                schema: "public",
                table: "TrainingLanguages",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingLanguages_Trainings_TrainingId",
                schema: "public",
                table: "TrainingLanguages",
                column: "TrainingId",
                principalSchema: "public",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
