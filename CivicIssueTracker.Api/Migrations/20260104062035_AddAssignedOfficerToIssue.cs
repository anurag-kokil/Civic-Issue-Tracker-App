using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicIssueTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedOfficerToIssue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssignedOfficerId",
                table: "Issues",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Issues_AssignedOfficerId",
                table: "Issues",
                column: "AssignedOfficerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Users_AssignedOfficerId",
                table: "Issues",
                column: "AssignedOfficerId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Users_AssignedOfficerId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_AssignedOfficerId",
                table: "Issues");

            migrationBuilder.DropColumn(
                name: "AssignedOfficerId",
                table: "Issues");
        }
    }
}
