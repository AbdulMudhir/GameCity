using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameStoreServices.Abstracts;
using Persistence;
using Persistence.DBFactories;
using Webservices.API.Factory;
using Webservices.API.Steam;
using Webservices.API.Steam.Interface;
using Webservices.Models.Steam.BaseAppModel;
using Webservices.Models.Steam.FullGameModel;

namespace GameStoreServices.Steam
{
    public class SteamGameService : BaseSteamService
    {


        public SteamGameService(ISteamAPI api):base(api)
        {

        }


        private SteamAppDetails _game;

        private int _steamID;

        public int SteamID { get => _steamID; set => _steamID = value; }
        public SteamAppDetails Game { get => _game; set => _game = value; }


        // filter out all steam games already in the database
        private async Task<List<SteamApp>> GetListOfGamesNotInDBAsync()
        {
            var set = new HashSet<int>(_databaseContext.SteamApp.Select(app => app.SteamId));

            var appsFromSteam = await _steamAPI.GetAppsAsync();

            appsFromSteam.RemoveAll(app => set.Contains(app.appid));

            return appsFromSteam;
        }


        public override async void RunAsync()
        {

            do
            {
                var steamapps = await GetListOfGamesNotInDBAsync();
                
                Console.WriteLine($"Total Steam Apps Left {steamapps.Count}");

                foreach (var app in steamapps)
                {
                    var steamAppDetails = await _steamAPI.GetAppBySteamIDAsync(app.appid);
                    SteamID = app.appid;
                    Game = steamAppDetails;
                    System.Console.WriteLine(app.appid);
                    OnUpdateReceived(this);

                }

                Console.WriteLine("All Apps up to date. Checking next 1 hour for further update ");

                await Task.Delay(TimeSpan.FromHours(1));


            } while (true);

        }




    }
}