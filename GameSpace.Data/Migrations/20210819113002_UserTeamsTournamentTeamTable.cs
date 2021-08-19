using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSpace.Data.Migrations
{
    public partial class UserTeamsTournamentTeamTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamsTournamentsTeams",
                table: "TeamsTournamentsTeams");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TeamsTournamentsTeams",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamsTournamentsTeams",
                table: "TeamsTournamentsTeams",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UsersTeamsTournamentTeams",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TeamsTournamentTeamId = table.Column<int>(type: "int", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTeamsTournamentTeams", x => new { x.TeamsTournamentTeamId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UsersTeamsTournamentTeams_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsersTeamsTournamentTeams_TeamsTournamentsTeams_TeamsTournamentTeamId",
                        column: x => x.TeamsTournamentTeamId,
                        principalTable: "TeamsTournamentsTeams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamsTournamentsTeams_TeamsTournamentId",
                table: "TeamsTournamentsTeams",
                column: "TeamsTournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTeamsTournamentTeams_UserId",
                table: "UsersTeamsTournamentTeams",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersTeamsTournamentTeams");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamsTournamentsTeams",
                table: "TeamsTournamentsTeams");

            migrationBuilder.DropIndex(
                name: "IX_TeamsTournamentsTeams_TeamsTournamentId",
                table: "TeamsTournamentsTeams");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TeamsTournamentsTeams");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamsTournamentsTeams",
                table: "TeamsTournamentsTeams",
                columns: new[] { "TeamsTournamentId", "TeamId" });
        }
    }
}
