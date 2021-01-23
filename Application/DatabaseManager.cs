using System.Threading.Tasks;
using Domain.DatabaseModel;
using Persistence;
using Persistence.DBFactories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Application
{
  

    public class DatabaseManager : IDatabaseManager
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
                           .Include(gd => gd.PriceOverview.Currency)
                           .Include(dd => dd.DealDate).ToListAsync();

                return games;
            }

        }

        public async Task<List<GameDeal>> GetAllSteamGameDeals()
        {

            using (var database = DbFactory.GetDatabaseContext())
            {
                var games = await database.GameDeal
                           .Where(gd => gd.Store.Name == "steam" && !gd.DealDate.Expired
                            && !gd.Game.ReleaseDate.ComingSoon && gd.PriceOverview != null)
                           .Include(p => p.PriceOverview)
                           .Include(g => g.Game)
                           .Include(s => s.Game.SteamApp)
                            .Include(gd => gd.PriceOverview.Currency)
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
                var gameDB = database.Game.Where(g => g.SteamApp.SteamId == game.SteamApp.SteamId).FirstOrDefault();


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

                // return any publishers not in database
                var categoriesNotInDB = categories.Where(c => !database.Category
                .Any(db => c.Description.ToLower().Trim() == db.Description.ToLower().Trim()));

                database.Category.AddRange(categoriesNotInDB);

                await database.SaveChangesAsync();

                // return all of the publishers from databased based on the list
                var categoriesInDB = categories.Select
                (p => database.Category.First
               (pb => pb.Description.ToLower().Trim() == p.Description.ToLower().Trim()));

                return categoriesInDB.Select(f => f.CategoryId).ToList();




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

        public async Task<List<Guid>> AddDevelopersAsync(List<Developer> developers, DatabaseContext context = null)
        {

            using (var database = context ?? DbFactory.GetDatabaseContext())
            {


                // return any publishers not in database
                var developersNotInDB = developers.Where(c => !database.Developer
                .Any(db => c.Name.ToLower().Trim() == db.Name.ToLower().Trim()));

                database.Developer.AddRange(developersNotInDB);

                await database.SaveChangesAsync();

                // return all of the publishers from databased based on the list
                var developersInDB = developers.Select
                (p => database.Developer.First
               (pb => pb.Name.ToLower().Trim() == p.Name.ToLower().Trim()));

                return developersInDB.Select(f => f.DeveloperId).ToList();
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

        public async Task<List<Guid>> AddGenreAsync(List<Genre> genres, DatabaseContext context = null)
        {
            using (var database = context ?? DbFactory.GetDatabaseContext())
            {



                var genresNotInDB = genres.Where(c => !database.Genre
                .Any(db => c.Description.ToLower().Trim() == db.Description.ToLower().Trim()));

                database.Genre.AddRange(genresNotInDB);

                await database.SaveChangesAsync();

                // return all of the publishers from databased based on the list
                var genresInDB = genres.Select
                (p => database.Genre.First
               (pb => pb.Description.ToLower().Trim() == p.Description.ToLower().Trim()));

                return genresInDB.Select(f => f.GenreId).ToList();


            }
        }


        public async Task<List<Guid>> AddPublisherAsync(List<Publisher> publishers, DatabaseContext context = null)
        {

            using (var database = context ?? DbFactory.GetDatabaseContext())
            {

                // return any publishers not in database
                var publishersNotInDB = publishers.Where(c => !database.Publisher
                .Any(db => c.Name.ToLower().Trim() == db.Name.ToLower().Trim()));

                database.Publisher.AddRange(publishersNotInDB);

                await database.SaveChangesAsync();

                // return all of the publishers from databased based on the list
                var publishersInDB = publishers.Select
                (p => database.Publisher.First
               (pb => pb.Name.ToLower().Trim() == p.Name.ToLower().Trim()));

                return publishersInDB.Select(f => f.PublisherId).ToList();
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

        public async Task<List<Guid>> AddDLCAsync(List<DLC> dlcs, DatabaseContext context = null)
        {
            using (var database = context ?? DbFactory.GetDatabaseContext())
            {


                // return any publishers not in database
                var dlcsNotInDB = dlcs.Where(c => !database.DLC
                .Any(db => c.SteamAppID == db.SteamAppID));

                database.DLC.AddRange(dlcsNotInDB);

                await database.SaveChangesAsync();

                // return all of the publishers from databased based on the list
                var dlcInDB = dlcs.Select(p =>
                database.DLC.First(pb => pb.SteamAppID == p.SteamAppID));

                return dlcInDB.Select(f => f.DLCId).ToList();


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