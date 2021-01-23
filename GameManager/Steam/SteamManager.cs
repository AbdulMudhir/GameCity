using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.GameCity;
using Domain.DatabaseModel;
using GameManager.Utils;
using SteamAPI.Interfaces;
using SteamAPI.Models;
using SteamAPI.Utilities;

namespace GameManager.Steam
{
    public class SteamManager
    {
        private readonly ISteamAPI _steamAPI;
        private readonly GameCityManager _gamecity;


        private CancellationTokenSource _tokenSource;


        public SteamManager() : this(SteamFactory.GetSteamAPI(), new GameCityManager())
        {
        }
        public SteamManager(ISteamAPI SteamApi, GameCityManager gameCity)
        {
            _steamAPI = SteamApi;
            _gamecity = gameCity;
            _tokenSource = new CancellationTokenSource();


        }

        // filter out all steam games already in the database
        private async Task<List<SteamAPI.Models.SteamApp>> GetListOfGamesNotInDBAsync()
        {
            var set = new HashSet<int>(await _gamecity.GetAllSteamIdAsync());

            var appsFromSteam = await _steamAPI.GetAppsAsync();

            appsFromSteam.RemoveAll(app => set.Contains(app.appid));


            return appsFromSteam;
        }

        public void Stop()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }


        public async Task<bool> PriceChange(GameDeal gamedeal, SteamAPI.Models.PriceOverview priceOverviewFromApi)

        {
            var steamid = gamedeal.Game.SteamApp.SteamId;

            if (gamedeal.PriceOverview != null && priceOverviewFromApi != null)
            {
                var finalPriceFromDB = (int)gamedeal.PriceOverview.FinalPrice;
                var finalPriceFromApi = priceOverviewFromApi.Final;
                // add a new game deal for this game
                if (finalPriceFromApi != finalPriceFromDB)
                {   //set the dealdate to expired for the game
                    Console.WriteLine($@"Deal Price is outdated for {gamedeal.GameId} | {gamedeal.Game.Title} | Price Went from {finalPriceFromDB} to {finalPriceFromApi}");
                    await AddGameDeal(priceOverviewFromApi, steamid, gamedeal.GameId);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else if (gamedeal.PriceOverview != null && priceOverviewFromApi == null)
            {
                throw new Exception
                ($@"Priceoverview is not null on the database but the api is null. 
                            The game could be free perm. SteamId: {steamid} \n GameId {gamedeal.GameId}");
            }
            else
            {
                return false;
            }

        }



        private async Task<int> UpdateSteamGames(HashSet<GameDeal> gamesDealToCheck)
        {


            var steamAppIds = gamesDealToCheck.Select(gd => gd.Game.SteamApp.SteamId).Split(700);

            var totalUpdatedROW = 0;

            foreach (List<int> ids in steamAppIds)
            {
                var steamData = await _steamAPI.GetPriceBySteamIdAsync(ids);
                var dealsToUpdate = new HashSet<GameDeal>(new List<GameDeal>());

                foreach (var data in steamData)
                {
                      int steamid = Int32.Parse(data.Key);

                        bool dataSuccessed = data.Value.Success;

                        if (dataSuccessed)
                        {
                            var price = data.Value.Data != null ? data.Value.Data.PriceOverview : null;

                            if (price != null)
                            {
                                var gamedeal = gamesDealToCheck.FirstOrDefault(gd => gd.Game.SteamApp.SteamId == steamid
                                   && gd.PriceOverview.Currency.Code == price.Currency.Trim());

                                if (gamedeal != null)
                                {
                                    bool priceChange = await PriceChange(gamedeal, price);

                                    if (priceChange)
                                    {
                                        gamedeal.DealDate.Expired = true;
                                        gamedeal.DealDate.ExpiredDate = DateTime.Now;
                                        dealsToUpdate.Add(gamedeal);
                                    }
                                }
                                else
                                {
                                    var game = await _gamecity.GetGameBySteamIdAsync(steamid);
                                    if (game == null)
                                    {
                                        throw new Exception("Game does not exist in DB");
                                    }
                                    Console.WriteLine($"The following Game does not have new deal {game.Title}: {steamid} | price {price.FinalFormat}");

                                    await AddGameDeal(price, steamid, game.GameID);
                                }
                            }
                            else
                            {
                                Console.WriteLine($"The following game does not have any price data associated with it {steamid}");
                            }

                        }
                        else
                        {
                            Console.WriteLine("Game is no longer avaliable");
                        }
                  
                }
                //update the database and expire the games
                totalUpdatedROW += await _gamecity.UpdateMultipleGameDeals(dealsToUpdate.ToList());

            }

            return totalUpdatedROW;

        }

        private async void CheckSteamGameLimitedTimeSales(CancellationToken token)
        {
            Console.WriteLine("Check Steam Limited Time Sale Services Started");

            do
            {

                var gameDealsCurrentlyOnSale = new HashSet<GameDeal>(await _gamecity.GetSteamGamesOnSale());

                var totalUpdatedROW = await UpdateSteamGames(gameDealsCurrentlyOnSale);

                Console.WriteLine($"the total amount of limited TimeSales updated {totalUpdatedROW}");

                // check every 30 minutes if prices have changed
                await Task.Delay(TimeSpan.FromMinutes(30));

            }

            while (!token.IsCancellationRequested);


        }

        private async void CheckSteamGameDailyPrice(CancellationToken token)
        {
            Console.WriteLine("Check Steam GameDaily Price Services Started");
            do
            {

                var allSteamGameDeals = new HashSet<GameDeal>(await _gamecity.GetAllSteamGameDeals());


                var totalUpdatedROW = await UpdateSteamGames(allSteamGameDeals);

                Console.WriteLine($"the total amount of prices updated {totalUpdatedROW}");

                // check every 30 minutes if prices have changed
                await Task.Delay(TimeSpan.FromDays(1));

            }

            while (!token.IsCancellationRequested);

        }


        public void Start()
        {
            // CheckSteamGameLimitedTimeSales(_tokenSource.Token);
            // CheckSteamGameDailyPrice(_tokenSource.Token);
            PopualteFromSteamWEBAPIDatabase(_tokenSource.Token);
        }


        private async void PopualteFromSteamWEBAPIDatabase(CancellationToken token)
        {
            do
            {
                var apps = await GetListOfGamesNotInDBAsync();
                try
                {
                    foreach (var steamApp in apps)

                    {
                        // steam full details from api
                        var sd = await _steamAPI.GetAppBySteamIDAsync(steamApp.appid);


                        if (sd != null)
                        {
                            Guid gameID;

                            try
                            {
                                gameID = await AddGame(sd);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($@"following game could not be added due to the following error {e.Message}. 
                                Steam Id has been added to database to track. SteamId {sd.SteamAppID} from api and {steamApp.appid} from old api");
                                Console.WriteLine(e.Message);
                                throw e;
                                _gamecity.CreateSteamApp(new Domain.DatabaseModel.SteamApp
                                {
                                    SteamId = steamApp.appid,
                                });
                                
                                continue;
                            }
                            await AddGameCategories(sd.Categories, gameID);
                            await AddGameDeal(sd.PriceOverview, sd.SteamAppID, gameID);
                            await AddGameDevelopers(sd.Developers, gameID);
                            await AddGameDLC(sd.DLC, gameID);
                            await AddGameGenre(sd.Genres, gameID);
                            await AddGamePublishers(sd.Publishers, gameID);
                            await AddSystemRequirement(sd, gameID);
                            await AddScreenshots(sd.Screenshots, gameID);
                            await AddVideo(sd.Movies, gameID);



                        }
                        else
                        {

                            _gamecity.CreateSteamApp(new Domain.DatabaseModel.SteamApp
                            {
                                SteamId = steamApp.appid,
                            });
                        }


                    }

                    Console.WriteLine("Waiting 1 hour for next batch");
                    await Task.Delay(TimeSpan.FromHours(1), token);



                }
                catch (TaskCanceledException e)
                {
                    Console.WriteLine("Scrapping has been cancelled");
                    break;
                }

            }
            while (!token.IsCancellationRequested);


        }

        private async Task AddVideo(List<Movie> videos, Guid gameID)
        {

            if (videos != null)
            {
                var filtered = videos.Select(m =>
                {

                    return new Domain.DatabaseModel.Video
                    {
                        Title = m.Name,
                        Thumbnail = m.Thumbnail,
                        GameId = gameID,
                        VideoContent = new List<VideoContent>
                        {
                         new VideoContent { Quality = m.MP4.Quality, Max = m.MP4.Max, MediaType = "mp4" },
                        new VideoContent { Quality = m.Webm.Quality, Max = m.Webm.Max, MediaType = "webm" },
                        },
                        Highlight = m.Highlight
                    };
                }).ToList();

                await _gamecity.AddVieoAsync(filtered);
            }
        }

        private async Task AddScreenshots(List<SteamAPI.Models.Screenshot> screenshots, Guid gameID)
        {

            if (screenshots != null)
            {

                var filtered = screenshots.Select(s =>
                {
                    return new Domain.DatabaseModel.Screenshot
                    {
                        GameId = gameID,
                        PathFull = s.PathFull,
                        PathThumbnail = s.PathFull
                    };
                }
                ).ToList();

                await _gamecity.AddScreenshotAsync(filtered);
            }

        }

        private async Task AddSystemRequirement(SteamAppDetails sd, Guid gameID)
        {
            var systemRequirements = new List<Domain.DatabaseModel.SystemRequirement>();

            if (sd.PcRequirement != null)
            {
                systemRequirements.Add(new Domain.DatabaseModel.SystemRequirement
                {
                    GameId = gameID,
                    Minimum = sd.PcRequirement.Minimum,
                    Recommended = sd.PcRequirement.Recommended,
                    Platform = new Domain.DatabaseModel.Platform { Name = "pc" }
                });
            }
            if (sd.LinuxRequirement != null)
            {
                systemRequirements.Add(new Domain.DatabaseModel.SystemRequirement
                {
                    GameId = gameID,
                    Minimum = sd.LinuxRequirement.Minimum,
                    Recommended = sd.LinuxRequirement.Recommended,
                    Platform = new Domain.DatabaseModel.Platform { Name = "linux" }
                });
            }
            if (sd.MacRequirement != null)
            {
                systemRequirements.Add(new Domain.DatabaseModel.SystemRequirement
                {
                    GameId = gameID,
                    Minimum = sd.MacRequirement.Minimum,
                    Recommended = sd.MacRequirement.Recommended,
                    Platform = new Domain.DatabaseModel.Platform { Name = "mac" }
                });

            }

            await _gamecity.AddSystemRequirementAsync(systemRequirements);
        }

        private async Task AddGamePublishers(List<string> publisher, Guid gameID)
        {
            if (publisher != null)
            {

                var set = new HashSet<string>(publisher);
                var filted = set.Select(c => new Publisher { Name = c }).ToList();

                var publisherGuids = await _gamecity.AddPublisherAsync(filted);

                var gamepublisherToAdd = publisherGuids.Select(id => new GamePublisher { GameId = gameID, PublisherId = id }).ToList();

                await _gamecity.AddGamePublisherAsync(gamepublisherToAdd);
            }
        }



        private async Task AddGameGenre(List<GenreModel> genres, Guid gameID)
        {
            if (genres != null)
            {


                var filted = genres.Select(c => new Genre { Description = c.Description }).ToList();

                var genreGuids = await _gamecity.AddGenreAsync(filted);

                var gameGenreToAdd = genreGuids.Select(id => new GameGenre { GameId = gameID, GenreId = id }).ToList();

                await _gamecity.CreateGameGenresAsync(gameGenreToAdd);
            }
        }


        private async Task AddGameDLC(List<int> gameDLC, Guid gameID)
        {
            if (gameDLC != null)
            {


                var filted = gameDLC.Select(c => new DLC { SteamAppID = c }).ToList();

                var dlcGuids = await _gamecity.AddDLCAsync(filted);

                var gameDLCToAdd = dlcGuids.Select(id => new GameDLC { GameId = gameID, DLCId = id }).ToList();

                await _gamecity.CreateGameDLC(gameDLCToAdd);
            }
        }

        private async Task AddGameDevelopers(List<string> developers, Guid gameID)
        {
            if (developers != null)
            {

                var set = new HashSet<string>(developers);

                var filted = set.Select(c => new Developer { Name = c }).ToList();

                var developersGuids = await _gamecity.AddDevelopersAsync(filted);

                var gamedevelopers = developersGuids.Select(id => new GameDeveloper { GameId = gameID, DeveloperId = id }).ToList();

                await _gamecity.CreateGameDevelopers(gamedevelopers);
            }
        }




        //pricing
        private async Task AddGameDeal(SteamAPI.Models.PriceOverview priceOverview, int steamAppId, Guid gameId)
        {
            const string storeName = "steam";


            string BASEURL = $"https://store.steampowered.com/app/{steamAppId}";



            var gameDeal = new GameDeal
            {
                Url = BASEURL,
                Store = new Store
                {
                    Name = storeName,
                },
                GameId = gameId,
                DealDate = new DealDate
                {
                    DatePosted = DateTime.Now,
                    ExpiringDate = null,
                    Expired = false,
                },


            };


            var PriceOverview = priceOverview;

            if (PriceOverview != null)
            {


                string currencyCode = priceOverview.Currency;


                var PriceOverviews = new Domain.DatabaseModel.PriceOverview()
                {
                    Price = PriceOverview.Initial,
                    PriceFormat = PriceOverview.InitialFormat,
                    FinalPrice = PriceOverview.Final,
                    FinalPriceFormat = PriceOverview.FinalFormat,
                    Currency = new Currency
                    {
                        Code = currencyCode

                    },
                    DiscountPercentage = PriceOverview.DiscountPercentage

                };

                gameDeal.DealDate.LimitedTimeDeal = PriceOverview.DiscountPercentage > 0;
                gameDeal.PriceOverview = PriceOverviews;
                gameDeal.IsFree = PriceOverview.DiscountPercentage == 100;
                if (gameDeal.IsFree)
                {
                    Console.WriteLine("Game is now free");
                }


            }
            else
            {
                gameDeal.IsFree = true;
            }

            await _gamecity.CreateGameDealAsync(gameDeal);

        }



        private async Task AddGameCategories(List<CategoryModel> categories, Guid gameId)
        {
            if (categories != null)
            {


                var filted = categories.Select(c => new Category { Description = c.Description }).ToList();

                var categoriesGuids = await AddCategories(filted);

                var gameCategories = categoriesGuids.Select(id => new GameCategory { GameId = gameId, CategoryId = id }).ToList();

                await _gamecity.CreateGameCategores(gameCategories);
            }

        }

        private async Task<List<Guid>> AddCategories(List<Category> categories)
        {

            return await _gamecity.CreateCategoryAsync(categories);

        }

        private async Task<Guid> AddGame(SteamAppDetails sd)
        {
            var gameGuid = await _gamecity.CreateGameAsync(new Game
            {
                Title = sd.Name,
                Description = sd.Description,
                Type = sd.Type,
                About = sd.About,
                Website = sd.Website,
                Thumbnail = sd.HeaderImage,
                ReleaseDate = new ReleaseDate
                {
                    ComingSoon = sd.ReleaseDate.ComingSoon,
                    ReleasedDate = sd.ReleaseDate.ReleaseDate

                },
                Background = sd.Background,
                SteamApp = new Domain.DatabaseModel.SteamApp
                {
                    SteamId = sd.SteamAppID,


                }

            });

            return gameGuid;

        }

    }
}