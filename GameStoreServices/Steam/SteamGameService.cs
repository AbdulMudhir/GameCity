using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application;
using Application.Factory;
using GameStoreServices.Abstracts;
using Webservices.API.Factory;
using Webservices.API.Steam.Interface;
using Webservices.Models.Steam.BaseAppModel;
using Webservices.Models.Steam.FullGameModel;

namespace GameStoreServices.Steam
{
    public class SteamGameService : GameService
    {


        private readonly ISteamAPI _steamAPI;

        private readonly IDatabaseManager _databaseManager;

        private SteamAppDetails _game;


        public SteamGameService(ISteamAPI api):this(api, DatabaseManagerFactory.GetDatabaseManager())
        {

        }

        public SteamGameService() : this(APIFactory.GetSteamAPI())
        {

        }

        // filter out all steam games already in the database
        private async Task<List<SteamApp>> GetListOfGamesNotInDBAsync()
        {
            var set = new HashSet<int>(await _databaseManager.GetAllSteamIdAsync());

            var appsFromSteam = await _steamAPI.GetAppsAsync();

            appsFromSteam.RemoveAll(app => set.Contains(app.appid));

            return appsFromSteam;
        }

        public SteamGameService(ISteamAPI steamAPI, IDatabaseManager databaseManager)
        {
            _steamAPI = steamAPI;
            _databaseManager = databaseManager;
        }

        public override async void RunAsync()
        {

            do
            {
                var steamapps = await GetListOfGamesNotInDBAsync();

                foreach (var app in steamapps)
                {
                    var steamAppDetails = await _steamAPI.GetAppBySteamIDAsync(app.appid);

                    setGame(steamAppDetails);

                }

                await Task.Delay(TimeSpan.FromHours(5));


            } while (true);

        }

        public void setGame(SteamAppDetails game)
        {
            this._game = game;
            OnUpdateReceived(this);
        }

        
        public SteamAppDetails GetGame()
        {
            return this._game;
        }
    }
}