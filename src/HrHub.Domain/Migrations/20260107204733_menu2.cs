using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class menu2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SysMenuPolicies",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SysMenuId = table.Column<long>(type: "bigint", nullable: false),
                    PolicyName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysMenuPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SysMenuPolicies_SysMenus_SysMenuId",
                        column: x => x.SysMenuId,
                        principalSchema: "public",
                        principalTable: "SysMenus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SysMenuPolicies_SysMenuId",
                schema: "public",
                table: "SysMenuPolicies",
                column: "SysMenuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SysMenuPolicies",
                schema: "public");
        }
    }
}
