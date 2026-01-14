using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class menu5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysMenuRoles_AppRole_RoleId",
                schema: "public",
                table: "SysMenuRoles");

            migrationBuilder.AddForeignKey(
                name: "FK_SysMenuRoles_AspNetRoles_RoleId",
                schema: "public",
                table: "SysMenuRoles",
                column: "RoleId",
                principalSchema: "public",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SysMenuRoles_AspNetRoles_RoleId",
                schema: "public",
                table: "SysMenuRoles");


            migrationBuilder.AddForeignKey(
                name: "FK_SysMenuRoles_AppRole_RoleId",
                schema: "public",
                table: "SysMenuRoles",
                column: "RoleId",
                principalSchema: "public",
                principalTable: "AppRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
