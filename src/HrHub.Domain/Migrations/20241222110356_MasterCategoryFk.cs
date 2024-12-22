using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class MasterCategoryFk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingCategories_TrainingCategories_MasterCategoryId",
                schema: "public",
                table: "TrainingCategories");

            migrationBuilder.AlterColumn<long>(
                name: "MasterCategoryId",
                schema: "public",
                table: "TrainingCategories",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCategories_TrainingCategories_MasterCategoryId",
                schema: "public",
                table: "TrainingCategories",
                column: "MasterCategoryId",
                principalSchema: "public",
                principalTable: "TrainingCategories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingCategories_TrainingCategories_MasterCategoryId",
                schema: "public",
                table: "TrainingCategories");

            migrationBuilder.AlterColumn<long>(
                name: "MasterCategoryId",
                schema: "public",
                table: "TrainingCategories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCategories_TrainingCategories_MasterCategoryId",
                schema: "public",
                table: "TrainingCategories",
                column: "MasterCategoryId",
                principalSchema: "public",
                principalTable: "TrainingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
