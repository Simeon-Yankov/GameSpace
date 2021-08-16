using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSpace.Data.Migrations
{
    public partial class TeamsTournamentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "GameAccounts",
                type: "nvarchar(56)",
                maxLength: 56,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "HostedTournamentsId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BracketTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BracketTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HostsTournaments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HostsTournaments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Maps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maps", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaximumTeamsFormats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Capacity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaximumTeamsFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamSizes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Format = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamSizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TeamsTournaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    BracketTypeId = table.Column<int>(type: "int", nullable: false),
                    BronzeMatch = table.Column<bool>(type: "bit", nullable: false),
                    MaximumTeamsId = table.Column<int>(type: "int", nullable: false),
                    MinimumTeams = table.Column<int>(type: "int", nullable: false),
                    TeamSizeId = table.Column<int>(type: "int", nullable: false),
                    MapId = table.Column<int>(type: "int", nullable: false),
                    ModeId = table.Column<int>(type: "int", nullable: false),
                    PrizePool = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CheckInPeriod = table.Column<int>(type: "int", nullable: false),
                    GoToGamePeriod = table.Column<int>(type: "int", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Information = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HosterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamsTournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamsTournaments_BracketTypes_BracketTypeId",
                        column: x => x.BracketTypeId,
                        principalTable: "BracketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamsTournaments_HostsTournaments_HosterId",
                        column: x => x.HosterId,
                        principalTable: "HostsTournaments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamsTournaments_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamsTournaments_MaximumTeamsFormats_MaximumTeamsId",
                        column: x => x.MaximumTeamsId,
                        principalTable: "MaximumTeamsFormats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamsTournaments_Modes_ModeId",
                        column: x => x.ModeId,
                        principalTable: "Modes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamsTournaments_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamsTournaments_TeamSizes_TeamSizeId",
                        column: x => x.TeamSizeId,
                        principalTable: "TeamSizes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlueSideId = table.Column<int>(type: "int", nullable: false),
                    RedSideId = table.Column<int>(type: "int", nullable: false),
                    WinnerId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TeamsTournamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_TeamsTournaments_TeamsTournamentId",
                        column: x => x.TeamsTournamentId,
                        principalTable: "TeamsTournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeamsTournamentsTeams",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false),
                    TeamsTournamentId = table.Column<int>(type: "int", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false),
                    IsEliminated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamsTournamentsTeams", x => new { x.TeamsTournamentId, x.TeamId });
                    table.ForeignKey(
                        name: "FK_TeamsTournamentsTeams_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TeamsTournamentsTeams_TeamsTournaments_TeamsTournamentId",
                        column: x => x.TeamsTournamentId,
                        principalTable: "TeamsTournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HostedTournamentsId",
                table: "AspNetUsers",
                column: "HostedTournamentsId",
                unique: true,
                filter: "[HostedTournamentsId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TeamsTournamentId",
                table: "Matches",
                column: "TeamsTournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamsTournaments_BracketTypeId",
                table: "TeamsTournaments",
                column: "BracketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamsTournaments_HosterId",
                table: "TeamsTournaments",
                column: "HosterId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamsTournaments_MapId",
                table: "TeamsTournaments",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamsTournaments_MaximumTeamsId",
                table: "TeamsTournaments",
                column: "MaximumTeamsId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamsTournaments_ModeId",
                table: "TeamsTournaments",
                column: "ModeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamsTournaments_RegionId",
                table: "TeamsTournaments",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamsTournaments_TeamSizeId",
                table: "TeamsTournaments",
                column: "TeamSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamsTournamentsTeams_TeamId",
                table: "TeamsTournamentsTeams",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_HostsTournaments_HostedTournamentsId",
                table: "AspNetUsers",
                column: "HostedTournamentsId",
                principalTable: "HostsTournaments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_HostsTournaments_HostedTournamentsId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "TeamsTournamentsTeams");

            migrationBuilder.DropTable(
                name: "TeamsTournaments");

            migrationBuilder.DropTable(
                name: "BracketTypes");

            migrationBuilder.DropTable(
                name: "HostsTournaments");

            migrationBuilder.DropTable(
                name: "Maps");

            migrationBuilder.DropTable(
                name: "MaximumTeamsFormats");

            migrationBuilder.DropTable(
                name: "Modes");

            migrationBuilder.DropTable(
                name: "TeamSizes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HostedTournamentsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HostedTournamentsId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "GameAccounts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(56)",
                oldMaxLength: 56);
        }
    }
}
