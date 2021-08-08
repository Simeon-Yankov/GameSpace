using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSpace.Data.Migrations
{
    public partial class AddColumInPendingTeamUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TeamName",
                table: "PendingTeamsRequests",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamName",
                table: "PendingTeamsRequests");
        }
    }
}
