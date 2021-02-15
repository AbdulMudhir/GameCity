using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.DatabaseModel;
using GameManager.Utils;

namespace GameManager.Steam
{
    public class SteamManager
    {
    //     private readonly ISteamAPI _steamAPI;
    //     private readonly GameCityManager _gamecity;


    //     private CancellationTokenSource _tokenSource;


    //     public SteamManager() : this(SteamFactory.GetSteamAPI(), new GameCityManager())
    //     {
    //     }
    //     public SteamManager(ISteamAPI SteamApi, GameCityManager gameCity)
    //     {
    //         _steamAPI = SteamApi;
    //         _gamecity = gameCity;
    //         _tokenSource = new CancellationTokenSource();


    //     }

   


    //     public async Task<bool> PriceChange(GameDeal gamedeal, SteamAPI.Models.PriceOverview priceOverviewFromApi)

    //     {
    //         var steamid = gamedeal.Game.SteamApp.SteamId;

    //         if (gamedeal.PriceOverview != null && priceOverviewFromApi != null)
    //         {
    //             var finalPriceFromDB = (int)gamedeal.PriceOverview.FinalPrice;
    //             var finalPriceFromApi = priceOverviewFromApi.Final;
    //             // add a new game deal for this game
    //             if (finalPriceFromApi != finalPriceFromDB)
    //             {   //set the dealdate to expired for the game
    //                 Console.WriteLine($@"Deal Price is outdated for {gamedeal.GameId} | {gamedeal.Game.Title} | Price Went from {finalPriceFromDB} to {finalPriceFromApi}");
    //                 await AddGameDeal(priceOverviewFromApi, steamid, gamedeal.GameId);
    //                 return true;
    //             }
    //             else
    //             {
    //                 return false;
    //             }

    //         }
    //         else if (gamedeal.PriceOverview != null && priceOverviewFromApi == null)
    //         {
    //             throw new Exception
    //             ($@"Priceoverview is not null on the database but the api is null. 
    //                         The game could be free perm. SteamId: {steamid} \n GameId {gamedeal.GameId}");
    //         }
    //         else
    //         {
    //             return false;
    //         }

    //     }



    //     private async Task<int> UpdateSteamGames(HashSet<GameDeal> gamesDealToCheck)
    //     {


    //         var steamAppIds = gamesDealToCheck.Select(gd => gd.Game.SteamApp.SteamId).Split(700);

    //         var totalUpdatedROW = 0;

    //         foreach (List<int> ids in steamAppIds)
    //         {
    //             var steamData = await _steamAPI.GetPriceBySteamIdAsync(ids);
    //             var dealsToUpdate = new HashSet<GameDeal>(new List<GameDeal>());

    //             foreach (var data in steamData)
    //             {
    //                   int steamid = Int32.Parse(data.Key);

    //                     bool dataSuccessed = data.Value.Success;

    //                     if (dataSuccessed)
    //                     {
    //                         var price = data.Value.Data != null ? data.Value.Data.PriceOverview : null;

    //                         if (price != null)
    //                         {
    //                             var gamedeal = gamesDealToCheck.FirstOrDefault(gd => gd.Game.SteamApp.SteamId == steamid
    //                                && gd.PriceOverview.Currency.Code == price.Currency.Trim());

    //                             if (gamedeal != null)
    //                             {
    //                                 bool priceChange = await PriceChange(gamedeal, price);

    //                                 if (priceChange)
    //                                 {
    //                                     gamedeal.DealDate.Expired = true;
    //                                     gamedeal.DealDate.ExpiredDate = DateTime.Now;
    //                                     dealsToUpdate.Add(gamedeal);
    //                                 }
    //                             }
    //                             else
    //                             {
    //                                 var game = await _gamecity.GetGameBySteamIdAsync(steamid);
    //                                 if (game == null)
    //                                 {
    //                                     throw new Exception("Game does not exist in DB");
    //                                 }
    //                                 Console.WriteLine($"The following Game does not have new deal {game.Title}: {steamid} | price {price.FinalFormat}");

    //                                 await AddGameDeal(price, steamid, game.GameID);
    //                             }
    //                         }
    //                         else
    //                         {
    //                             Console.WriteLine($"The following game does not have any price data associated with it {steamid}");
    //                         }

    //                     }
    //                     else
    //                     {
    //                         Console.WriteLine("Game is no longer avaliable");
    //                     }
                  
    //             }
    //             //update the database and expire the games
    //             totalUpdatedROW += await _gamecity.UpdateMultipleGameDeals(dealsToUpdate.ToList());

    //         }

    //         return totalUpdatedROW;

    //     }

    //     private async void CheckSteamGameLimitedTimeSales(CancellationToken token)
    //     {
    //         Console.WriteLine("Check Steam Limited Time Sale Services Started");

    //         do
    //         {

    //             var gameDealsCurrentlyOnSale = new HashSet<GameDeal>(await _gamecity.GetSteamGamesOnSale());

    //             var totalUpdatedROW = await UpdateSteamGames(gameDealsCurrentlyOnSale);

    //             Console.WriteLine($"the total amount of limited TimeSales updated {totalUpdatedROW}");

    //             // check every 30 minutes if prices have changed
    //             await Task.Delay(TimeSpan.FromMinutes(30));

    //         }

    //         while (!token.IsCancellationRequested);


    //     }

    //     private async void CheckSteamGameDailyPrice(CancellationToken token)
    //     {
    //         Console.WriteLine("Check Steam GameDaily Price Services Started");
    //         do
    //         {

    //             var allSteamGameDeals = new HashSet<GameDeal>(await _gamecity.GetAllSteamGameDeals());


    //             var totalUpdatedROW = await UpdateSteamGames(allSteamGameDeals);

    //             Console.WriteLine($"the total amount of prices updated {totalUpdatedROW}");

    //             // check every 30 minutes if prices have changed
    //             await Task.Delay(TimeSpan.FromDays(1));

    //         }

    //         while (!token.IsCancellationRequested);

    //     }




    }
}