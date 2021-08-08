using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSpace.Data.Migrations
{
    public partial class UserTableUpdateAndProfileInfoTableCreateAndLanguageTableCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Appearances_AppearanceId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_SocialNetworks_SocialNetworksId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersTeams_AspNetUsers_UserId",
                table: "UsersTeams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_AppearanceId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_SocialNetworksId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "AppearanceId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "SocialNetworksId",
                table: "Teams");

            migrationBuilder.AddColumn<string>(
                name: "ProfileInfoId",
                table: "SocialNetworks",
                type: "nvarchar(40)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileInfoId1",
                table: "SocialNetworks",
                type: "nvarchar(40)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "SocialNetworks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamId1",
                table: "SocialNetworks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileInfoId",
                table: "AspNetUsers",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfileInfoId",
                table: "Appearances",
                type: "nvarchar(40)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileInfoId1",
                table: "Appearances",
                type: "nvarchar(40)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Appearances",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TeamId1",
                table: "Appearances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProfilesInfo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfilesInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<int>(type: "int", nullable: false),
                    ProfileInfoId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Languages_ProfilesInfo_ProfileInfoId",
                        column: x => x.ProfileInfoId,
                        principalTable: "ProfilesInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SocialNetworks_ProfileInfoId",
                table: "SocialNetworks",
                column: "ProfileInfoId",
                unique: true,
                filter: "[ProfileInfoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SocialNetworks_ProfileInfoId1",
                table: "SocialNetworks",
                column: "ProfileInfoId1",
                unique: true,
                filter: "[ProfileInfoId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SocialNetworks_TeamId",
                table: "SocialNetworks",
                column: "TeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialNetworks_TeamId1",
                table: "SocialNetworks",
                column: "TeamId1",
                unique: true,
                filter: "[TeamId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileInfoId",
                table: "AspNetUsers",
                column: "ProfileInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appearances_ProfileInfoId",
                table: "Appearances",
                column: "ProfileInfoId",
                unique: true,
                filter: "[ProfileInfoId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Appearances_ProfileInfoId1",
                table: "Appearances",
                column: "ProfileInfoId1",
                unique: true,
                filter: "[ProfileInfoId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Appearances_TeamId",
                table: "Appearances",
                column: "TeamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appearances_TeamId1",
                table: "Appearances",
                column: "TeamId1",
                unique: true,
                filter: "[TeamId1] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_ProfileInfoId",
                table: "Languages",
                column: "ProfileInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appearances_ProfilesInfo_ProfileInfoId",
                table: "Appearances",
                column: "ProfileInfoId",
                principalTable: "ProfilesInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appearances_ProfilesInfo_ProfileInfoId1",
                table: "Appearances",
                column: "ProfileInfoId1",
                principalTable: "ProfilesInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appearances_Teams_TeamId",
                table: "Appearances",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appearances_Teams_TeamId1",
                table: "Appearances",
                column: "TeamId1",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProfilesInfo_ProfileInfoId",
                table: "AspNetUsers",
                column: "ProfileInfoId",
                principalTable: "ProfilesInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialNetworks_ProfilesInfo_ProfileInfoId",
                table: "SocialNetworks",
                column: "ProfileInfoId",
                principalTable: "ProfilesInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialNetworks_ProfilesInfo_ProfileInfoId1",
                table: "SocialNetworks",
                column: "ProfileInfoId1",
                principalTable: "ProfilesInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialNetworks_Teams_TeamId",
                table: "SocialNetworks",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialNetworks_Teams_TeamId1",
                table: "SocialNetworks",
                column: "TeamId1",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTeams_AspNetUsers_UserId",
                table: "UsersTeams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appearances_ProfilesInfo_ProfileInfoId",
                table: "Appearances");

            migrationBuilder.DropForeignKey(
                name: "FK_Appearances_ProfilesInfo_ProfileInfoId1",
                table: "Appearances");

            migrationBuilder.DropForeignKey(
                name: "FK_Appearances_Teams_TeamId",
                table: "Appearances");

            migrationBuilder.DropForeignKey(
                name: "FK_Appearances_Teams_TeamId1",
                table: "Appearances");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProfilesInfo_ProfileInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialNetworks_ProfilesInfo_ProfileInfoId",
                table: "SocialNetworks");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialNetworks_ProfilesInfo_ProfileInfoId1",
                table: "SocialNetworks");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialNetworks_Teams_TeamId",
                table: "SocialNetworks");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialNetworks_Teams_TeamId1",
                table: "SocialNetworks");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersTeams_AspNetUsers_UserId",
                table: "UsersTeams");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "ProfilesInfo");

            migrationBuilder.DropIndex(
                name: "IX_SocialNetworks_ProfileInfoId",
                table: "SocialNetworks");

            migrationBuilder.DropIndex(
                name: "IX_SocialNetworks_ProfileInfoId1",
                table: "SocialNetworks");

            migrationBuilder.DropIndex(
                name: "IX_SocialNetworks_TeamId",
                table: "SocialNetworks");

            migrationBuilder.DropIndex(
                name: "IX_SocialNetworks_TeamId1",
                table: "SocialNetworks");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfileInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Appearances_ProfileInfoId",
                table: "Appearances");

            migrationBuilder.DropIndex(
                name: "IX_Appearances_ProfileInfoId1",
                table: "Appearances");

            migrationBuilder.DropIndex(
                name: "IX_Appearances_TeamId",
                table: "Appearances");

            migrationBuilder.DropIndex(
                name: "IX_Appearances_TeamId1",
                table: "Appearances");

            migrationBuilder.DropColumn(
                name: "ProfileInfoId",
                table: "SocialNetworks");

            migrationBuilder.DropColumn(
                name: "ProfileInfoId1",
                table: "SocialNetworks");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "SocialNetworks");

            migrationBuilder.DropColumn(
                name: "TeamId1",
                table: "SocialNetworks");

            migrationBuilder.DropColumn(
                name: "ProfileInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileInfoId",
                table: "Appearances");

            migrationBuilder.DropColumn(
                name: "ProfileInfoId1",
                table: "Appearances");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "Appearances");

            migrationBuilder.DropColumn(
                name: "TeamId1",
                table: "Appearances");

            migrationBuilder.AddColumn<int>(
                name: "AppearanceId",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocialNetworksId",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_AppearanceId",
                table: "Teams",
                column: "AppearanceId",
                unique: true,
                filter: "[AppearanceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SocialNetworksId",
                table: "Teams",
                column: "SocialNetworksId",
                unique: true,
                filter: "[SocialNetworksId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Appearances_AppearanceId",
                table: "Teams",
                column: "AppearanceId",
                principalTable: "Appearances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_SocialNetworks_SocialNetworksId",
                table: "Teams",
                column: "SocialNetworksId",
                principalTable: "SocialNetworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTeams_AspNetUsers_UserId",
                table: "UsersTeams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
