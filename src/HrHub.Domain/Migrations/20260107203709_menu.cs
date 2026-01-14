using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class menu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysMenus",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Path = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: true),
                    OrderNo = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_SysMenus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysMenus_SysMenus_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "public",
                        principalTable: "SysMenus",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrainingProcessHistories",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrainingId = table.Column<long>(type: "bigint", nullable: false),
                    OldStatusId = table.Column<long>(type: "bigint", nullable: true),
                    NewStatusId = table.Column<long>(type: "bigint", nullable: false),
                    ActionUserId = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "SysMenuRoles",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SysMenuId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenuRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysMenuRoles_AppRole_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "public",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SysMenuRoles_SysMenus_SysMenuId",
                        column: x => x.SysMenuId,
                        principalSchema: "public",
                        principalTable: "SysMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SysMenuRoles_RoleId",
                schema: "public",
                table: "SysMenuRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SysMenuRoles_SysMenuId",
                schema: "public",
                table: "SysMenuRoles",
                column: "SysMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_SysMenus_ParentId",
                schema: "public",
                table: "SysMenus",
                column: "ParentId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysMenuRoles",
                schema: "public");

            migrationBuilder.DropTable(
                name: "TrainingProcessHistories",
                schema: "public");

            migrationBuilder.DropTable(
                name: "SysMenus",
                schema: "public");
        }
    }
}
