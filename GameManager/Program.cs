
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DatabaseModel;
using GameStoreServices.Steam;
using Persistence.DBFactories;
using Webservices.API.Factory;

namespace GameManager
{
    class Program
    {


        public async static Task Main(string[] args)
        {
            var steamApi = APIFactory.GetSteamAPI();

            var steamStore = new SteamGameService(steamApi);
            var steamPriceService = new SteamPriceService(steamApi);


            var DatabaseManager = new SteamGameDatabaseManager(DbFactory.GetDatabaseContext());
            var priceManager = new SteamPriceDatabaseManager(DbFactory.GetDatabaseContext());

            steamStore.updateReceived += DatabaseManager.OnUpdateReceived;
            DatabaseManager.databaseUpdated += priceManager.OnDatabaseUpdated;
            priceManager.GameDealAdded += OnDatabaseUpdated;


            steamPriceService.SteamAPPSalePriceUpdatRecieved += priceManager.OnSteamAppPriceRecieved;
            steamPriceService.SteamAPPPriceUpdatRecieved+= priceManager.OnSteamAppPriceRecieved;

            steamStore.RunAsync();
            steamPriceService.RunAsync();



            await Task.Delay(-1);

        }

        public static void OnDatabaseUpdated(GameDeal gameDeal)
        {
        }


    }
}
