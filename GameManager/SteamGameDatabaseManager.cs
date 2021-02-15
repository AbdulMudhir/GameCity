using System;
using System.Linq;
using Domain.DatabaseModel;
using GameStoreServices.Abstracts;
using GameStoreServices.Steam;
using Persistence;
using Webservices.Models.Steam.FullGameModel;

namespace GameManager
{
    public class SteamGameDatabaseManager : SteamGameInfoDatabaseManager
    {



        public SteamGameDatabaseManager(DatabaseContext databasemanager) : base(databasemanager)
        {
        }


        public override void OnUpdateReceived(GameService gameService)
        {
            var steamGameService = (SteamGameService)gameService;


            if (steamGameService.Game != null)
            {
                Console.WriteLine(steamGameService.Game.Name + " " + steamGameService.SteamID);
                AddSteamGameToDB(steamGameService.Game, steamGameService.SteamID);
            }
            else
            {
                AddSteamApp(steamGameService.SteamID);
                System.Console.WriteLine($"No GameData from Steam for this steam ID {steamGameService.SteamID}");
                // _databasemanager.SteamApp();
            }
        }


        private void AddSteamGameToDB(SteamAppDetails game, int defaultSteamID)
        {

            var gameDB = _databaseContext.Game.Any(g => g.SteamApp.SteamId == game.SteamAppID);

            if (!gameDB)
            {

                var newGame = new Game
                {
                    Title = game.Name,
                    Description = game.Description,
                    Type = game.Type,
                    About = game.About,
                    Website = game.Website,
                    Thumbnail = game.HeaderImage,
                    ReleaseDate = new ReleaseDate
                    {
                        ComingSoon = game.ReleaseDate.ComingSoon,
                        ReleasedDate = game.ReleaseDate.ReleaseDate

                    },
                    Background = game.Background,
                    SteamApp = new SteamApp
                    {
                        SteamId = game.SteamAppID,
                    }

                };


                _databaseContext.Game.Add(newGame);
                _databaseContext.SaveChanges();

                PopulateSteamInfo(game,newGame.GameID);

                RecentGameAddedToDB = newGame;
                SteamappDetails = game;
                OnDatabasedUpdated(this);

            }
            // add outdated steam id if there's one already associated with an account
            else if (gameDB && defaultSteamID != game.SteamAppID)
            {
                RecentGameAddedToDB = null;
                SteamappDetails = null;
                AddSteamApp(defaultSteamID, game.SteamAppID);

            }
            else
            {
                RecentGameAddedToDB = null;
                SteamappDetails = null;
                throw new Exception($"Game Already Exists | {game.SteamAppID}  | {game.Name}");
            }

        }

        private void AddSteamApp(int steamAppId, int? steamIDLinkedTo = null)
        {

            var steamAppDB = _databaseContext.SteamApp.Any(sa => sa.SteamId == steamAppId);

            if (!steamAppDB)
            {
                _databaseContext.SteamApp.Add(new SteamApp
                {
                    SteamId = steamAppId,
                    ValidSteamId = false,
                    SteamIdLinkedTo = steamIDLinkedTo,

                });
                _databaseContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Game Already Exists");
            }
        }


    }
}