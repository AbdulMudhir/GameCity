using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Symbole = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "DealDate",
                columns: table => new
                {
                    DealDateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiringDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LimitedTimeDeal = table.Column<bool>(type: "bit", nullable: false),
                    Expired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealDate", x => x.DealDateId);
                });

            migrationBuilder.CreateTable(
                name: "Developer",
                columns: table => new
                {
                    DeveloperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Developer", x => x.DeveloperId);
                });

            migrationBuilder.CreateTable(
                name: "DLC",
                columns: table => new
                {
                    DLCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SteamAppID = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DLC", x => x.DLCId);
                });

            migrationBuilder.CreateTable(
                name: "GameCategory",
                columns: table => new
                {
                    GameCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameCategory", x => x.GameCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "GameDeveloper",
                columns: table => new
                {
                    GameDeveloperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeveloperId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDeveloper", x => x.GameDeveloperId);
                });

            migrationBuilder.CreateTable(
                name: "GameDLC",
                columns: table => new
                {
                    GameDLCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DLCId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDLC", x => x.GameDLCId);
                });

            migrationBuilder.CreateTable(
                name: "GameGenre",
                columns: table => new
                {
                    GameGenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGenre", x => x.GameGenreId);
                });

            migrationBuilder.CreateTable(
                name: "GamePublisher",
                columns: table => new
                {
                    GamePublisherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PublisherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePublisher", x => x.GamePublisherId);
                });

            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    GenreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.GenreId);
                });

            migrationBuilder.CreateTable(
                name: "Platform",
                columns: table => new
                {
                    PlatformId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platform", x => x.PlatformId);
                });

            migrationBuilder.CreateTable(
                name: "Publisher",
                columns: table => new
                {
                    PublisherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publisher", x => x.PublisherId);
                });

            migrationBuilder.CreateTable(
                name: "ReleaseDate",
                columns: table => new
                {
                    ReleaseDateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComingSoon = table.Column<bool>(type: "bit", nullable: false),
                    ReleasedDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseDate", x => x.ReleaseDateId);
                });

            migrationBuilder.CreateTable(
                name: "Screenshot",
                columns: table => new
                {
                    ScreenshotId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PathFull = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PathThumbnail = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Screenshot", x => x.ScreenshotId);
                });

            migrationBuilder.CreateTable(
                name: "SteamApp",
                columns: table => new
                {
                    SteamAppId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SteamReview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SteamReviewCount = table.Column<int>(type: "int", nullable: false),
                    SteamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SteamApp", x => x.SteamAppId);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.StoreId);
                });

            migrationBuilder.CreateTable(
                name: "Video",
                columns: table => new
                {
                    VideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Highlight = table.Column<bool>(type: "bit", nullable: false),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Video", x => x.VideoId);
                });

            migrationBuilder.CreateTable(
                name: "PriceOverview",
                columns: table => new
                {
                    PriceOverviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FinalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalPriceFormat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceOverview", x => x.PriceOverviewId);
                    table.ForeignKey(
                        name: "FK_PriceOverview_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "CurrencyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SystemRequirement",
                columns: table => new
                {
                    SystemRequirementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlatformId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Minimum = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recommended = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemRequirement", x => x.SystemRequirementId);
                    table.ForeignKey(
                        name: "FK_SystemRequirement_Platform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platform",
                        principalColumn: "PlatformId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    GameID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", maxLength: 20000, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Thumbnail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReleaseDateID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HeaderImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Background = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SteamAppId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.GameID);
                    table.ForeignKey(
                        name: "FK_Game_ReleaseDate_ReleaseDateID",
                        column: x => x.ReleaseDateID,
                        principalTable: "ReleaseDate",
                        principalColumn: "ReleaseDateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Game_SteamApp_SteamAppId",
                        column: x => x.SteamAppId,
                        principalTable: "SteamApp",
                        principalColumn: "SteamAppId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VideoContent",
                columns: table => new
                {
                    VideoContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VideoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Max = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MediaType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoContent", x => x.VideoContentId);
                    table.ForeignKey(
                        name: "FK_VideoContent_Video_VideoId",
                        column: x => x.VideoId,
                        principalTable: "Video",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameDeal",
                columns: table => new
                {
                    GameDealId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoreId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PriceOverviewId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DealDateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsFree = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameDeal", x => x.GameDealId);
                    table.ForeignKey(
                        name: "FK_GameDeal_DealDate_DealDateId",
                        column: x => x.DealDateId,
                        principalTable: "DealDate",
                        principalColumn: "DealDateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameDeal_PriceOverview_PriceOverviewId",
                        column: x => x.PriceOverviewId,
                        principalTable: "PriceOverview",
                        principalColumn: "PriceOverviewId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameDeal_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Category_Description",
                table: "Category",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currency_Code",
                table: "Currency",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Developer_Name",
                table: "Developer",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Game_ReleaseDateID",
                table: "Game",
                column: "ReleaseDateID");

            migrationBuilder.CreateIndex(
                name: "IX_Game_SteamAppId",
                table: "Game",
                column: "SteamAppId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameDeal_DealDateId",
                table: "GameDeal",
                column: "DealDateId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDeal_PriceOverviewId",
                table: "GameDeal",
                column: "PriceOverviewId");

            migrationBuilder.CreateIndex(
                name: "IX_GameDeal_StoreId",
                table: "GameDeal",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Genre_Description",
                table: "Genre",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Platform_Name",
                table: "Platform",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceOverview_CurrencyId",
                table: "PriceOverview",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Publisher_Name",
                table: "Publisher",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SteamApp_SteamId",
                table: "SteamApp",
                column: "SteamId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SystemRequirement_PlatformId",
                table: "SystemRequirement",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_VideoContent_VideoId",
                table: "VideoContent",
                column: "VideoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Developer");

            migrationBuilder.DropTable(
                name: "DLC");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "GameCategory");

            migrationBuilder.DropTable(
                name: "GameDeal");

            migrationBuilder.DropTable(
                name: "GameDeveloper");

            migrationBuilder.DropTable(
                name: "GameDLC");

            migrationBuilder.DropTable(
                name: "GameGenre");

            migrationBuilder.DropTable(
                name: "GamePublisher");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Publisher");

            migrationBuilder.DropTable(
                name: "Screenshot");

            migrationBuilder.DropTable(
                name: "SystemRequirement");

            migrationBuilder.DropTable(
                name: "VideoContent");

            migrationBuilder.DropTable(
                name: "ReleaseDate");

            migrationBuilder.DropTable(
                name: "SteamApp");

            migrationBuilder.DropTable(
                name: "DealDate");

            migrationBuilder.DropTable(
                name: "PriceOverview");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Platform");

            migrationBuilder.DropTable(
                name: "Video");

            migrationBuilder.DropTable(
                name: "Currency");
        }
    }
}
