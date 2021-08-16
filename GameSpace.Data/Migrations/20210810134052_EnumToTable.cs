using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSpace.Data.Migrations
{
    public partial class EnumToTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Languages_ProfilesInfo_ProfileInfoId",
                table: "Languages");

            migrationBuilder.DropIndex(
                name: "IX_Languages_ProfileInfoId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "ProfileInfoId",
                table: "Languages");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "GameAccounts");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "GameAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Languages",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RankId",
                table: "GameAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "GameAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProfileInfosLanguages",
                columns: table => new
                {
                    ProfileInfoId = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileInfosLanguages", x => new { x.ProfileInfoId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_ProfileInfosLanguages_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileInfosLanguages_ProfilesInfo_ProfileInfoId",
                        column: x => x.ProfileInfoId,
                        principalTable: "ProfilesInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ranks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ranks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameAccounts_RankId",
                table: "GameAccounts",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_GameAccounts_RegionId",
                table: "GameAccounts",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileInfosLanguages_LanguageId",
                table: "ProfileInfosLanguages",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameAccounts_Ranks_RankId",
                table: "GameAccounts",
                column: "RankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameAccounts_Regions_RegionId",
                table: "GameAccounts",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameAccounts_Ranks_RankId",
                table: "GameAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_GameAccounts_Regions_RegionId",
                table: "GameAccounts");

            migrationBuilder.DropTable(
                name: "ProfileInfosLanguages");

            migrationBuilder.DropTable(
                name: "Ranks");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_GameAccounts_RankId",
                table: "GameAccounts");

            migrationBuilder.DropIndex(
                name: "IX_GameAccounts_RegionId",
                table: "GameAccounts");

            migrationBuilder.DropColumn(
                name: "RankId",
                table: "GameAccounts");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "GameAccounts");

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "Languages",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(15)",
                oldMaxLength: 15);

            migrationBuilder.AddColumn<string>(
                name: "ProfileInfoId",
                table: "Languages",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "GameAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Region",
                table: "GameAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Languages_ProfileInfoId",
                table: "Languages",
                column: "ProfileInfoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Languages_ProfilesInfo_ProfileInfoId",
                table: "Languages",
                column: "ProfileInfoId",
                principalTable: "ProfilesInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
