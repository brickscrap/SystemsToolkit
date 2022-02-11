using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SysTk.WebApi.Data.Migrations
{
    public partial class CreateBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Stations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Stations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "FtpCredentials",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "FtpCredentials",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "FtpCredentials");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "FtpCredentials",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
