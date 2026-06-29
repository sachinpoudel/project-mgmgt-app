using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mvc_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class jjj : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectMembers_ProjectMemberId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_ProjectMemberId",
                table: "ProjectTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMembers");

            migrationBuilder.RenameColumn(
                name: "ProjectMemberId",
                table: "ProjectTasks",
                newName: "ProjectMemberUserId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectMemberProjectId",
                table: "ProjectTasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers",
                columns: new[] { "UserId", "ProjectId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectMemberUserId_ProjectMemberProjectId",
                table: "ProjectTasks",
                columns: new[] { "ProjectMemberUserId", "ProjectMemberProjectId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectMembers_ProjectMemberUserId_ProjectMemb~",
                table: "ProjectTasks",
                columns: new[] { "ProjectMemberUserId", "ProjectMemberProjectId" },
                principalTable: "ProjectMembers",
                principalColumns: new[] { "UserId", "ProjectId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectMembers_ProjectMemberUserId_ProjectMemb~",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_ProjectMemberUserId_ProjectMemberProjectId",
                table: "ProjectTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers");

            migrationBuilder.DropColumn(
                name: "ProjectMemberProjectId",
                table: "ProjectTasks");

            migrationBuilder.RenameColumn(
                name: "ProjectMemberUserId",
                table: "ProjectTasks",
                newName: "ProjectMemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectMembers",
                table: "ProjectMembers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectMemberId",
                table: "ProjectTasks",
                column: "ProjectMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_UserId",
                table: "ProjectMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectMembers_ProjectMemberId",
                table: "ProjectTasks",
                column: "ProjectMemberId",
                principalTable: "ProjectMembers",
                principalColumn: "Id");
        }
    }
}
