using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.DatabaseModel;
using GameStoreServices.Abstracts;
using GameStoreServices.Extension;
using Microsoft.EntityFrameworkCore;
using Webservices.API.Steam.Interface;
using Webservices.Models.Steam.FullGameModel;
using Webservices.Models.Steam.PriceoverviewModel;
using PriceOverview = Webservices.Models.Steam.FullGameModel.PriceOverview;

namespace GameStoreServices.Steam
{
    public class SteamPriceService : BaseSteamService
    {

        public event Action<List<PriceOverviewUpdateResponse>> SteamAPPSalePriceUpdatRecieved;
        public event Action<List<PriceOverviewUpdateResponse>> SteamAPPPriceUpdatRecieved;

        public SteamPriceService(ISteamAPI api) : base(api)
        {

        }


        private async Task<List<GameDeal>> GetSteamGamesOnSaleAsync()
        {

            var gameDealsCurrentlyOnSale = await _databaseContext.GameDeal
                          .Where(gd => gd.PriceOverview.DiscountPercentage > 0 && gd.Store.Name == "steam" && !gd.DealDate.Expired)
                          .Include(p => p.PriceOverview)
                          .Include(g => g.Game)
                          .Include(s => s.Game.SteamApp)
                          .Include(gd => gd.PriceOverview.Currency)
                          .Include(dd => dd.DealDate).ToListAsync();


            return gameDealsCurrentlyOnSale;
        }

        private async Task<List<Game>> GetSteamAppsAsync()
        {

            var games = await _databaseContext.Game
                          .Where(x => x.SteamApp != null && x.SteamApp.ValidSteamId)
                          .Include(x => x.SteamApp)
                          .Include(x => x.GameDeals.Where(g => g.PriceOverview != null
                          && !g.DealDate.Expired && !g.DealDate.LimitedTimeDeal && g.Available)).ThenInclude(x => x.DealDate)
                          .Include(x => x.GameDeals.Where(g =>
                          g.PriceOverview != null && !g.DealDate.Expired &&
                          !g.DealDate.LimitedTimeDeal && g.Available)).ThenInclude(x => x.PriceOverview)
                          .ThenInclude(gd => gd.Currency)
                          .ToListAsync();


            return games;
        }

        protected void OnSalePriceUpdateReceived(List<PriceOverviewUpdateResponse> priceoverview)
        {
            SteamAPPSalePriceUpdatRecieved?.Invoke(priceoverview);
        }
        protected void OnPriceUpdateReceived(List<PriceOverviewUpdateResponse> priceoverview)
        {
            SteamAPPPriceUpdatRecieved?.Invoke(priceoverview);
        }

        private async void RetrieveSteamGameSalesPriceAsync()
        {

            var steamAppsCurrentlyOnSale = await GetSteamGamesOnSaleAsync();

            var steamAppsIdSplit = steamAppsCurrentlyOnSale.Select(gd => gd.Game.SteamApp.SteamId).Split(600);

            foreach (var steamappIds in steamAppsIdSplit)
            {
                var priceOverviewreponse = new List<PriceOverviewUpdateResponse>();

                var steamAppsBaseResponse = await _steamAPI.GetPriceBySteamIdAsync(steamappIds);


                priceOverviewreponse.AddRange(await ParseSteamAppBaseResponseAsync(steamAppsCurrentlyOnSale, steamAppsBaseResponse));
                OnSalePriceUpdateReceived(priceOverviewreponse);



            }



            Console.WriteLine("All Game Deal Price Received.");

        }


        private async void RetrieveSteamAppPriceAsync()
        {

            var steamApps = new HashSet<Game>(await GetSteamAppsAsync());



            var steamAppsIdSplit = steamApps.Select(gd => gd.SteamApp.SteamId).Split(600);

            foreach (var steamappIds in steamAppsIdSplit)
            {
                var priceOverviewreponse = new List<PriceOverviewUpdateResponse>();

                var steamAppsBaseResponse = await _steamAPI.GetPriceBySteamIdAsync(steamappIds);


                priceOverviewreponse.AddRange(await ParseSteamAppBaseResponseAsync(steamApps, steamAppsBaseResponse));

                OnPriceUpdateReceived(priceOverviewreponse);

            }


            Console.WriteLine("All Steam App  Price Received.");

        }
        private async Task<List<PriceOverviewUpdateResponse>> ParseSteamAppBaseResponseAsync(HashSet<Game> source, Dictionary<string, BasePriceOverviewResponse> responses)
        {
            var gamedeals = new List<GameDeal>();

            foreach (var game in source)
            {
                if (game.GameDeals.Count == 0)
                {
                    gamedeals.Add(new GameDeal { Game = game });
                }
                else
                {
                    gamedeals.AddRange(game.GameDeals);
                }
            }

            return await ParseSteamAppBaseResponseAsync(gamedeals, responses);
        }

        private async Task<List<PriceOverviewUpdateResponse>> ParseSteamAppBaseResponseAsync(List<GameDeal> source, Dictionary<string, BasePriceOverviewResponse> responses)
        {
            var priceoverviewdata = new List<PriceOverviewUpdateResponse>();


            foreach (var response in responses)
            {
                var priceoverviewFromAPI = response.Value.Data?.PriceOverview;
                var steamID = Int32.Parse(response.Key);

                if (priceoverviewFromAPI is null)
                {
                    var game = await _steamAPI.GetAppBySteamIDAsync(Int32.Parse(response.Key));

                    if (game != null)
                    {

                        if (game.IsFree || game.ReleaseDate.ComingSoon)
                        {
                            priceoverviewdata.Add(new PriceOverviewUpdateResponse()
                            {
                                SteamAppId = game.SteamAppID,
                                GameDeal = source.FirstOrDefault(x => x.Game.SteamApp.SteamId == game.SteamAppID),
                                Priceoverview = priceoverviewFromAPI,
                                IsFree = game.IsFree

                            });

                            Console.WriteLine($"{game.Name} is F2P");


                        }

                        else
                        {
                            priceoverviewdata.Add(new PriceOverviewUpdateResponse()
                            {
                                SteamAppId = steamID,
                                GameDeal = source.FirstOrDefault(x => x.Game.SteamApp.SteamId == steamID),
                                Priceoverview = priceoverviewFromAPI,
                                Available = false,

                            });
                            Console.WriteLine($"{response.Key} | developer has requested for product not to be sold or product no longer exists");

                        }
                    }
                    else
                    {
                       var steamapp =  _databaseContext.SteamApp.FirstOrDefault(x => x.SteamId == steamID);

                       steamapp.ValidSteamId = false;

                        await _databaseContext.SaveChangesAsync();

                        Console.WriteLine($"{steamID} id is no longer available");
                    }

                    continue;
                }

                var gamedealsBySteamID = source.Where(x => x.Game.SteamApp.SteamId == Int32.Parse(response.Key));

                var gamedeal = gamedealsBySteamID.FirstOrDefault(x => x.PriceOverview?.Currency.Code == priceoverviewFromAPI.Currency

                );
                //currency on the database seems to have currency code for other regions
                if (gamedeal is null)
                {
                    // CREATE delegate here
                    var gamedealForReference = gamedealsBySteamID.FirstOrDefault();
                    priceoverviewdata.Add(new PriceOverviewUpdateResponse()
                    {
                        SteamAppId = gamedealForReference.Game.SteamApp.SteamId,
                        GameDeal = gamedealForReference,
                        Priceoverview = priceoverviewFromAPI,
                        IsNewCurrency = true
                    });

                    // Console.WriteLine($"Could not find the gamedeal for the following {response.Key} due to possiblity that currency code returned from api is not same as the one from database");

                }
                else
                {
                    priceoverviewdata.Add(new PriceOverviewUpdateResponse()
                    {
                        SteamAppId = gamedeal.Game.SteamApp.SteamId,
                        GameDeal = gamedeal,
                        Priceoverview = priceoverviewFromAPI
                    });

                }


            }





            return priceoverviewdata;
        }


        public async override void RunAsync()
        {
            do
            {
                RetrieveSteamAppPriceAsync();
                // RetrieveSteamGameSalesPriceAsync();





                await Task.Delay(TimeSpan.FromHours(5));


            } while (true);
        }



    }
}