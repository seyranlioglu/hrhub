using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "CartStatus",
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
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CertificateTypes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LangCode = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentTypes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LangCode = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrAccs",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    TaxNumber = table.Column<string>(type: "text", nullable: false),
                    IdentityNumber = table.Column<string>(type: "text", nullable: false),
                    PostalCode = table.Column<string>(type: "text", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrAccs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CurrAccTrainingStatuses",
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
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrAccTrainingStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstructorTypes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LangCode = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LangCode = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeUnits",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LangCode = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trainings",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HeaderImage = table.Column<string>(type: "text", nullable: true),
                    LangCode = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    InstructorId = table.Column<int>(type: "integer", nullable: false),
                    TrainingType = table.Column<string>(type: "text", nullable: true),
                    CurrentAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountRate = table.Column<decimal>(type: "numeric", nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric", nullable: false),
                    CertificateOfAchievementRate = table.Column<decimal>(type: "numeric", nullable: false),
                    CertificateOfParticipationRate = table.Column<decimal>(type: "numeric", nullable: false),
                    CompletionTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CompletionTimeUnitId = table.Column<long>(type: "bigint", nullable: false),
                    TrainingLevelId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingStatuses",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LangCode = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingTypes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LangCode = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CertificateTemplates",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CertificateTypeId = table.Column<long>(type: "bigint", nullable: false),
                    TemplatePath = table.Column<string>(type: "text", nullable: true),
                    VerificationURL = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateTemplates_CertificateTypes_CertificateTypeId",
                        column: x => x.CertificateTypeId,
                        principalSchema: "public",
                        principalTable: "CertificateTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrAccId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_CurrAccs_CurrAccId",
                        column: x => x.CurrAccId,
                        principalSchema: "public",
                        principalTable: "CurrAccs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Exams",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrainingId = table.Column<long>(type: "bigint", nullable: false),
                    ExamTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    SuccesRate = table.Column<long>(type: "bigint", nullable: false),
                    ViewQuestionCount = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exams_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TimeUnitTraining_Trainings_CompletionTimeUnitId",
                        column: x => x.CompletionTimeUnitId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingAmounts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TrainingId = table.Column<long>(type: "bigint", nullable: false),
                    MinLicenceCount = table.Column<int>(type: "integer", nullable: false),
                    MaxLicenceCount = table.Column<int>(type: "integer", nullable: false),
                    AmountPerLicence = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountRate = table.Column<decimal>(type: "numeric", nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingAmounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingAmounts_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingCategories",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MasterCategoryId = table.Column<long>(type: "bigint", nullable: false),
                    CategoryId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingCategories_TrainingCategories_MasterCategoryId",
                        column: x => x.MasterCategoryId,
                        principalSchema: "public",
                        principalTable: "TrainingCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingCategories_Trainings_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingLevels",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Priority = table.Column<long>(type: "bigint", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    TrainingLevelId = table.Column<long>(type: "bigint", nullable: true),
                    TrainingLevelId1 = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingLevels_TrainingLevels_TrainingLevelId1",
                        column: x => x.TrainingLevelId1,
                        principalSchema: "public",
                        principalTable: "TrainingLevels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingLevels_Trainings_TrainingLevelId",
                        column: x => x.TrainingLevelId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingSections",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrainingId = table.Column<long>(type: "bigint", nullable: false),
                    RowNumber = table.Column<long>(type: "bigint", nullable: false),
                    LangCode = table.Column<string>(type: "text", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingSections_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WhatYouWillLearns",
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
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WhatYouWillLearns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WhatYouWillLearns_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrAccId = table.Column<long>(type: "bigint", nullable: false),
                    PromotionCode = table.Column<string>(type: "text", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CardStatusId = table.Column<long>(type: "bigint", nullable: false),
                    ConfirmDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConfirmUserId = table.Column<long>(type: "bigint", nullable: false),
                    ConfirmNotes = table.Column<string>(type: "text", nullable: false),
                    AddCartUserId = table.Column<long>(type: "bigint", nullable: false),
                    StatusId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_CartStatus_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "public",
                        principalTable: "CartStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_CurrAccs_CurrAccId",
                        column: x => x.CurrAccId,
                        principalSchema: "public",
                        principalTable: "CurrAccs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_Users_AddCartUserId",
                        column: x => x.AddCartUserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carts_Users_ConfirmUserId",
                        column: x => x.ConfirmUserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrAccTrainings",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrAccId = table.Column<long>(type: "bigint", nullable: false),
                    TrainingId = table.Column<long>(type: "bigint", nullable: false),
                    CurrAccTrainingStatusId = table.Column<long>(type: "bigint", nullable: false),
                    ConfirmDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ConfirmUserId = table.Column<long>(type: "bigint", nullable: false),
                    ConfirmNotes = table.Column<string>(type: "text", nullable: false),
                    LicenceCount = table.Column<int>(type: "integer", nullable: false),
                    CartItemId = table.Column<long>(type: "bigint", nullable: false),
                    StatusId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrAccTrainings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrAccTrainings_CurrAccTrainingStatuses_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "public",
                        principalTable: "CurrAccTrainingStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrAccTrainings_CurrAccs_CurrAccId",
                        column: x => x.CurrAccId,
                        principalSchema: "public",
                        principalTable: "CurrAccs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrAccTrainings_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrAccTrainings_Users_ConfirmUserId",
                        column: x => x.ConfirmUserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PicturePath = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Facebook = table.Column<string>(type: "text", nullable: false),
                    Linkedin = table.Column<string>(type: "text", nullable: false),
                    Twitter = table.Column<string>(type: "text", nullable: false),
                    Instagram = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    InstructorTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instructors_InstructorTypes_InstructorTypeId",
                        column: x => x.InstructorTypeId,
                        principalSchema: "public",
                        principalTable: "InstructorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Instructors_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrainingId = table.Column<long>(type: "bigint", nullable: false),
                    InstructorPoint = table.Column<long>(type: "bigint", nullable: false),
                    TrainingPoint = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    InstructorReview = table.Column<string>(type: "text", nullable: false),
                    TrainingReview = table.Column<string>(type: "text", nullable: false),
                    CommentedUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_CommentedUserId",
                        column: x => x.CommentedUserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingAnnouncements",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrainingId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingAnnouncements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingAnnouncements_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingAnnouncements_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamTopics",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamId = table.Column<long>(type: "bigint", nullable: false),
                    ImgPath = table.Column<string>(type: "text", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamTopics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamTopics_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "public",
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingContents",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrainingSectionId = table.Column<long>(type: "bigint", nullable: false),
                    ContentTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Time = table.Column<TimeSpan>(type: "interval", nullable: false),
                    PageCount = table.Column<int>(type: "integer", nullable: false),
                    CompletedRate = table.Column<decimal>(type: "numeric", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    Mandatory = table.Column<bool>(type: "boolean", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    AllowSeeking = table.Column<bool>(type: "boolean", nullable: false),
                    PartCount = table.Column<int>(type: "integer", nullable: false),
                    MinReadTimeThreshold = table.Column<int>(type: "integer", nullable: false),
                    SectionId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingContents_ContentTypes_ContentTypeId",
                        column: x => x.ContentTypeId,
                        principalSchema: "public",
                        principalTable: "ContentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingContents_TrainingSections_SectionId",
                        column: x => x.SectionId,
                        principalSchema: "public",
                        principalTable: "TrainingSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItem",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CartId = table.Column<long>(type: "bigint", nullable: false),
                    TrainingId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    DiscountRate = table.Column<decimal>(type: "numeric", nullable: false),
                    TaxRate = table.Column<decimal>(type: "numeric", nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    LicenceCount = table.Column<int>(type: "integer", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItem_Carts_CartId",
                        column: x => x.CartId,
                        principalSchema: "public",
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItem_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CurrAccTrainingUsers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrAccTrainingsId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    QualifiedCertificate = table.Column<bool>(type: "boolean", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrAccTrainingUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CurrAccTrainingUsers_CurrAccTrainings_CurrAccTrainingsId",
                        column: x => x.CurrAccTrainingsId,
                        principalSchema: "public",
                        principalTable: "CurrAccTrainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CurrAccTrainingUsers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InstructorTraining_Trainings_InstructorId",
                        column: x => x.InstructorId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingAnnouncementsComments",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrainingAnnouncementsId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingAnnouncementsComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingAnnouncementsComments_TrainingAnnouncements_Trainin~",
                        column: x => x.TrainingAnnouncementsId,
                        principalSchema: "public",
                        principalTable: "TrainingAnnouncements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingAnnouncementsComments_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamQuestions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamTopicId = table.Column<long>(type: "bigint", nullable: false),
                    Question = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamQuestions_ExamTopics_ExamTopicId",
                        column: x => x.ExamTopicId,
                        principalSchema: "public",
                        principalTable: "ExamTopics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentComments",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentId = table.Column<long>(type: "bigint", nullable: false),
                    Pinned = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ContentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MasterContentId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentComments_ContentComments_MasterContentId",
                        column: x => x.MasterContentId,
                        principalSchema: "public",
                        principalTable: "ContentComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentComments_TrainingContents_ContentId",
                        column: x => x.ContentId,
                        principalSchema: "public",
                        principalTable: "TrainingContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentComments_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentNotes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentId = table.Column<long>(type: "bigint", nullable: false),
                    NoteTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    TrainingContentId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentNotes_TrainingContents_TrainingContentId",
                        column: x => x.TrainingContentId,
                        principalSchema: "public",
                        principalTable: "TrainingContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentNotes_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContentsViewLogs",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrainingContentId = table.Column<long>(type: "bigint", nullable: false),
                    CurrAccTrainingUserId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    State = table.Column<bool>(type: "boolean", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserContentsViewLogId = table.Column<long>(type: "bigint", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContentsViewLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContentsViewLogs_CurrAccs_CurrAccTrainingUserId",
                        column: x => x.CurrAccTrainingUserId,
                        principalSchema: "public",
                        principalTable: "CurrAccs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContentsViewLogs_TrainingContents_TrainingContentId",
                        column: x => x.TrainingContentId,
                        principalSchema: "public",
                        principalTable: "TrainingContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserContentsViewLogs_UserContentsViewLogs_UserContentsViewL~",
                        column: x => x.UserContentsViewLogId,
                        principalSchema: "public",
                        principalTable: "UserContentsViewLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserCertificates",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrAccTrainingUsersId = table.Column<long>(type: "bigint", nullable: false),
                    CertificateTemplateId = table.Column<long>(type: "bigint", nullable: false),
                    CertificateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ConstructorName = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TrainerName = table.Column<string>(type: "text", nullable: false),
                    TrainerTitle = table.Column<string>(type: "text", nullable: false),
                    NumberOfLecture = table.Column<long>(type: "bigint", nullable: false),
                    ProviderName = table.Column<string>(type: "text", nullable: false),
                    ProviderTitle = table.Column<string>(type: "text", nullable: false),
                    VerificationURL = table.Column<string>(type: "text", nullable: false),
                    CertificateId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCertificates_CertificateTemplates_CertificateTemplateId",
                        column: x => x.CertificateTemplateId,
                        principalSchema: "public",
                        principalTable: "CertificateTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCertificates_CurrAccTrainingUsers_CurrAccTrainingUsersId",
                        column: x => x.CurrAccTrainingUsersId,
                        principalSchema: "public",
                        principalTable: "CurrAccTrainingUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExams",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExamId = table.Column<long>(type: "bigint", nullable: false),
                    CurrAccTrainingUserId = table.Column<long>(type: "bigint", nullable: false),
                    TotalAnswer = table.Column<long>(type: "bigint", nullable: true),
                    TotalCorrectAnswer = table.Column<long>(type: "bigint", nullable: true),
                    ExamScore = table.Column<long>(type: "bigint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserExams_CurrAccTrainingUsers_CurrAccTrainingUserId",
                        column: x => x.CurrAccTrainingUserId,
                        principalSchema: "public",
                        principalTable: "CurrAccTrainingUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserExams_Exams_ExamId",
                        column: x => x.ExamId,
                        principalSchema: "public",
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExamAnswers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Abbreviation = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamAnswers_ExamQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "public",
                        principalTable: "ExamQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentVotes",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentCommentId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentVotes_ContentComments_ContentCommentId",
                        column: x => x.ContentCommentId,
                        principalSchema: "public",
                        principalTable: "ContentComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentVotes_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserContentsViewLogDetails",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserContentsViewLogId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    PartNumber = table.Column<int>(type: "integer", nullable: false),
                    LogDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserContentsViewLogDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserContentsViewLogDetails_UserContentsViewLogs_UserContent~",
                        column: x => x.UserContentsViewLogId,
                        principalSchema: "public",
                        principalTable: "UserContentsViewLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAnswers",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserExamId = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    AnswerId = table.Column<long>(type: "bigint", nullable: false),
                    AnswerDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SuccessRate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnswers_ExamAnswers_AnswerId",
                        column: x => x.AnswerId,
                        principalSchema: "public",
                        principalTable: "ExamAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswers_ExamQuestions_QuestionId",
                        column: x => x.QuestionId,
                        principalSchema: "public",
                        principalTable: "ExamQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswers_UserExams_UserExamId",
                        column: x => x.UserExamId,
                        principalSchema: "public",
                        principalTable: "UserExams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_CartId",
                schema: "public",
                table: "CartItem",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItem_TrainingId",
                schema: "public",
                table: "CartItem",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_AddCartUserId",
                schema: "public",
                table: "Carts",
                column: "AddCartUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ConfirmUserId",
                schema: "public",
                table: "Carts",
                column: "ConfirmUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_CurrAccId",
                schema: "public",
                table: "Carts",
                column: "CurrAccId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_StatusId",
                schema: "public",
                table: "Carts",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateTemplates_CertificateTypeId",
                schema: "public",
                table: "CertificateTemplates",
                column: "CertificateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVotes_ContentCommentId",
                schema: "public",
                table: "CommentVotes",
                column: "ContentCommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentVotes_UserId",
                schema: "public",
                table: "CommentVotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentComments_ContentId",
                schema: "public",
                table: "ContentComments",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentComments_MasterContentId",
                schema: "public",
                table: "ContentComments",
                column: "MasterContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentComments_UserId",
                schema: "public",
                table: "ContentComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentNotes_TrainingContentId",
                schema: "public",
                table: "ContentNotes",
                column: "TrainingContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentNotes_UserId",
                schema: "public",
                table: "ContentNotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrAccTrainings_ConfirmUserId",
                schema: "public",
                table: "CurrAccTrainings",
                column: "ConfirmUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrAccTrainings_CurrAccId",
                schema: "public",
                table: "CurrAccTrainings",
                column: "CurrAccId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrAccTrainings_StatusId",
                schema: "public",
                table: "CurrAccTrainings",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrAccTrainings_TrainingId",
                schema: "public",
                table: "CurrAccTrainings",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrAccTrainingUsers_CurrAccTrainingsId",
                schema: "public",
                table: "CurrAccTrainingUsers",
                column: "CurrAccTrainingsId");

            migrationBuilder.CreateIndex(
                name: "IX_CurrAccTrainingUsers_UserId",
                schema: "public",
                table: "CurrAccTrainingUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamAnswers_QuestionId",
                schema: "public",
                table: "ExamAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamQuestions_ExamTopicId",
                schema: "public",
                table: "ExamQuestions",
                column: "ExamTopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Exams_TrainingId",
                schema: "public",
                table: "Exams",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_ExamTopics_ExamId",
                schema: "public",
                table: "ExamTopics",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_InstructorTypeId",
                schema: "public",
                table: "Instructors",
                column: "InstructorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_UserId",
                schema: "public",
                table: "Instructors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorTraining_InstructorsId",
                schema: "public",
                table: "InstructorTraining",
                column: "InstructorsId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CommentedUserId",
                schema: "public",
                table: "Reviews",
                column: "CommentedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_TrainingId",
                schema: "public",
                table: "Reviews",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeUnitTraining_TimeUnitsId",
                schema: "public",
                table: "TimeUnitTraining",
                column: "TimeUnitsId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingAmounts_TrainingId",
                schema: "public",
                table: "TrainingAmounts",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingAnnouncements_TrainingId",
                schema: "public",
                table: "TrainingAnnouncements",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingAnnouncements_UserId",
                schema: "public",
                table: "TrainingAnnouncements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingAnnouncementsComments_TrainingAnnouncementsId",
                schema: "public",
                table: "TrainingAnnouncementsComments",
                column: "TrainingAnnouncementsId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingAnnouncementsComments_UserId",
                schema: "public",
                table: "TrainingAnnouncementsComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCategories_CategoryId",
                schema: "public",
                table: "TrainingCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCategories_MasterCategoryId",
                schema: "public",
                table: "TrainingCategories",
                column: "MasterCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingContents_ContentTypeId",
                schema: "public",
                table: "TrainingContents",
                column: "ContentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingContents_SectionId",
                schema: "public",
                table: "TrainingContents",
                column: "SectionId");

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
                name: "IX_TrainingSections_TrainingId",
                schema: "public",
                table: "TrainingSections",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_AnswerId",
                schema: "public",
                table: "UserAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_QuestionId",
                schema: "public",
                table: "UserAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswers_UserExamId",
                schema: "public",
                table: "UserAnswers",
                column: "UserExamId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCertificates_CertificateTemplateId",
                schema: "public",
                table: "UserCertificates",
                column: "CertificateTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCertificates_CurrAccTrainingUsersId",
                schema: "public",
                table: "UserCertificates",
                column: "CurrAccTrainingUsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentsViewLogDetails_UserContentsViewLogId",
                schema: "public",
                table: "UserContentsViewLogDetails",
                column: "UserContentsViewLogId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentsViewLogs_CurrAccTrainingUserId",
                schema: "public",
                table: "UserContentsViewLogs",
                column: "CurrAccTrainingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentsViewLogs_TrainingContentId",
                schema: "public",
                table: "UserContentsViewLogs",
                column: "TrainingContentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserContentsViewLogs_UserContentsViewLogId",
                schema: "public",
                table: "UserContentsViewLogs",
                column: "UserContentsViewLogId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExams_CurrAccTrainingUserId",
                schema: "public",
                table: "UserExams",
                column: "CurrAccTrainingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExams_ExamId",
                schema: "public",
                table: "UserExams",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CurrAccId",
                schema: "public",
                table: "Users",
                column: "CurrAccId");

            migrationBuilder.CreateIndex(
                name: "IX_WhatYouWillLearns_TrainingId",
                schema: "public",
                table: "WhatYouWillLearns",
                column: "TrainingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItem",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CommentVotes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ContentNotes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "InstructorTraining",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Languages",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Reviews",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TimeUnitTraining",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TrainingAmounts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TrainingAnnouncementsComments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TrainingCategories",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TrainingLevels",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TrainingStatuses",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TrainingTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserAnswers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserCertificates",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserContentsViewLogDetails",
                schema: "public");

            migrationBuilder.DropTable(
                name: "WhatYouWillLearns",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Carts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ContentComments",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Instructors",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TimeUnits",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TrainingAnnouncements",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ExamAnswers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserExams",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CertificateTemplates",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserContentsViewLogs",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CartStatus",
                schema: "public");

            migrationBuilder.DropTable(
                name: "InstructorTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ExamQuestions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CurrAccTrainingUsers",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CertificateTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TrainingContents",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ExamTopics",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CurrAccTrainings",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ContentTypes",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TrainingSections",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Exams",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CurrAccTrainingStatuses",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Trainings",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CurrAccs",
                schema: "public");
        }
    }
}
