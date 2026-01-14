using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class menu4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreateUserId",
                schema: "public",
                table: "SysMenuRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "public",
                table: "SysMenuRoles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                schema: "public",
                table: "SysMenuRoles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleteUserId",
                schema: "public",
                table: "SysMenuRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "SysMenuRoles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "public",
                table: "SysMenuRoles",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "public",
                table: "SysMenuRoles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdateUserId",
                schema: "public",
                table: "SysMenuRoles",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CreateUserId",
                schema: "public",
                table: "SysMenuPolicies",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "public",
                table: "SysMenuPolicies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                schema: "public",
                table: "SysMenuPolicies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleteUserId",
                schema: "public",
                table: "SysMenuPolicies",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "SysMenuPolicies",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "public",
                table: "SysMenuPolicies",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "public",
                table: "SysMenuPolicies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdateUserId",
                schema: "public",
                table: "SysMenuPolicies",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateUserId",
                schema: "public",
                table: "SysMenuRoles");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "public",
                table: "SysMenuRoles");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                schema: "public",
                table: "SysMenuRoles");

            migrationBuilder.DropColumn(
                name: "DeleteUserId",
                schema: "public",
                table: "SysMenuRoles");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "SysMenuRoles");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "public",
                table: "SysMenuRoles");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "public",
                table: "SysMenuRoles");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                schema: "public",
                table: "SysMenuRoles");

            migrationBuilder.DropColumn(
                name: "CreateUserId",
                schema: "public",
                table: "SysMenuPolicies");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "public",
                table: "SysMenuPolicies");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                schema: "public",
                table: "SysMenuPolicies");

            migrationBuilder.DropColumn(
                name: "DeleteUserId",
                schema: "public",
                table: "SysMenuPolicies");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "SysMenuPolicies");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "public",
                table: "SysMenuPolicies");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "public",
                table: "SysMenuPolicies");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                schema: "public",
                table: "SysMenuPolicies");
        }
    }
}
