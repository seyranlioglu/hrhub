using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class trainingCurrAcc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingProcessHistories",
                schema: "public");

            migrationBuilder.AddColumn<bool>(
                name: "IsPrivate",
                schema: "public",
                table: "Trainings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "OwnerCurrAccId",
                schema: "public",
                table: "Trainings",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TrainingProcessRequests",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrainingId = table.Column<long>(type: "bigint", nullable: false),
                    RequestType = table.Column<int>(type: "integer", nullable: false),
                    RequestStatusId = table.Column<long>(type: "bigint", nullable: false),
                    TargetStatusId = table.Column<long>(type: "bigint", nullable: true),
                    RequesterUserId = table.Column<long>(type: "bigint", nullable: false),
                    ResponderUserId = table.Column<long>(type: "bigint", nullable: true),
                    CurrAccTrainingUserId = table.Column<long>(type: "bigint", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    ResponseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingProcessRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingProcessRequests_AspNetUsers_RequesterUserId",
                        column: x => x.RequesterUserId,
                        principalSchema: "public",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingProcessRequests_CurrAccTrainingUsers_CurrAccTrainin~",
                        column: x => x.CurrAccTrainingUserId,
                        principalSchema: "public",
                        principalTable: "CurrAccTrainingUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingProcessRequests_TrainingStatuses_RequestStatusId",
                        column: x => x.RequestStatusId,
                        principalSchema: "public",
                        principalTable: "TrainingStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingProcessRequests_TrainingStatuses_TargetStatusId",
                        column: x => x.TargetStatusId,
                        principalSchema: "public",
                        principalTable: "TrainingStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingProcessRequests_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProcessRequests_CurrAccTrainingUserId",
                schema: "public",
                table: "TrainingProcessRequests",
                column: "CurrAccTrainingUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProcessRequests_RequesterUserId",
                schema: "public",
                table: "TrainingProcessRequests",
                column: "RequesterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProcessRequests_RequestStatusId",
                schema: "public",
                table: "TrainingProcessRequests",
                column: "RequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProcessRequests_TargetStatusId",
                schema: "public",
                table: "TrainingProcessRequests",
                column: "TargetStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProcessRequests_TrainingId",
                schema: "public",
                table: "TrainingProcessRequests",
                column: "TrainingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingProcessRequests",
                schema: "public");

            migrationBuilder.DropColumn(
                name: "IsPrivate",
                schema: "public",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "OwnerCurrAccId",
                schema: "public",
                table: "Trainings");

            migrationBuilder.CreateTable(
                name: "TrainingProcessHistories",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ActionUserId = table.Column<long>(type: "bigint", nullable: false),
                    NewStatusId = table.Column<long>(type: "bigint", nullable: false),
                    OldStatusId = table.Column<long>(type: "bigint", nullable: true),
                    TrainingId = table.Column<long>(type: "bigint", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeleteDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeleteUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDelete = table.Column<bool>(type: "boolean", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdateUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingProcessHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingProcessHistories_AspNetUsers_ActionUserId",
                        column: x => x.ActionUserId,
                        principalSchema: "public",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingProcessHistories_TrainingStatuses_NewStatusId",
                        column: x => x.NewStatusId,
                        principalSchema: "public",
                        principalTable: "TrainingStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingProcessHistories_TrainingStatuses_OldStatusId",
                        column: x => x.OldStatusId,
                        principalSchema: "public",
                        principalTable: "TrainingStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TrainingProcessHistories_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalSchema: "public",
                        principalTable: "Trainings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProcessHistories_ActionUserId",
                schema: "public",
                table: "TrainingProcessHistories",
                column: "ActionUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProcessHistories_NewStatusId",
                schema: "public",
                table: "TrainingProcessHistories",
                column: "NewStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProcessHistories_OldStatusId",
                schema: "public",
                table: "TrainingProcessHistories",
                column: "OldStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingProcessHistories_TrainingId",
                schema: "public",
                table: "TrainingProcessHistories",
                column: "TrainingId");
        }
    }
}
