using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TrainingType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainingType",
                schema: "public",
                table: "Trainings");

            migrationBuilder.AddColumn<long>(
                name: "TrainingTypeId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_TrainingTypeId",
                schema: "public",
                table: "Trainings",
                column: "TrainingTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingTypes_TrainingTypeId",
                schema: "public",
                table: "Trainings",
                column: "TrainingTypeId",
                principalSchema: "public",
                principalTable: "TrainingTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingTypes_TrainingTypeId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_TrainingTypeId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "TrainingTypeId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.AddColumn<string>(
                name: "TrainingType",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: true);
        }
    }
}
