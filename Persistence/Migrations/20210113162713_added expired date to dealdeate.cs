using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class addedexpireddatetodealdeate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExpiredDate",
                table: "DealDate",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameDeal_GameId",
                table: "GameDeal",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameDeal_Game_GameId",
                table: "GameDeal",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "GameID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameDeal_Game_GameId",
                table: "GameDeal");

            migrationBuilder.DropIndex(
                name: "IX_GameDeal_GameId",
                table: "GameDeal");

            migrationBuilder.DropColumn(
                name: "ExpiredDate",
                table: "DealDate");
        }
    }
}
