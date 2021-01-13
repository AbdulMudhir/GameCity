
using System.Collections.Generic;
using System.Threading.Tasks;
using SteamAPI.Models;
using SteamAPI.Models.JsonDeserializeModel;

namespace SteamAPI.Interfaces
{
    public interface ISteamAPI
    {
        Task<SteamAppDetails> GetAppBySteamIDAsync(int steamID);
        Task<List<SteamApp>> GetAppsAsync();

        Task<Dictionary<string, PriceOverviewDetailsJsonModel>> GetPriceBySteamIdAsync(List<int> steamID);

    }
}