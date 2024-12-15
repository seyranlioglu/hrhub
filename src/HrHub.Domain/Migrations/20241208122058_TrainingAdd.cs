using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class TrainingAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingCategories_Trainings_CategoryId",
                schema: "public",
                table: "TrainingCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingLevels_TrainingLevels_TrainingLevelId1",
                schema: "public",
                table: "TrainingLevels");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingLevels_Trainings_TrainingLevelId",
                schema: "public",
                table: "TrainingLevels");

            migrationBuilder.DropTable(
                name: "InstructorTraining",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TimeUnitTraining",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_TrainingLevels_TrainingLevelId",
                schema: "public",
                table: "TrainingLevels");

            migrationBuilder.DropIndex(
                name: "IX_TrainingLevels_TrainingLevelId1",
                schema: "public",
                table: "TrainingLevels");

            migrationBuilder.DropIndex(
                name: "IX_TrainingCategories_CategoryId",
                schema: "public",
                table: "TrainingCategories");

            migrationBuilder.DropColumn(
                name: "TrainingLevelId",
                schema: "public",
                table: "TrainingLevels");

            migrationBuilder.DropColumn(
                name: "TrainingLevelId1",
                schema: "public",
                table: "TrainingLevels");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                schema: "public",
                table: "TrainingCategories");

            migrationBuilder.AddColumn<int>(
                name: "AccessFailedCount",
                schema: "public",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AuthCode",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                schema: "public",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LockoutEnabled",
                schema: "public",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LockoutEnd",
                schema: "public",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedUserName",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfirmed",
                schema: "public",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SecurityStamp",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SurName",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "TwoFactorEnabled",
                schema: "public",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserShortName",
                schema: "public",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<string>(
                name: "CongratulationMessage",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CourseImage",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "EducationLevelId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ForWhomId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Labels",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "PreconditionId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "PriceTierId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Trailer",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: false,
                defaultValue: "");

    

            migrationBuilder.AddColumn<string>(
                name: "WelcomeMessage",
                schema: "public",
                table: "Trainings",
                type: "text",
                nullable: false,
                defaultValue: "");

           

            migrationBuilder.CreateTable(
                name: "EducationLevels",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_EducationLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForWhoms",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_ForWhoms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Preconditions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_Preconditions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PriceTiers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_PriceTiers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_CategoryId",
                schema: "public",
                table: "Trainings",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_CompletionTimeUnitId",
                schema: "public",
                table: "Trainings",
                column: "CompletionTimeUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_EducationLevelId",
                schema: "public",
                table: "Trainings",
                column: "EducationLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_ForWhomId",
                schema: "public",
                table: "Trainings",
                column: "ForWhomId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_InstructorId",
                schema: "public",
                table: "Trainings",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_PreconditionId",
                schema: "public",
                table: "Trainings",
                column: "PreconditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_PriceTierId",
                schema: "public",
                table: "Trainings",
                column: "PriceTierId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_TrainingLevelId",
                schema: "public",
                table: "Trainings",
                column: "TrainingLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainings_TrainingStatusId",
                schema: "public",
                table: "Trainings",
                column: "TrainingStatusId");


            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_EducationLevels_EducationLevelId",
                schema: "public",
                table: "Trainings",
                column: "EducationLevelId",
                principalSchema: "public",
                principalTable: "EducationLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_ForWhoms_ForWhomId",
                schema: "public",
                table: "Trainings",
                column: "ForWhomId",
                principalSchema: "public",
                principalTable: "ForWhoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Instructors_InstructorId",
                schema: "public",
                table: "Trainings",
                column: "InstructorId",
                principalSchema: "public",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_Preconditions_PreconditionId",
                schema: "public",
                table: "Trainings",
                column: "PreconditionId",
                principalSchema: "public",
                principalTable: "Preconditions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_PriceTiers_PriceTierId",
                schema: "public",
                table: "Trainings",
                column: "PriceTierId",
                principalSchema: "public",
                principalTable: "PriceTiers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TimeUnits_CompletionTimeUnitId",
                schema: "public",
                table: "Trainings",
                column: "CompletionTimeUnitId",
                principalSchema: "public",
                principalTable: "TimeUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingCategories_CategoryId",
                schema: "public",
                table: "Trainings",
                column: "CategoryId",
                principalSchema: "public",
                principalTable: "TrainingCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingLevels_TrainingLevelId",
                schema: "public",
                table: "Trainings",
                column: "TrainingLevelId",
                principalSchema: "public",
                principalTable: "TrainingLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingStatuses_TrainingStatusId",
                schema: "public",
                table: "Trainings",
                column: "TrainingStatusId",
                principalSchema: "public",
                principalTable: "TrainingStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_EducationLevels_EducationLevelId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_ForWhoms_ForWhomId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Instructors_InstructorId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_Preconditions_PreconditionId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_PriceTiers_PriceTierId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TimeUnits_CompletionTimeUnitId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingCategories_CategoryId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingLevels_TrainingLevelId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingStatuses_TrainingStatusId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropTable(
                name: "ContentLibraries",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EducationLevels",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ForWhoms",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Preconditions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "PriceTiers",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_CategoryId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_CompletionTimeUnitId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_EducationLevelId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_ForWhomId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_InstructorId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_PreconditionId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_PriceTierId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_TrainingLevelId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropIndex(
                name: "IX_Trainings_TrainingStatusId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "AccessFailedCount",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AuthCode",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnabled",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LockoutEnd",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "NormalizedUserName",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PhoneNumberConfirmed",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SecurityStamp",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SurName",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TwoFactorEnabled",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserShortName",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CongratulationMessage",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "CourseImage",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "EducationLevelId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "ForWhomId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Labels",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "PreconditionId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "PriceTierId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "SubTitle",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "Trailer",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "TrainingStatusId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "WelcomeMessage",
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

            migrationBuilder.AddColumn<long>(
                name: "TrainingLevelId",
                schema: "public",
                table: "TrainingLevels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TrainingLevelId1",
                schema: "public",
                table: "TrainingLevels",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CategoryId",
                schema: "public",
                table: "TrainingCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InstructorTraining",
                schema: "public",
                columns: table => new
                {
                    InstructorId = table.Column<long>(type: "bigint", nullable: false),
                    InstructorsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorTraining", x => new { x.InstructorId, x.InstructorsId });
                    table.ForeignKey(
                        name: "FK_InstructorTraining_Instructors_InstructorsId",
                        column: x => x.InstructorsId,
                        principalSchema: "public",
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InstructorTraining_Trainings_InstructorId",
                        column: x => x.InstructorId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TimeUnitTraining",
                schema: "public",
                columns: table => new
                {
                    CompletionTimeUnitId = table.Column<long>(type: "bigint", nullable: false),
                    TimeUnitsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeUnitTraining", x => new { x.CompletionTimeUnitId, x.TimeUnitsId });
                    table.ForeignKey(
                        name: "FK_TimeUnitTraining_TimeUnits_TimeUnitsId",
                        column: x => x.TimeUnitsId,
                        principalSchema: "public",
                        principalTable: "TimeUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TimeUnitTraining_Trainings_CompletionTimeUnitId",
                        column: x => x.CompletionTimeUnitId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingLevels_TrainingLevelId",
                schema: "public",
                table: "TrainingLevels",
                column: "TrainingLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingLevels_TrainingLevelId1",
                schema: "public",
                table: "TrainingLevels",
                column: "TrainingLevelId1");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCategories_CategoryId",
                schema: "public",
                table: "TrainingCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorTraining_InstructorsId",
                schema: "public",
                table: "InstructorTraining",
                column: "InstructorsId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeUnitTraining_TimeUnitsId",
                schema: "public",
                table: "TimeUnitTraining",
                column: "TimeUnitsId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCategories_Trainings_CategoryId",
                schema: "public",
                table: "TrainingCategories",
                column: "CategoryId",
                principalSchema: "public",
                principalTable: "Trainings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingLevels_TrainingLevels_TrainingLevelId1",
                schema: "public",
                table: "TrainingLevels",
                column: "TrainingLevelId1",
                principalSchema: "public",
                principalTable: "TrainingLevels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingLevels_Trainings_TrainingLevelId",
                schema: "public",
                table: "TrainingLevels",
                column: "TrainingLevelId",
                principalSchema: "public",
                principalTable: "Trainings",
                principalColumn: "Id");
        }
    }
}
