using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DatabaseModel;
using Persistence;

namespace Application
{
    public interface IDatabaseManager
    {
        Task<List<Guid>> AddDevelopersAsync(List<Developer> developers, DatabaseContext context = null);
        Task<List<Guid>> AddDLCAsync(List<DLC> dlcs, DatabaseContext context = null);
        Task AddGamePublisherAsync(List<GamePublisher> gamePublishers, DatabaseContext context = null);
        Task<List<Guid>> AddGenreAsync(List<Genre> genres, DatabaseContext context = null);
        Task<List<Guid>> AddPublisherAsync(List<Publisher> publishers, DatabaseContext context = null);
        Task AddScreenshotAsync(List<Screenshot> screenshots);
        Task AddSystemRequirementAsync(List<SystemRequirement> requirements, DatabaseContext context = null);
        Task AddVieoAsync(List<Video> videos);
        Task<List<Guid>> CreateCategoryAsync(List<Category> categories, DatabaseContext context = null);
        Task<Guid> CreateGameAsync(Game game, DatabaseContext context = null);
        Task CreateGameCategores(List<GameCategory> gameCategories, DatabaseContext context = null);
        Task CreateGameDealAsync(GameDeal deal, DatabaseContext context = null);
        Task CreateGameDevelopers(List<GameDeveloper> gamedevelopers, DatabaseContext context = null);
        Task CreateGameDLC(List<GameDLC> gameDLCToAdd, DatabaseContext context = null);
        Task CreateGameGenresAsync(List<GameGenre> gameGenreToAdd, DatabaseContext context = null);
        void CreateSteamApp(SteamApp steam);
        Task<List<GameDeal>> GetAllSteamGameDeals();
        Task<List<int>> GetAllSteamIdAsync(DatabaseContext context = null);
        Task<Currency> GetCurrencyByCode(string Code, DatabaseContext context = null);
        Task<Game> GetGameBySteamIdAsync(int SteamId, DatabaseContext context = null);
        Task<GameDeal> GetGameDealOnSaleBySteamIdAndStoreNameAsync(int steamId, string storeName, DatabaseContext context = null);
        Task<List<GameDeal>> GetSteamGamesOnSale();
        Task<Store> GetStoreIDByTitle(string name, DatabaseContext context = null);
        Task<int> UpdateMultipleGameDeals(List<GameDeal> deals);
    }
}