using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class ContentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TrainingContentId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_TrainingContentId",
                schema: "public",
                table: "Trainings",
                column: "TrainingContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingContents_TrainingContentId",
                schema: "public",
                table: "Trainings",
                column: "TrainingContentId",
                principalSchema: "public",
                principalTable: "TrainingContents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingContents_TrainingContentId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_TrainingContentId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "TrainingContentId",
                schema: "public",
                table: "Trainings");
        }
    }
}
