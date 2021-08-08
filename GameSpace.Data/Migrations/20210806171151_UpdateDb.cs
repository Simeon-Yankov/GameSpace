using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSpace.Data.Migrations
{
    public partial class UpdateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "SocialNetworkId",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AppearanceId",
                table: "ProfilesInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocialNetworkId",
                table: "ProfilesInfo",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teams_AppearanceId",
                table: "Teams",
                column: "AppearanceId",
                unique: true,
                filter: "[AppearanceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_SocialNetworkId",
                table: "Teams",
                column: "SocialNetworkId",
                unique: true,
                filter: "[SocialNetworkId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilesInfo_AppearanceId",
                table: "ProfilesInfo",
                column: "AppearanceId",
                unique: true,
                filter: "[AppearanceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProfilesInfo_SocialNetworkId",
                table: "ProfilesInfo",
                column: "SocialNetworkId",
                unique: true,
                filter: "[SocialNetworkId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilesInfo_Appearances_AppearanceId",
                table: "ProfilesInfo",
                column: "AppearanceId",
                principalTable: "Appearances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfilesInfo_SocialNetworks_SocialNetworkId",
                table: "ProfilesInfo",
                column: "SocialNetworkId",
                principalTable: "SocialNetworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_Appearances_AppearanceId",
                table: "Teams",
                column: "AppearanceId",
                principalTable: "Appearances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Teams_SocialNetworks_SocialNetworkId",
                table: "Teams",
                column: "SocialNetworkId",
                principalTable: "SocialNetworks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfilesInfo_Appearances_AppearanceId",
                table: "ProfilesInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfilesInfo_SocialNetworks_SocialNetworkId",
                table: "ProfilesInfo");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_Appearances_AppearanceId",
                table: "Teams");

            migrationBuilder.DropForeignKey(
                name: "FK_Teams_SocialNetworks_SocialNetworkId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_AppearanceId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_Teams_SocialNetworkId",
                table: "Teams");

            migrationBuilder.DropIndex(
                name: "IX_ProfilesInfo_AppearanceId",
                table: "ProfilesInfo");

            migrationBuilder.DropIndex(
                name: "IX_ProfilesInfo_SocialNetworkId",
                table: "ProfilesInfo");

            migrationBuilder.DropColumn(
                name: "AppearanceId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "SocialNetworkId",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "AppearanceId",
                table: "ProfilesInfo");

            migrationBuilder.DropColumn(
                name: "SocialNetworkId",
                table: "ProfilesInfo");

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
        }
    }
}
