using System;
using System.Collections.Generic;
using System.Linq;
using Domain.DatabaseModel;
using GameStoreServices.Abstracts;
using Persistence;
using Webservices.Models.Steam.FullGameModel;

namespace GameManager
{
    public class SteamPriceDatabaseManager : DatabaseManager
    {

        private const string _storeName = "steam";


        private string baseUrl = $"https://store.steampowered.com/app/";


        public delegate void GameDealAddedEventHandler(GameDeal gameDeal);

        public event GameDealAddedEventHandler GameDealAdded;

        private GameDeal _gameDealAddedToDb { get; set; }



        public SteamPriceDatabaseManager(DatabaseContext databaseContext) : base(databaseContext)
        {

        }

        public override void OnUpdateReceived(GameService gameService)
        {
            throw new System.NotImplementedException();
        }

        // when a new game is added
        public void OnDatabaseUpdated(DatabaseManager databaseManager)
        {
            var gameManager = (SteamGameDatabaseManager)databaseManager;
            // making sure only when new game is added 


            AddGameDealAsync(gameManager.RecentGameAddedToDB, gameManager.SteamappDetails.PriceOverview);

        }


        public void OnSteamAppPriceRecieved(List<PriceOverviewUpdateResponse> priceoverviews)
        {   

            foreach(var priceoverview in priceoverviews)
            {
               
            }

        }



        private async void AddGameDealAsync(Game game, Webservices.Models.Steam.FullGameModel.PriceOverview priceOverview)
        {

            GameDeal gameDeal;
            Domain.DatabaseModel.PriceOverview po;

            // game is f2p
            if (priceOverview == null)
            {
                gameDeal = _databaseContext.GameDeal.FirstOrDefault(gd => gd.GameId == game.GameID && gd.Store.Name == _storeName && !gd.DealDate.Expired);

                po = null;
            }

            else
            {
                gameDeal = _databaseContext.GameDeal
                           .FirstOrDefault(gd => gd.GameId == game.GameID && gd.Store.Name == _storeName &&
                           gd.PriceOverview.Currency.Code == priceOverview.Currency && !gd.DealDate.Expired);


                string currencyCode = priceOverview.Currency;

                po = new Domain.DatabaseModel.PriceOverview()
                {
                    Price = priceOverview.Initial,
                    PriceFormat = priceOverview.InitialFormat,
                    FinalPrice = priceOverview.Final,
                    FinalPriceFormat = priceOverview.FinalFormat,
                    Currency = new Currency
                    {
                        Code = currencyCode

                    },
                    DiscountPercentage = priceOverview.DiscountPercentage

                };


            }


            if (gameDeal == null)
            {

                gameDeal = new GameDeal
                {
                    Url = baseUrl + game.SteamApp.SteamId,
                    Store = new Store
                    {
                        Name = _storeName,
                    },
                    GameId = game.GameID,
                    DealDate = new DealDate
                    {
                        DatePosted = DateTime.Now,
                        ExpiringDate = null,
                        Expired = false,
                    },
                    PriceOverview = po,


                };



                gameDeal.DealDate.LimitedTimeDeal = priceOverview == null ? false : (priceOverview.DiscountPercentage > 0);
                gameDeal.IsFree = priceOverview == null ? true : (priceOverview.DiscountPercentage == 100);

                _gameDealAddedToDb = await AddGameDealAsync(gameDeal);

                OnGameDealAdded();

            }

            else
            {
                _gameDealAddedToDb = null;
                // get function that updates price
                System.Console.WriteLine("Game deal Exist");

                throw new NotImplementedException("Game Deal Already Exist");
            }


        }


        public void OnGameDealAdded()
        {
            GameDealAdded?.Invoke(_gameDealAddedToDb);
        }

    }
}