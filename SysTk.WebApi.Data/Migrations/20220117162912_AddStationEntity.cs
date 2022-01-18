using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SysTk.WebApi.Data.Migrations
{
    public partial class AddStationEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cluster",
                table: "FtpCredentials");

            migrationBuilder.DropColumn(
                name: "IP",
                table: "FtpCredentials");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FtpCredentials");

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    Cluster = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IP = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FtpCredentials_StationId",
                table: "FtpCredentials",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_FtpCredentials_Stations_StationId",
                table: "FtpCredentials",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FtpCredentials_Stations_StationId",
                table: "FtpCredentials");

            migrationBuilder.DropTable(
                name: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_FtpCredentials_StationId",
                table: "FtpCredentials");

            migrationBuilder.AddColumn<string>(
                name: "Cluster",
                table: "FtpCredentials",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "FtpCredentials",
                type: "varchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FtpCredentials",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }
    }
}
