using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSpace.Data.Migrations
{
    public partial class AddColumCreateOninPendingTeamRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "PendingTeamsRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "PendingTeamsRequests");
        }
    }
}
