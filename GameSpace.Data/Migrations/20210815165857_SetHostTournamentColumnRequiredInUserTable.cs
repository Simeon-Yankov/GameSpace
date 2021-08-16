using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSpace.Data.Migrations
{
    public partial class SetHostTournamentColumnRequiredInUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_HostsTournaments_HostedTournamentsId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HostedTournamentsId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "HostedTournamentsId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HostedTournamentsId",
                table: "AspNetUsers",
                column: "HostedTournamentsId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_HostsTournaments_HostedTournamentsId",
                table: "AspNetUsers",
                column: "HostedTournamentsId",
                principalTable: "HostsTournaments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_HostsTournaments_HostedTournamentsId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HostedTournamentsId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "HostedTournamentsId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HostedTournamentsId",
                table: "AspNetUsers",
                column: "HostedTournamentsId",
                unique: true,
                filter: "[HostedTournamentsId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_HostsTournaments_HostedTournamentsId",
                table: "AspNetUsers",
                column: "HostedTournamentsId",
                principalTable: "HostsTournaments",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
