using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TrainingContentv4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingContents_TrainingSections_SectionId",
                schema: "public",
                table: "TrainingContents");

            migrationBuilder.DropIndex(
                name: "IX_TrainingContents_SectionId",
                schema: "public",
                table: "TrainingContents");

            migrationBuilder.DropColumn(
                name: "SectionId",
                schema: "public",
                table: "TrainingContents");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingContents_TrainingSectionId",
                schema: "public",
                table: "TrainingContents",
                column: "TrainingSectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingContents_TrainingSections_TrainingSectionId",
                schema: "public",
                table: "TrainingContents",
                column: "TrainingSectionId",
                principalSchema: "public",
                principalTable: "TrainingSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingContents_TrainingSections_TrainingSectionId",
                schema: "public",
                table: "TrainingContents");

            migrationBuilder.DropIndex(
                name: "IX_TrainingContents_TrainingSectionId",
                schema: "public",
                table: "TrainingContents");

            migrationBuilder.AddColumn<long>(
                name: "SectionId",
                schema: "public",
                table: "TrainingContents",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingContents_SectionId",
                schema: "public",
                table: "TrainingContents",
                column: "SectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingContents_TrainingSections_SectionId",
                schema: "public",
                table: "TrainingContents",
                column: "SectionId",
                principalSchema: "public",
                principalTable: "TrainingSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
