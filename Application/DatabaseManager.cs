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
  

    public class DatabaseManager 
    {

       

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

       

      
    }
}