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

            var steamappdetails = gameManager.SteamappDetails;
            AddGameDealAsync(gameManager.RecentGameAddedToDB,
            new PriceOverviewUpdateResponse { IsFree = steamappdetails.IsFree, Priceoverview = steamappdetails.PriceOverview });

        }



        public async void OnSteamAppPriceRecieved(List<PriceOverviewUpdateResponse> priceoverviewupdateresponses)
        {

            foreach (var priceoverviewupdateresponse in priceoverviewupdateresponses)
            {
                var priceoverview = priceoverviewupdateresponse.Priceoverview;

                if (priceoverview is null)
                {
                    AddGameDealAsync(priceoverviewupdateresponse.GameDeal.Game,
                     new PriceOverviewUpdateResponse { Priceoverview = 
                     priceoverview, IsFree =priceoverviewupdateresponse.IsFree, 
                     Available =priceoverviewupdateresponse.Available  });

                     continue;
                }

                if (priceoverviewupdateresponse.IsNewCurrency)
                {
                    var gameDealForThisCurrencyExists = await
                    SteamGameDealExistsAsync("steam", priceoverviewupdateresponse.GameDeal.Game.SteamApp.SteamId, priceoverview.Currency);

                    if (gameDealForThisCurrencyExists)
                    {
                        throw new Exception("Game Deal for this currency exists implement an update feature for this");
                    }


                    AddGameDealAsync(priceoverviewupdateresponse.GameDeal.Game, new PriceOverviewUpdateResponse { Priceoverview = priceoverview });
                }
                else
                {
                    if (priceoverviewupdateresponse.GameDeal.Game.SteamApp.SteamId == 357070)
                    {
                        Console.WriteLine("here");
                    }

                    if (IsPriceEqual(priceoverviewupdateresponse.GameDeal.PriceOverview, priceoverview))
                    {
                        continue;
                    }
                    else
                    {

                        SetGameDealExpired(priceoverviewupdateresponse.GameDeal.GameDealId);
                        AddGameDealAsync(priceoverviewupdateresponse.GameDeal.Game, new PriceOverviewUpdateResponse { Priceoverview = priceoverview });
                    }
                }
            }

        }





        private bool IsPriceEqual(Domain.DatabaseModel.PriceOverview source, Webservices.Models.Steam.FullGameModel.PriceOverview api)
        {
            if (source.Currency.Code != api.Currency)
            {
                throw new Exception("Currency Code does not match");
            }
            else
            {
                var sourceFinalPrice = (int)source.FinalPrice;
                var apiFinalPrice = (int)api.Final;

                return sourceFinalPrice == apiFinalPrice;
            }
        }


        private async void AddGameDealAsync(Game game, PriceOverviewUpdateResponse priceOverviewUpdate)
        {


            var gameDeal = new GameDeal
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
                Available = priceOverviewUpdate.Available,
                IsFree = priceOverviewUpdate.IsFree

            };



            if (priceOverviewUpdate.Priceoverview != null)
            {
                gameDeal.PriceOverview = new Domain.DatabaseModel.PriceOverview()
                {
                    Price = priceOverviewUpdate.Priceoverview.Initial,
                    PriceFormat = priceOverviewUpdate.Priceoverview.InitialFormat,
                    FinalPrice = priceOverviewUpdate.Priceoverview.Final,
                    FinalPriceFormat = priceOverviewUpdate.Priceoverview.FinalFormat,
                    Currency = new Currency
                    {
                        Code = priceOverviewUpdate.Priceoverview.Currency

                    },
                    DiscountPercentage = priceOverviewUpdate.Priceoverview.DiscountPercentage

                };

                gameDeal.DealDate.LimitedTimeDeal = priceOverviewUpdate.Priceoverview.DiscountPercentage > 0;


                gameDeal.IsFree = (priceOverviewUpdate.Priceoverview.DiscountPercentage == 100) || priceOverviewUpdate.IsFree;

            }
    



            _gameDealAddedToDb = await AddGameDealAsync(gameDeal);

            OnGameDealAdded();



        }


        public void OnGameDealAdded()
        {
            GameDealAdded?.Invoke(_gameDealAddedToDb);
        }

    }
}