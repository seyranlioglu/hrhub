using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class ContentLibrary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "InstructorId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<long>(
                name: "TrainingStatusId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "ContentLibraries",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    TrainingContentId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentLibraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentLibraries_TrainingContents_TrainingContentId",
                        column: x => x.TrainingContentId,
                        principalSchema: "public",
                        principalTable: "TrainingContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TrainingTrainingStatus",
                schema: "public",
                columns: table => new
                {
                    TrainingStatusId = table.Column<long>(type: "bigint", nullable: false),
                    TrainingStatusesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTrainingStatus", x => new { x.TrainingStatusId, x.TrainingStatusesId });
                    table.ForeignKey(
                        name: "FK_TrainingTrainingStatus_TrainingStatuses_TrainingStatusesId",
                        column: x => x.TrainingStatusesId,
                        principalSchema: "public",
                        principalTable: "TrainingStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TrainingTrainingStatus_Trainings_TrainingStatusId",
                        column: x => x.TrainingStatusId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentLibraries_TrainingContentId",
                schema: "public",
                table: "ContentLibraries",
                column: "TrainingContentId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingTrainingStatus_TrainingStatusesId",
                schema: "public",
                table: "TrainingTrainingStatus",
                column: "TrainingStatusesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentLibraries",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TrainingTrainingStatus",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "TrainingStatusId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.AlterColumn<int>(
                name: "InstructorId",
                schema: "public",
                table: "Trainings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                schema: "public",
                table: "Trainings",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
