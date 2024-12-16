using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HrHub.Domain.Migrations
{
    /// <inheritdoc />
    public partial class userlast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           





            migrationBuilder.CreateTable(
                name: "PasswordHistories",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    ChangeReason = table.Column<string>(type: "text", nullable: false),
                    IsSuccess = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordChangeDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PassForgotting = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_PasswordHistories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CurrAccs_CurrAccId",
                schema: "public",
                table: "AspNetUsers",
                column: "CurrAccId",
                principalSchema: "public",
                principalTable: "CurrAccs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_AspNetUsers_AddCartUserId",
                schema: "public",
                table: "Carts",
                column: "AddCartUserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_AspNetUsers_ConfirmUserId",
                schema: "public",
                table: "Carts",
                column: "ConfirmUserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVotes_AspNetUsers_UserId",
                schema: "public",
                table: "CommentVotes",
                column: "UserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentComments_AspNetUsers_UserId",
                schema: "public",
                table: "ContentComments",
                column: "UserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentNotes_AspNetUsers_UserId",
                schema: "public",
                table: "ContentNotes",
                column: "UserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrAccTrainings_AspNetUsers_ConfirmUserId",
                schema: "public",
                table: "CurrAccTrainings",
                column: "ConfirmUserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrAccTrainingUsers_AspNetUsers_UserId",
                schema: "public",
                table: "CurrAccTrainingUsers",
                column: "UserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_AspNetUsers_UserId",
                schema: "public",
                table: "ExamResults",
                column: "UserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_AspNetUsers_InstructorId",
                schema: "public",
                table: "Exams",
                column: "InstructorId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_AspNetUsers_UserId",
                schema: "public",
                table: "Instructors",
                column: "UserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_CommentedUserId",
                schema: "public",
                table: "Reviews",
                column: "CommentedUserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingAnnouncements_AspNetUsers_UserId",
                schema: "public",
                table: "TrainingAnnouncements",
                column: "UserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingAnnouncementsComments_AspNetUsers_UserId",
                schema: "public",
                table: "TrainingAnnouncementsComments",
                column: "UserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CurrAccs_CurrAccId",
                schema: "public",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_AspNetUsers_AddCartUserId",
                schema: "public",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_Carts_AspNetUsers_ConfirmUserId",
                schema: "public",
                table: "Carts");

            migrationBuilder.DropForeignKey(
                name: "FK_CommentVotes_AspNetUsers_UserId",
                schema: "public",
                table: "CommentVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentComments_AspNetUsers_UserId",
                schema: "public",
                table: "ContentComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentNotes_AspNetUsers_UserId",
                schema: "public",
                table: "ContentNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrAccTrainings_AspNetUsers_ConfirmUserId",
                schema: "public",
                table: "CurrAccTrainings");

            migrationBuilder.DropForeignKey(
                name: "FK_CurrAccTrainingUsers_AspNetUsers_UserId",
                schema: "public",
                table: "CurrAccTrainingUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ExamResults_AspNetUsers_UserId",
                schema: "public",
                table: "ExamResults");

            migrationBuilder.DropForeignKey(
                name: "FK_Exams_AspNetUsers_InstructorId",
                schema: "public",
                table: "Exams");

            migrationBuilder.DropForeignKey(
                name: "FK_Instructors_AspNetUsers_UserId",
                schema: "public",
                table: "Instructors");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_CommentedUserId",
                schema: "public",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingAnnouncements_AspNetUsers_UserId",
                schema: "public",
                table: "TrainingAnnouncements");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingAnnouncementsComments_AspNetUsers_UserId",
                schema: "public",
                table: "TrainingAnnouncementsComments");

            migrationBuilder.DropTable(
                name: "PasswordHistories",
                schema: "public");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                schema: "public",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "public",
                newName: "Users",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_CurrAccId",
                schema: "public",
                table: "Users",
                newName: "IX_Users_CurrAccId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "public",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_AddCartUserId",
                schema: "public",
                table: "Carts",
                column: "AddCartUserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Users_ConfirmUserId",
                schema: "public",
                table: "Carts",
                column: "ConfirmUserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentVotes_Users_UserId",
                schema: "public",
                table: "CommentVotes",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentComments_Users_UserId",
                schema: "public",
                table: "ContentComments",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentNotes_Users_UserId",
                schema: "public",
                table: "ContentNotes",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrAccTrainings_Users_ConfirmUserId",
                schema: "public",
                table: "CurrAccTrainings",
                column: "ConfirmUserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrAccTrainingUsers_Users_UserId",
                schema: "public",
                table: "CurrAccTrainingUsers",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExamResults_Users_UserId",
                schema: "public",
                table: "ExamResults",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exams_Users_InstructorId",
                schema: "public",
                table: "Exams",
                column: "InstructorId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Instructors_Users_UserId",
                schema: "public",
                table: "Instructors",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_CommentedUserId",
                schema: "public",
                table: "Reviews",
                column: "CommentedUserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingAnnouncements_Users_UserId",
                schema: "public",
                table: "TrainingAnnouncements",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingAnnouncementsComments_Users_UserId",
                schema: "public",
                table: "TrainingAnnouncementsComments",
                column: "UserId",
                principalSchema: "public",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CurrAccs_CurrAccId",
                schema: "public",
                table: "Users",
                column: "CurrAccId",
                principalSchema: "public",
                principalTable: "CurrAccs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
