using System.Threading.Tasks;
using Domain.DatabaseModel;
using Persistence;
using Persistence.DBFactories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Application.GameCity
{
    public class GameCityManager
    {

        public async void CreateSteamApp(SteamApp steam)
        {
            using (var database = DbFactory.GetDatabaseContext())
            {
                var steamAppDB = database.SteamApp.Any(sa => sa.SteamId == steam.SteamId);

                if (!steamAppDB)
                {
                    database.SteamApp.Add(steam);

                    await database.SaveChangesAsync();
                }
            }

        }

        public async Task<List<GameDeal>> GetSteamGamesOnSale()
        {

            using (var database = DbFactory.GetDatabaseContext())
            {
                var games = await database.GameDeal
                           .Where(gd => gd.PriceOverview.DiscountPercentage > 0 &&
                               gd.Store.Name == "steam" && !gd.DealDate.Expired)
                           .Include(p => p.PriceOverview)
                           .Include(g => g.Game)
                           .Include(s => s.Game.SteamApp)
                           .Include(dd => dd.DealDate).ToListAsync();

                return games;
            }

        }

           public async Task<List<GameDeal>> GetAllSteamGameDeals()
        {

            using (var database = DbFactory.GetDatabaseContext())
            {
                var games = await database.GameDeal
                           .Where(gd =>gd.Store.Name == "steam" && !gd.DealDate.Expired 
                            && !gd.Game.ReleaseDate.ComingSoon && gd.PriceOverview != null)
                           .Include(p => p.PriceOverview)
                           .Include(g => g.Game)
                           .Include(s => s.Game.SteamApp)
                           .Include(dd => dd.DealDate).ToListAsync();

                return games;
            }

        }


        public async Task<GameDeal> GetGameDealOnSaleBySteamIdAndStoreNameAsync(int steamId, string storeName, DatabaseContext context = null)
        {
            using (var database = DbFactory.GetDatabaseContext())
            {
                var gamedeal = await database.GameDeal.Include(gd => gd.PriceOverview).FirstOrDefaultAsync(gd =>
                
                gd.Game.SteamApp.SteamId == steamId && gd.DealDate.LimitedTimeDeal
                && !gd.DealDate.Expired && gd.Store.Name.ToLower() == storeName.ToLower()
                );

                return gamedeal;
            }
        }

        public async Task AddVieoAsync(List<Video> videos)
        {
            using (var database = DbFactory.GetDatabaseContext())
            {
                database.Video.AddRange(videos);

                await database.SaveChangesAsync();
            }

        }


        public async Task<int> UpdateMultipleGameDeals(List<GameDeal> deals)
        {
            var totalUpdatedRow = 0;

            using (var database = DbFactory.GetDatabaseContext())
            {

                foreach (var deal in deals)
                {
                    var dealToUpdate = database.GameDeal.Find(deal.GameDealId);

                    dealToUpdate.DealDate = deal.DealDate ?? dealToUpdate.DealDate;
                    dealToUpdate.Url = deal.Url ?? dealToUpdate.Url;
                    dealToUpdate.Store = deal.Store ?? dealToUpdate.Store;
                    dealToUpdate.PriceOverview = deal.PriceOverview ?? dealToUpdate.PriceOverview;
                    dealToUpdate.DealDate = deal.DealDate ?? dealToUpdate.DealDate;
                    dealToUpdate.IsFree = deal.IsFree;

                    totalUpdatedRow += await database.SaveChangesAsync();

                }

                return totalUpdatedRow;
            }
        }


        public async Task AddScreenshotAsync(List<Screenshot> screenshots)
        {
            using (var database = DbFactory.GetDatabaseContext())
            {

                database.Screenshot.AddRange(screenshots);

                await database.SaveChangesAsync();
            }
        }

        public async Task CreateGameDealAsync(GameDeal deal, DatabaseContext context = null)
        {


            // using different databasecontext doesnt allow multiple objects to be inserted
            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                var storeDB = await database.Store.FirstOrDefaultAsync
                (s => s.Name.Trim().ToLower() == deal.Store.Name.Trim().ToLower());

                deal.Store = storeDB ?? deal.Store;

                if (deal.PriceOverview != null)
                {
                    var currencyCode = deal.PriceOverview.Currency.Code;

                    var currencyDB = await database.Currency.FirstOrDefaultAsync
                    (s => s.Code.Trim().ToLower() == currencyCode.Trim().ToLower());


                    deal.PriceOverview.Currency = currencyDB ?? deal.PriceOverview.Currency;


                }

                database.GameDeal.Add(deal);

                await database.SaveChangesAsync();
            }

        }


        public async Task<Store> GetStoreIDByTitle(string name, DatabaseContext context = null)
        {

            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                return await database.Store.FirstOrDefaultAsync(s => s.Name.Trim().ToLower() == name.Trim().ToLower());

            }
        }

        public async Task<Currency> GetCurrencyByCode(string Code, DatabaseContext context = null)
        {
            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                return await database.Currency.FirstOrDefaultAsync(s => s.Code.Trim().ToLower() == Code.Trim().ToLower());
            }
        }

        public async Task<Guid> CreateGameAsync(Game game, DatabaseContext context = null)
        {
            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                var gameDB = await database.Game.Where(g => g.SteamApp.SteamId == game.SteamApp.SteamId).FirstOrDefaultAsync();


                if (gameDB == null)
                {

                    database.Game.Add(game);

                    await database.SaveChangesAsync();

                    Console.WriteLine($"{game.Title} has been added");

                    return game.GameID;


                }
                else
                {
                    throw new Exception($"Game Already Exist, {game.SteamAppId} : {game.GameID}");
                }

            }
        }


        public async Task AddSystemRequirementAsync(List<SystemRequirement> requirements, DatabaseContext context = null)
        {
            using (var database = context ?? DbFactory.GetDatabaseContext())
            {

                var platformsDb = database.Platform;

                var filtered = requirements.Select(r =>
                {
                    r.Platform = platformsDb.FirstOrDefault(d => d.Name.Trim().ToLower() == r.Platform.Name.Trim().ToLower()) ?? r.Platform;
                    return r;
                });

                database.SystemRequirement.AddRange(filtered);


                await database.SaveChangesAsync();
            }
        }

        public async Task<List<Guid>> CreateCategoryAsync(List<Category> categories, DatabaseContext context = null)
        {

            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                var categoriesDB = new HashSet<Category>(database.Category);


                var categoriesToAdd = categories.Where(c => !categoriesDB.Any(db => c.Description.Trim().ToLower() == db.Description.Trim().ToLower()));


                database.Category.AddRange(categoriesToAdd);

                await database.SaveChangesAsync();

                var filteredCategoriesAdded = categories.Where(c => categoriesDB.Any(db => c.Description.Trim().ToLower() == db.Description.Trim().ToLower())).ToList();

                filteredCategoriesAdded.AddRange(categoriesToAdd);


                return filteredCategoriesAdded.Select(c => c.CategoryId).ToList();
            }

        }

        public async Task CreateGameCategores(List<GameCategory> gameCategories, DatabaseContext context = null)
        {

            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                database.GameCategory.AddRange(gameCategories);

                await database.SaveChangesAsync();
            }
        }

        public async Task<Game> GetGameBySteamIdAsync(int SteamId, DatabaseContext context = null)
        {

            using (var dbConext = context ?? DbFactory.GetDatabaseContext())
            {
                return await dbConext.Game.Where(g => g.SteamApp.SteamId == SteamId).FirstOrDefaultAsync();

            }

        }

        public async Task<List<int>> GetAllSteamIdAsync(DatabaseContext context = null)
        {
            using (var dbConext = context ?? DbFactory.GetDatabaseContext())
            {
                return await dbConext.SteamApp.Select(app => app.SteamId).ToListAsync();

            }

        }

        public async Task<List<Guid>> AddDevelopersAsync(List<Developer> filted, DatabaseContext context = null)
        {

            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                var developersDB = new HashSet<Developer>(database.Developer);


                var developersToAdd = filted.Where(c => !developersDB.Any(db => c.Name.Trim().ToLower() == db.Name.Trim().ToLower()));


                database.Developer.AddRange(developersToAdd);

                await database.SaveChangesAsync();

                var filteredDevelopersAdded = filted.Where(c => developersDB.Any(db => c.Name.Trim().ToLower() == db.Name.Trim().ToLower())).ToList();

                filteredDevelopersAdded.AddRange(developersToAdd);


                return filteredDevelopersAdded.Select(c => c.DeveloperId).ToList();
            }
        }

        public async Task CreateGameDevelopers(List<GameDeveloper> gamedevelopers, DatabaseContext context = null)
        {
            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                database.GameDeveloper.AddRange(gamedevelopers);

                await database.SaveChangesAsync();
            }

        }

        public async Task<List<Guid>> AddGenreAsync(List<Genre> filted, DatabaseContext context = null)
        {
            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                var genreDB = new HashSet<Genre>(database.Genre);


                var genreToAdd = filted.Where(c => !genreDB.Any(db => c.Description.Trim().ToLower() == db.Description.Trim().ToLower())).ToList();


                database.Genre.AddRange(genreToAdd);

                await database.SaveChangesAsync();

                var filteredGenreAdded = filted.Where(c => genreDB.Any(db => c.Description.Trim().ToLower() == db.Description.Trim().ToLower())).ToList();

                filteredGenreAdded.AddRange(genreToAdd);


                return filteredGenreAdded.Select(c => c.GenreId).ToList();
            }
        }


        public async Task<List<Guid>> AddPublisherAsync(List<Publisher> filted, DatabaseContext context = null)
        {

            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                var publisherDB = new HashSet<Publisher>(database.Publisher);


                var publisherToAdd = filted.Where(c => !publisherDB.Any(db => c.Name.Trim().ToLower() == db.Name.Trim().ToLower())).ToList();


                database.Publisher.AddRange(publisherToAdd);

                await database.SaveChangesAsync();

                var filteredPublisherAdded = publisherDB.Where(pb => filted.Any(f => f.Name.Trim().ToLower() == pb.Name.Trim().ToLower())).ToList();

                filteredPublisherAdded.AddRange(publisherToAdd);


                return filteredPublisherAdded.Select(f => f.PublisherId).ToList();
            }
        }


        public async Task AddGamePublisherAsync(List<GamePublisher> gamePublishers, DatabaseContext context = null)
        {
            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                database.GamePublisher.AddRange(gamePublishers);
                await database.SaveChangesAsync();
            }
        }


        public async Task CreateGameGenresAsync(List<GameGenre> gameGenreToAdd, DatabaseContext context = null)
        {
            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                database.GameGenre.AddRange(gameGenreToAdd);

                await database.SaveChangesAsync();
            }
        }

        public async Task<List<Guid>> AddDLCAsync(List<DLC> dLCs, DatabaseContext context = null)
        {
            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                var dLCsDB = new HashSet<DLC>(database.DLC);


                var dLCsToAdd = dLCs.Where(c => !dLCsDB.Any(db => c.SteamAppID == db.SteamAppID)).ToList();


                database.DLC.AddRange(dLCsToAdd);

                await database.SaveChangesAsync();

                var filteredDLCAdded = dLCs.Where(c => dLCsDB.Any(db => c.SteamAppID == db.SteamAppID)).ToList();

                filteredDLCAdded.AddRange(dLCsToAdd);


                return filteredDLCAdded.Select(c => c.DLCId).ToList();
            }
        }

        public async Task CreateGameDLC(List<GameDLC> gameDLCToAdd, DatabaseContext context = null)
        {
            using (var database = context ?? DbFactory.GetDatabaseContext())
            {
                database.GameDLC.AddRange(gameDLCToAdd);

                await database.SaveChangesAsync();
            }
        }
    }
}