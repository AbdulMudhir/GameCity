
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SteamAPI.Processors
{
    public class SteamProcessors
    {

        private readonly int _requestDelayTime;

        private const string _storeName = "steam";
     


   
        public async Task Start()
        {
            // var apps  = await _steamAPI.GetApps();

            // //var steamAppIdsFromDB = await _gameManager.GetAllSteamIdAsync();
  
            // //var set = new HashSet<int>(steamAppIdsFromDB);

            // apps.RemoveAll(i => set.Contains(i.appid));


            // for (var i = 0; i < apps.Count; i++)
            // {

            //     var app = apps[i];
            //     var fullApp = await _steamAPI.GetAppBySteamID(app.appid);

            //     if (fullApp != null)
            //     {
            //         if (String.IsNullOrEmpty(fullApp.Name))
            //         {
            //             fullApp.Name = fullApp.SteamAppID.ToString();
            //         }
            

            //     }
            //     else
            //     {
                

            //     }
            //     await Task.Delay(_requestDelayTime);


            // }


            Console.WriteLine("Database has finished");
        }

        


        //private async void AddFullGameAsync(SteamAppDetails fullApp)
        //{
        //    var gameId = await AddGame(fullApp);

        //    Console.WriteLine($"{fullApp.Name} has been updated");

        //    AddDlcAsync(fullApp.DLC, gameId);
        //    AddDeveloperAsync(fullApp.Developers, gameId);
        //    AddPublisherAsync(fullApp.Publishers, gameId);
        //    AddVideosAsync(fullApp.Movies, gameId, fullApp.Name);
        //    AddGameDeal(fullApp, gameId);
        //    AddCategoriesAsync(fullApp.Categories, gameId);
        //    AddGenreAsync(fullApp.Genres, gameId);
        //    AddSystemRequirements(fullApp, gameId);
        //}

        //private async void AddDlcAsync(List<int> Dlcs, int gameId)
        //{
        //    if(Dlcs != null)
        //    {
        //        foreach(var dlcId in Dlcs)
        //        {
        //            try
        //            {
        //                await _gameManager.AddGameDLC(new GameDLCAddModel
        //                {
        //                    GameId = gameId,
        //                    DLC = new DLCAddModel { SteamAppId = dlcId }
        //                });

        //            }

        //            catch(SqlException)
        //            {

        //            }
        //        }
        //    }

        //}
        //private async void AddVideosAsync(List<Movie> movies, int gameId, string title)
        //{
        //    if(movies != null)
        //    {
        //        foreach (var video in movies)
        //        {
        //            var videoToAdd = new VideoAddModel()
        //            {
        //                GameId = gameId,
        //                Title = string.IsNullOrEmpty(video.Name)? title : video.Name ,
        //                Thumbnail = video.Thumbnail
        //            };

        //            if (video.MP4 != null)
        //                videoToAdd.MP4 = new VideoContentAddModel
        //                {
        //                    Max = video.MP4.Max,
        //                    Quality = video.MP4.Quality,
        //                    MediaType = "mp4"
        //                };

        //            if(video.Webm != null)
        //                videoToAdd.Webm = new VideoContentAddModel
        //                {
        //                    Max = video.Webm.Max,
        //                    Quality = video.Webm.Quality,
        //                    MediaType = "webm"
        //                };
            

        //          await _gameManager.AddVideoAsync(videoToAdd);
        //        }
        //    }
        //}
  

        //private async void AddPublisherAsync(List<string> publishers, int gameId)
        //{
        //    if (publishers != null)
        //    {
        //        foreach(var publisher in publishers)
        //        {
        //            if (!string.IsNullOrEmpty(publisher))
        //            {
        //                await _gameManager.AddGamePublisherAsync(gameId,
        //                  publisher);
        //            }
        //        }

        //    }
               
        //}
        //private async void AddDeveloperAsync(List<string> developers, int gameId)
        //{
        //    if(developers != null)
        //    {
        //        foreach (var developer in developers)
        //        {
        //            if (!string.IsNullOrEmpty(developer))
        //            {
        //                await _gameManager.AddGameDeveloperAsync(gameId, developer);
        //            }
        //        }
        //    }
                  

        //}
        //private void AddGameDeal(SteamAppDetails app, int gameId)
        //{
        //    DateTime currentDateTime = DateTime.Now;
        //    string sqlFormattedDate = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");


        //    var gamedeal = new GameDealAddModel
        //    {
        //        GameId = gameId,
        //        DealDate = new DealDateAddModel { DatePosted = sqlFormattedDate },
        //        Url = _basedStoreURl + app.SteamAppID,
        //        IsFree = app.IsFree,
        //        Store = new StoreAddModel { Name = _storeName },
               
        //    };

        //    var priceOverview = app.PriceOverview;

        //    if (priceOverview != null)
        //    {
        //        gamedeal.PriceOverview = new PriceOverviewAddModel
        //        {
        //            Price = priceOverview.Initial,
        //            PriceFormat = priceOverview.InitialFormat,
        //            FinalPrice = priceOverview.Final,
        //            FinalPriceFormat = priceOverview.FinalFormat,
        //            DiscountPercentage = priceOverview.DiscountPercentage,
        //            Currency = new CurrencyAddModel { Code = priceOverview.Currency, Symbole = "£" }
        //        };

        //       if(priceOverview.Initial != priceOverview.Final)
        //        {
        //            gamedeal.DealDate.LimitedTimeDeal = true;
        //        }
        //    }
        //   _gameManager.AddGameDeal(gamedeal);
        //}

        //private void AddSystemRequirements(SteamAppDetails systemRequirement, int gameId)
        //{
        //    if (systemRequirement.PcRequirement != null)
        //    {
        //     _gameManager.AddSystemRequirement(new SystemRequirementAddModel
        //        {
        //            GameId = gameId,
        //            Minimum = systemRequirement.PcRequirement.Minimum,
        //            Recommended = systemRequirement.PcRequirement.Recommended,
        //            Platform = new PlatformAddModel { Name = "pc" }
        //        });
        //    }
        //    if (systemRequirement.LinuxRequirement != null)
        //    {
        //        _gameManager.AddSystemRequirement(new SystemRequirementAddModel
        //        {
        //            GameId = gameId,
        //            Minimum = systemRequirement.LinuxRequirement.Minimum,
        //            Recommended = systemRequirement.LinuxRequirement.Recommended,
        //            Platform = new PlatformAddModel { Name = "linux" }
        //        });
        //    }
        //    if (systemRequirement.MacRequirement != null)
        //    {
        //        _gameManager.AddSystemRequirement(new SystemRequirementAddModel
        //        {
        //            GameId = gameId,
        //            Minimum = systemRequirement.MacRequirement.Minimum,
        //            Recommended = systemRequirement.MacRequirement.Recommended,
        //            Platform = new PlatformAddModel { Name = "mac" }
        //        });
        //    }

        //}

        //private async void AddCategoriesAsync(List<CategoryModel> categories, int gameId)
        //{
        //    if (categories != null)
        //    {
        //        foreach (var category in categories)
        //        {
        //            await _gameManager.AddCategoryToGameByDescription(gameId, category.Description);
        //        };
        //    }
        //}

        //private async void AddGenreAsync(List<GenreModel> genres, int gameId)
        //{
        //    if (genres != null)
        //    {
        //        foreach (var genre in genres)
        //        {
        //           await _gameManager.AddGenreToGameByDescription(gameId, genre.Description);
        //        };
        //    }
        //}

        //private void AddSteamApp(SteamAppAddModel steamApp)
        //{
        //    _gameManager.AddSteamApp(steamApp);
        //}
        //private async Task<int> AddGame (SteamAppDetails fullApp)
        //{
        //    var releasedDate = fullApp.ReleaseDate.ReleaseDate;

        //    var fullGameModel = new FullGameAddModel
        //    {
        //        Title = fullApp.Name,
        //        Type = fullApp.Type,
        //        Website = fullApp.Website,
        //        Description = fullApp.Description,
        //        HeaderImage = fullApp.HeaderImage,
        //        Background = fullApp.Background,
        //        About = fullApp.About,
        //        ShortDescription = fullApp.ShortDescription,
        //        ReleaseDate = new ReleaseDateAddModel()
        //        {
        //            ComingSoon = fullApp.ReleaseDate.ComingSoon,
        //            ReleasedDate = String.IsNullOrEmpty(releasedDate) ? "Not confirmed" : releasedDate 

        //        },
        //        steamApp = new SteamAppAddModel()
        //        {
        //            SteamAppId = fullApp.SteamAppID,
        //            SteamReview = fullApp.Reviews,
        //            Valid = true
        //        },
        //        SteamAppId = fullApp.SteamAppID
               
        //};




        //    return await _gameManager.AddFullGameAsync(fullGameModel);

            
        //}




    }
}
