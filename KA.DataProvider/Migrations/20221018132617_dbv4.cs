using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KA.DataProvider.Migrations
{
    public partial class dbv4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "subscriptions",
                newName: "RegisterDate");

            migrationBuilder.AddColumn<string>(
                name: "CourseName",
                table: "subscriptions",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredDate",
                table: "subscriptions",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "DurationTime",
                table: "courses",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Place",
                table: "courses",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "courses",
                type: "datetime(6)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseName",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                table: "subscriptions");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "DurationTime",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "Place",
                table: "courses");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "courses");

            migrationBuilder.RenameColumn(
                name: "RegisterDate",
                table: "subscriptions",
                newName: "CreatedDate");
        }
    }
}
