using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TrainingLanguageAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Exams_Trainings_TrainingId",
            //    schema: "public",
            //    table: "Exams");

            //migrationBuilder.DropIndex(
            //    name: "IX_Exams_TrainingId",
            //    schema: "public",
            //    table: "Exams");

            //migrationBuilder.DropColumn(
            //    name: "TrainingId",
            //    schema: "public",
            //    table: "Exams");

            migrationBuilder.CreateTable(
                name: "TrainingLanguages",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrainingId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_TrainingLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingLanguages_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingLanguages_TrainingId",
                schema: "public",
                table: "TrainingLanguages",
                column: "TrainingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "TrainingLanguages",
            //    schema: "public");

            //migrationBuilder.AddColumn<long>(
            //    name: "TrainingId",
            //    schema: "public",
            //    table: "Exams",
            //    type: "bigint",
            //    nullable: false,
            //    defaultValue: 0L);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Exams_TrainingId",
            //    schema: "public",
            //    table: "Exams",
            //    column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Trainings_TrainingId",
                schema: "public",
                table: "Exams",
                column: "TrainingId",
                principalSchema: "public",
                principalTable: "Trainings",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
