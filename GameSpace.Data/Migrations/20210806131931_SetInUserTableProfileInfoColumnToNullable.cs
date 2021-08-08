using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GameSpace.Data.Migrations
{
    public partial class SetInUserTableProfileInfoColumnToNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProfilesInfo_ProfileInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfileInfoId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "ProfilesInfo",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "ProfilesInfo",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProfileInfoId",
                table: "AspNetUsers",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileInfoId",
                table: "AspNetUsers",
                column: "ProfileInfoId",
                unique: true,
                filter: "[ProfileInfoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProfilesInfo_ProfileInfoId",
                table: "AspNetUsers",
                column: "ProfileInfoId",
                principalTable: "ProfilesInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ProfilesInfo_ProfileInfoId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ProfileInfoId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "ProfilesInfo",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOn",
                table: "ProfilesInfo",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "ProfileInfoId",
                table: "AspNetUsers",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileInfoId",
                table: "AspNetUsers",
                column: "ProfileInfoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ProfilesInfo_ProfileInfoId",
                table: "AspNetUsers",
                column: "ProfileInfoId",
                principalTable: "ProfilesInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
