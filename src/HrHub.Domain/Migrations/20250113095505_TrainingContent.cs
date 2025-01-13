using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TrainingContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingContents_TrainingContentId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_WhatYouWillLearns_Trainings_TrainingId",
                schema: "public",
                table: "WhatYouWillLearns");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_TrainingContentId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "TrainingContentId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.AlterColumn<long>(
                name: "TrainingId",
                schema: "public",
                table: "WhatYouWillLearns",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_WhatYouWillLearns_Trainings_TrainingId",
                schema: "public",
                table: "WhatYouWillLearns",
                column: "TrainingId",
                principalSchema: "public",
                principalTable: "Trainings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WhatYouWillLearns_Trainings_TrainingId",
                schema: "public",
                table: "WhatYouWillLearns");

            migrationBuilder.AlterColumn<long>(
                name: "TrainingId",
                schema: "public",
                table: "WhatYouWillLearns",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_WhatYouWillLearns_Trainings_TrainingId",
                schema: "public",
                table: "WhatYouWillLearns",
                column: "TrainingId",
                principalSchema: "public",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
