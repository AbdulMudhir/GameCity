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

        public SteamPriceService(ISteamAPI api) : base(api)
        {

        }

        private List<PriceOverviewUpdateResponse> _priceOverviewreponse;

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

        protected void OnSalePriceUpdateReceived(List<PriceOverviewUpdateResponse> priceoverview)
        {
            SteamAPPSalePriceUpdatRecieved?.Invoke(priceoverview);
        }


        private async void RetrieveSteamGameSalesPriceAsync()
        {
            _priceOverviewreponse = new List<PriceOverviewUpdateResponse>();

            var steamAppsCurrentlyOnSale = await GetSteamGamesOnSaleAsync();

            var steamAppsIdSplit = steamAppsCurrentlyOnSale.Select(gd => gd.Game.SteamApp.SteamId).Split(600);

            foreach (var steamappIds in steamAppsIdSplit)
            {
                var steamAppsBaseResponse = await _steamAPI.GetPriceBySteamIdAsync(steamappIds);


                _priceOverviewreponse.AddRange(ParseSteamAppBaseResponse(steamAppsCurrentlyOnSale, steamAppsBaseResponse));


            }


            OnSalePriceUpdateReceived(_priceOverviewreponse);

            Console.WriteLine("All Game Deal Price Received.");

        }


        private List<PriceOverviewUpdateResponse> ParseSteamAppBaseResponse(List<GameDeal> source, Dictionary<string, BasePriceOverviewResponse> responses)
        {
            var priceoverviewdata = new List<PriceOverviewUpdateResponse>();


            foreach (var response in responses)
            {
                var priceoverviewFromAPI = response.Value.Data?.PriceOverview;

                if (priceoverviewFromAPI is null)
                {
                    Console.WriteLine($"{response.Key} | developer has requested for product not to be sold or product no longer exists");
                    continue;
                }
                
                var gamedealsBySteamID = source.Where(x => x.Game.SteamApp.SteamId == Int32.Parse(response.Key));

                var gamedeal = gamedealsBySteamID.FirstOrDefault(x => x.PriceOverview.Currency.Code == priceoverviewFromAPI.Currency);
                //currency on the database seems to have currency code for other regions
                if (gamedeal is null)
                {   
                    // CREATE delegate here
                    var gamedealForReference = source.FirstOrDefault();
                    Console.WriteLine($"Could not find the gamedeal for the following {response.Key} due to possiblity that currency code returned from api is not same as the one from database");

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
                RetrieveSteamGameSalesPriceAsync();





                await Task.Delay(TimeSpan.FromHours(5));


            } while (true);
        }



    }
}