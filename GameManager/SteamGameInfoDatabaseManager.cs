using System;
using System.Collections.Generic;
using System.Linq;
using Domain.DatabaseModel;
using GameStoreServices.Abstracts;
using Persistence;
using Webservices.API.Steam;
using Webservices.Models.Steam.FullGameModel;

namespace GameManager
{
    public class SteamGameInfoDatabaseManager : DatabaseManager
    {
        public SteamGameInfoDatabaseManager(DatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override void OnUpdateReceived(GameService gameService)
        {
            throw new System.NotImplementedException();
        }

        protected async void PopulateSteamInfo(SteamAppDetails steamgame, Guid gameid)
        {
           
             AddGameCategories(steamgame.Categories, gameid);
             AddGameDevelopers(steamgame.Developers, gameid);
             AddGameDLC(steamgame.DLC, gameid);
             AddGameGenre(steamgame.Genres, gameid);
             AddGamePublishers(steamgame.Publishers, gameid);
             AddSystemRequirement(steamgame, gameid);
             AddScreenshots(steamgame.Screenshots, gameid);
             AddVideo(steamgame.Movies, gameid);
        }



        protected void AddVideo(List<Movie> videos, Guid gameID)
        {

            if (videos != null)
            {
                var filtered = videos.Select(m =>
                {

                    return new Domain.DatabaseModel.Video
                    {
                        Title = m.Name,
                        Thumbnail = m.Thumbnail,
                        GameId = gameID,
                        VideoContent = new List<VideoContent>
                        {
                         new VideoContent { Quality = m.MP4.Quality, Max = m.MP4.Max, MediaType = "mp4" },
                        new VideoContent { Quality = m.Webm.Quality, Max = m.Webm.Max, MediaType = "webm" },
                        },
                        Highlight = m.Highlight
                    };
                }).ToList();

                AddVideoAsync(filtered);
            }
        }



        protected void AddScreenshots(List<Webservices.Models.Steam.FullGameModel.Screenshot> screenshots, Guid gameID)
        {

            if (screenshots != null)
            {

                var filtered = screenshots.Select(s =>
                {
                    return new Domain.DatabaseModel.Screenshot
                    {
                        GameId = gameID,
                        PathFull = s.PathFull,
                        PathThumbnail = s.PathFull
                    };
                }
                ).ToList();

                AddScreenshotAsync(filtered);
            }

        }

        protected void AddSystemRequirement(SteamAppDetails sd, Guid gameID)
        {
            var systemRequirements = new List<Domain.DatabaseModel.SystemRequirement>();

            if (sd.PcRequirement != null)
            {
                systemRequirements.Add(new Domain.DatabaseModel.SystemRequirement
                {
                    GameId = gameID,
                    Minimum = sd.PcRequirement.Minimum,
                    Recommended = sd.PcRequirement.Recommended,
                    Platform = new Domain.DatabaseModel.Platform { Name = "pc" }
                });
            }
            if (sd.LinuxRequirement != null)
            {
                systemRequirements.Add(new Domain.DatabaseModel.SystemRequirement
                {
                    GameId = gameID,
                    Minimum = sd.LinuxRequirement.Minimum,
                    Recommended = sd.LinuxRequirement.Recommended,
                    Platform = new Domain.DatabaseModel.Platform { Name = "linux" }
                });
            }
            if (sd.MacRequirement != null)
            {
                systemRequirements.Add(new Domain.DatabaseModel.SystemRequirement
                {
                    GameId = gameID,
                    Minimum = sd.MacRequirement.Minimum,
                    Recommended = sd.MacRequirement.Recommended,
                    Platform = new Domain.DatabaseModel.Platform { Name = "mac" }
                });

            }

            AddSystemRequirementAsync(systemRequirements);
        }

        protected async void AddGamePublishers(List<string> publisher, Guid gameID)
        {
            if (publisher != null)
            {

                var set = new HashSet<string>(publisher, StringComparer.OrdinalIgnoreCase);
                var filted = set.Select(c => new Publisher { Name = c }).ToList();

                var publisherGuids = await AddPublisherAsync(filted);

                var gamepublisherToAdd = publisherGuids.Select(id => new GamePublisher { GameId = gameID, PublisherId = id }).ToList();

                AddGamePublisherAsync(gamepublisherToAdd);
            }
        }



        protected async void AddGameGenre(List<GenreModel> genres, Guid gameID)
        {
            if (genres != null)
            {


                var filted = genres.Select(c => new Genre { Description = c.Description }).ToList();

                var genreGuids = await AddGenreAsync(filted);

                var gameGenreToAdd = genreGuids.Select(id => new GameGenre { GameId = gameID, GenreId = id }).ToList();

                AddGameGenresAsync(gameGenreToAdd);
            }
        }


        protected async void AddGameDLC(List<int> gameDLC, Guid gameID)
        {
            if (gameDLC != null)
            {


                var filted = gameDLC.Select(c => new DLC { SteamAppID = c }).ToList();

                var dlcGuids = await AddDLCAsync(filted);

                var gameDLCToAdd = dlcGuids.Select(id => new GameDLC { GameId = gameID, DLCId = id }).ToList();

                AddGameDLC(gameDLCToAdd);
            }
        }

        protected async void AddGameDevelopers(List<string> developers, Guid gameID)
        {
            if (developers != null)
            {

                var set = new HashSet<string>(developers, StringComparer.OrdinalIgnoreCase);

                var filted = set.Select(c => new Developer { Name = c }).ToList();

                var developersGuids = await AddDevelopersAsync(filted);

                var gamedevelopers = developersGuids.Select(id => new GameDeveloper { GameId = gameID, DeveloperId = id }).ToList();

                AddGameDevelopers(gamedevelopers);
            }
        }


        protected async void AddGameCategories(List<CategoryModel> categories, Guid gameId)
        {
            if (categories != null)
            {


                var filted = categories.Select(c => new Category { Description = c.Description }).ToList();

                var categoriesGuids = await AddCategoryAsync(filted);

                var gameCategories = categoriesGuids.Select(id => new GameCategory { GameId = gameId, CategoryId = id }).ToList();

                AddGameCategores(gameCategories);
            }

        }


    }
}