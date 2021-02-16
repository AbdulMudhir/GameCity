using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DatabaseModel;
using GameStoreServices.Abstracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.DBFactories;

namespace GameManager
{
    public abstract class DatabaseManager
    {
        private Game _recentGameIdAddedToDB;

        private Webservices.Models.Steam.FullGameModel.SteamAppDetails _steamappDetails;


        public Game RecentGameAddedToDB { get => _recentGameIdAddedToDB; set => _recentGameIdAddedToDB = value; }
        public Webservices.Models.Steam.FullGameModel.SteamAppDetails SteamappDetails { get => _steamappDetails; set => _steamappDetails = value; }


        public delegate void DatabaseUpdatedEventHandler(DatabaseManager source);

        public event DatabaseUpdatedEventHandler databaseUpdated;
        protected readonly DatabaseContext _databaseContext;

        public DatabaseManager(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public abstract void OnUpdateReceived(GameService gameService);

        protected void OnDatabasedUpdated(DatabaseManager source)
        {
            databaseUpdated?.Invoke(this);
        }

        protected async void SetGameDealExpired(Guid gameDealId)
        {
            using (var databaseContext = DbFactory.GetDatabaseContext())
            {

                var gamedealDB = databaseContext.GameDeal.Include(x => x.DealDate)
                .FirstOrDefault(x => x.GameDealId == gameDealId);

                gamedealDB.DealDate.Expired = true;
                gamedealDB.DealDate.ExpiredDate = DateTime.Now;

                await databaseContext.SaveChangesAsync();

            }

        }

        protected async void AddVideoAsync(List<Domain.DatabaseModel.Video> videos)
        {

            using (var databaseContext = DbFactory.GetDatabaseContext())
            {

                databaseContext.Video.AddRange(videos);

                await databaseContext.SaveChangesAsync();
            }


        }

        protected async Task<bool> SteamGameDealExistsAsync(string store, int steamId, string currencyCode)
        {
            using (var databaseContext = DbFactory.GetDatabaseContext())
            {
                return await databaseContext
                          .GameDeal.AnyAsync(x => x.PriceOverview.Currency.Code
                         == currencyCode && x.Store.Name == store && !x.DealDate.Expired &&
                         x.Game.SteamApp.SteamId == steamId);
            }
        }

        public async void AddGameDevelopers(List<GameDeveloper> gamedevelopers)
        {

            using (var databaseContext = DbFactory.GetDatabaseContext())
            {

                databaseContext.GameDeveloper.AddRange(gamedevelopers);

                await databaseContext.SaveChangesAsync();
            }


        }



        protected async void AddGamePublisherAsync(List<GamePublisher> gamePublishers)
        {

            using (var databaseContext = DbFactory.GetDatabaseContext())
            {

                databaseContext.GamePublisher.AddRange(gamePublishers);
                await databaseContext.SaveChangesAsync();
            }

        }


        protected async void AddGameGenresAsync(List<GameGenre> gameGenreToAdd)
        {

            using (var databaseContext = DbFactory.GetDatabaseContext())
            {

                databaseContext.GameGenre.AddRange(gameGenreToAdd);

                await databaseContext.SaveChangesAsync();
            }

        }

        protected async void AddGameDLC(List<GameDLC> gameDLCToAdd)
        {

            using (var databaseContext = DbFactory.GetDatabaseContext())
            {

                databaseContext.GameDLC.AddRange(gameDLCToAdd);

                await databaseContext.SaveChangesAsync();
            }

        }


        protected void AddVideo(List<Video> videos)
        {

            _databaseContext.Video.AddRange(videos);

            _databaseContext.SaveChanges();


        }

        protected async void AddSystemRequirementAsync(List<SystemRequirement> requirements)
        {

            using (var databaseContext = DbFactory.GetDatabaseContext())
            {


                var platformsDb = databaseContext.Platform;

                var systemRequirements = requirements.Select(r =>
                {
                    r.Platform = platformsDb.FirstOrDefault(d => d.Name.Trim().ToLower() == r.Platform.Name.Trim().ToLower()) ?? r.Platform;
                    return r;
                });

                databaseContext.SystemRequirement.AddRange(systemRequirements);


                await databaseContext.SaveChangesAsync();
            }

        }

        protected async Task<List<Guid>> AddDevelopersAsync(List<Developer> developers)
        {



            using (var databaseContext = DbFactory.GetDatabaseContext())
            {

                // return any publishers not in database
                var developersNotInDB = developers.Where(c => !databaseContext.Developer
                .Any(db => c.Name.ToLower().Trim() == db.Name.ToLower().Trim()));

                databaseContext.Developer.AddRange(developersNotInDB);

                await databaseContext.SaveChangesAsync();

                // return all of the publishers from databased based on the list
                var developersInDB = developers.Select
                (p => databaseContext.Developer.First
               (pb => pb.Name.ToLower().Trim() == p.Name.ToLower().Trim()));

                return developersInDB.Select(f => f.DeveloperId).ToList();
            }
        }



        protected async Task<List<Guid>> AddGenreAsync(List<Genre> genres)
        {


            using (var databaseContext = DbFactory.GetDatabaseContext())
            {


                var genresNotInDB = genres.Where(c => !databaseContext.Genre
                .Any(db => c.Description.ToLower().Trim() == db.Description.ToLower().Trim()));

                databaseContext.Genre.AddRange(genresNotInDB);

                await databaseContext.SaveChangesAsync();

                // return all of the publishers from databased based on the list
                var genresInDB = genres.Select
                (p => databaseContext.Genre.First
               (pb => pb.Description.ToLower().Trim() == p.Description.ToLower().Trim()));

                return genresInDB.Select(f => f.GenreId).ToList();


            }
        }



        protected async Task<List<Guid>> AddPublisherAsync(List<Publisher> publishers)
        {


            using (var databaseContext = DbFactory.GetDatabaseContext())
            {

                // return any publishers not in database
                var publishersNotInDB = publishers.Where(c => !databaseContext.Publisher
                .Any(db => c.Name.ToLower().Trim() == db.Name.ToLower().Trim()));

                databaseContext.Publisher.AddRange(publishersNotInDB);

                await databaseContext.SaveChangesAsync();

                // return all of the publishers from databased based on the list
                var publishersInDB = publishers.Select
                (p => databaseContext.Publisher.First
               (pb => pb.Name.ToLower().Trim() == p.Name.ToLower().Trim()));

                return publishersInDB.Select(f => f.PublisherId).ToList();
            }

        }



        protected async Task<List<Guid>> AddDLCAsync(List<DLC> dlcs, DatabaseContext context = null)
        {


            using (var databaseContext = DbFactory.GetDatabaseContext())
            {

                // return any publishers not in database
                var dlcsNotInDB = dlcs.Where(c => !databaseContext.DLC
                .Any(db => c.SteamAppID == db.SteamAppID));

                databaseContext.DLC.AddRange(dlcsNotInDB);

                await databaseContext.SaveChangesAsync();

                // return all of the publishers from databased based on the list
                var dlcInDB = dlcs.Select(p =>
                databaseContext.DLC.First(pb => pb.SteamAppID == p.SteamAppID));

                return dlcInDB.Select(f => f.DLCId).ToList();
            }



        }

        protected async Task<GameDeal> AddGameDealAsync(GameDeal deal)
        {

            using (var databaseContext = DbFactory.GetDatabaseContext())
            {

                var storeDB = databaseContext.Store.FirstOrDefault
                (s => s.Name.Trim().ToLower() == deal.Store.Name.Trim().ToLower());

                deal.Store = storeDB ?? deal.Store;

                if (deal.PriceOverview != null)
                {
                    var currencyCode = deal.PriceOverview.Currency.Code;

                    var currencyDB = await databaseContext.Currency.FirstOrDefaultAsync
                    (s => s.Code.Trim().ToLower() == currencyCode.Trim().ToLower());


                    deal.PriceOverview.Currency = currencyDB ?? deal.PriceOverview.Currency;


                }

                databaseContext.GameDeal.Add(deal);

                await databaseContext.SaveChangesAsync();

                return deal;
            }


        }


        protected async void AddScreenshotAsync(List<Screenshot> screenshots)
        {

            using (var databaseContext = DbFactory.GetDatabaseContext())
            {

                databaseContext.Screenshot.AddRange(screenshots);

                await databaseContext.SaveChangesAsync();
            }

        }



        protected Store GetStoreIDByTitle(string name, DatabaseContext context = null)
        {


            return _databaseContext.Store.FirstOrDefault(s => s.Name.Trim().ToLower() == name.Trim().ToLower());

        }

        protected Currency GetCurrencyByCode(string Code, DatabaseContext context = null)
        {
            return _databaseContext.Currency.FirstOrDefault(s => s.Code.Trim().ToLower() == Code.Trim().ToLower());

        }


        protected async Task<List<Guid>> AddCategoryAsync(List<Category> categories)
        {

            using (var databaseContext = DbFactory.GetDatabaseContext())
            {



                // return any publishers not in database
                var categoriesNotInDB = categories.Where(c => !databaseContext.Category
                .Any(db => c.Description.ToLower().Trim() == db.Description.ToLower().Trim()));

                databaseContext.Category.AddRange(categoriesNotInDB);

                await databaseContext.SaveChangesAsync();

                // return all of the publishers from databased based on the list
                var categoriesInDB = categories.Select
                (p => databaseContext.Category.First
               (pb => pb.Description.ToLower().Trim() == p.Description.ToLower().Trim()));

                return categoriesInDB.Select(f => f.CategoryId).ToList();

            }
        }

        protected async void AddGameCategores(List<GameCategory> gameCategories)
        {

            using (var databaseContext = DbFactory.GetDatabaseContext())
            {
                databaseContext.GameCategory.AddRange(gameCategories);

                await databaseContext.SaveChangesAsync();
            }

        }

        protected async void AddSteamApp(SteamApp steam)
        {

            using (var databaseContext = DbFactory.GetDatabaseContext())
            {

                var steamAppDB = databaseContext.SteamApp.Any(sa => sa.SteamId == steam.SteamId);

                if (!steamAppDB)
                {
                    databaseContext.SteamApp.Add(steam);

                    await databaseContext.SaveChangesAsync();
                }
            }

        }




    }
}