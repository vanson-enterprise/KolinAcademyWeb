using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KA.DataProvider.Migrations
{
    public partial class dbv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreateUserId",
                table: "appusers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "appusers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "appusers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "appusers",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdateUserId",
                table: "appusers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateUserId",
                table: "appusers");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "appusers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "appusers");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "appusers");

            migrationBuilder.DropColumn(
                name: "UpdateUserId",
                table: "appusers");
        }
    }
}
