using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class updatedsteamapptocountforoldids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SteamIdLinkedTo",
                table: "SteamApp",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ValidSteamId",
                table: "SteamApp",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SteamIdLinkedTo",
                table: "SteamApp");

            migrationBuilder.DropColumn(
                name: "ValidSteamId",
                table: "SteamApp");
        }
    }
}
