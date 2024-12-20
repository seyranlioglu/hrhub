using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class userlast2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CreateUserId",
                schema: "public",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "public",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteDate",
                schema: "public",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DeleteUserId",
                schema: "public",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                schema: "public",
                table: "AspNetUsers",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                schema: "public",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdateUserId",
                schema: "public",
                table: "AspNetUsers",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateUserId",
                schema: "public",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "public",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeleteDate",
                schema: "public",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeleteUserId",
                schema: "public",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                schema: "public",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                schema: "public",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                schema: "public",
                table: "AspNetUsers");
        }
    }
}
