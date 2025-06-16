using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class fixExam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exams_Trainings_TrainingId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropIndex(
                name: "IX_Exams_TrainingId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                schema: "public",
                table: "Exams");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TrainingId",
                schema: "public",
                table: "Exams",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Exams_TrainingId",
                schema: "public",
                table: "Exams",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Trainings_TrainingId",
                schema: "public",
                table: "Exams",
                column: "TrainingId",
                principalSchema: "public",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
