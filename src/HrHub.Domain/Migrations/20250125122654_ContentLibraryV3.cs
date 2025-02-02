using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class ContentLibraryV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentLibraries_TrainingContents_TrainingContentId",
                schema: "public",
                table: "ContentLibraries");

            migrationBuilder.AlterColumn<long>(
                name: "TrainingContentId",
                schema: "public",
                table: "ContentLibraries",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentLibraries_TrainingContents_TrainingContentId",
                schema: "public",
                table: "ContentLibraries",
                column: "TrainingContentId",
                principalSchema: "public",
                principalTable: "TrainingContents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentLibraries_TrainingContents_TrainingContentId",
                schema: "public",
                table: "ContentLibraries");

            migrationBuilder.AlterColumn<long>(
                name: "TrainingContentId",
                schema: "public",
                table: "ContentLibraries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentLibraries_TrainingContents_TrainingContentId",
                schema: "public",
                table: "ContentLibraries",
                column: "TrainingContentId",
                principalSchema: "public",
                principalTable: "TrainingContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
