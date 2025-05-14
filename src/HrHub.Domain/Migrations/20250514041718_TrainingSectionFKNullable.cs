using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TrainingSectionFKNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingSections_Trainings_TrainingId",
                schema: "public",
                table: "TrainingSections");

            migrationBuilder.AlterColumn<long>(
                name: "TrainingId",
                schema: "public",
                table: "TrainingSections",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingSections_Trainings_TrainingId",
                schema: "public",
                table: "TrainingSections",
                column: "TrainingId",
                principalSchema: "public",
                principalTable: "Trainings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingSections_Trainings_TrainingId",
                schema: "public",
                table: "TrainingSections");

            migrationBuilder.AlterColumn<long>(
                name: "TrainingId",
                schema: "public",
                table: "TrainingSections",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingSections_Trainings_TrainingId",
                schema: "public",
                table: "TrainingSections",
                column: "TrainingId",
                principalSchema: "public",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
