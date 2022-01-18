using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SysTk.WebApi.Data.Migrations
{
    public partial class RenameStationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PetrolServerId",
                table: "FtpCredentials",
                newName: "StationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StationId",
                table: "FtpCredentials",
                newName: "PetrolServerId");
        }
    }
}
