using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSpace.Data.Migrations
{
    public partial class LittleUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReciverId",
                table: "PendingTeamsRequests",
                newName: "ReceiverId");

            migrationBuilder.RenameColumn(
                name: "ReciverId",
                table: "Notifications",
                newName: "ReceiverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "PendingTeamsRequests",
                newName: "ReciverId");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Notifications",
                newName: "ReciverId");
        }
    }
}
