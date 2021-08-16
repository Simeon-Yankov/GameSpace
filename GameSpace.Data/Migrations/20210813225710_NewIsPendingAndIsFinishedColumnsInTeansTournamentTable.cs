using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSpace.Data.Migrations
{
    public partial class NewIsPendingAndIsFinishedColumnsInTeansTournamentTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "TeamsTournaments",
                newName: "IsPending");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "TeamsTournaments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "TeamsTournaments");

            migrationBuilder.RenameColumn(
                name: "IsPending",
                table: "TeamsTournaments",
                newName: "IsDeleted");
        }
    }
}
