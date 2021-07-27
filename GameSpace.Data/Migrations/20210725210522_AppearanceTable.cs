using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSpace.Data.Migrations
{
    public partial class AppearanceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersTeams_AspNetUsers_UserId",
                table: "UsersTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersTeams_Teams_TeamId",
                table: "UsersTeams");

            migrationBuilder.AddColumn<int>(
                name: "AppearanceId",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Appearances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Banner = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appearances", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_AppearanceId",
                table: "Teams",
                column: "AppearanceId",
                unique: true,
                filter: "[AppearanceId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Appearances_AppearanceId",
                table: "Teams",
                column: "AppearanceId",
                principalTable: "Appearances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTeams_AspNetUsers_UserId",
                table: "UsersTeams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTeams_Teams_TeamId",
                table: "UsersTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Appearances_AppearanceId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersTeams_AspNetUsers_UserId",
                table: "UsersTeams");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersTeams_Teams_TeamId",
                table: "UsersTeams");

            migrationBuilder.DropTable(
                name: "Appearances");

            migrationBuilder.DropIndex(
                name: "IX_Teams_AppearanceId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "AppearanceId",
                table: "Teams");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTeams_AspNetUsers_UserId",
                table: "UsersTeams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTeams_Teams_TeamId",
                table: "UsersTeams",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
