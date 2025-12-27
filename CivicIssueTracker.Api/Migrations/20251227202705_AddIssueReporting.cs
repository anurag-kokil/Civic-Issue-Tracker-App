using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CivicIssueTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddIssueReporting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Issues_ReportedByUserId",
                table: "Issues",
                column: "ReportedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Issues_Users_ReportedByUserId",
                table: "Issues",
                column: "ReportedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Issues_Users_ReportedByUserId",
                table: "Issues");

            migrationBuilder.DropIndex(
                name: "IX_Issues_ReportedByUserId",
                table: "Issues");
        }
    }
}
