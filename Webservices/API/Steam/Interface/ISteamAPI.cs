
using System.Collections.Generic;
using System.Threading.Tasks;
using Webservices.Models.Steam.BaseAppModel;
using Webservices.Models.Steam.FullGameModel;
using Webservices.Models.Steam.PriceoverviewModel;

namespace Webservices.API.Steam.Interface
{
    public interface ISteamAPI
    {
        Task<SteamAppDetails> GetAppBySteamIDAsync(int steamID);
        Task<List<SteamApp>> GetAppsAsync();

        Task<Dictionary<string, BasePriceOverviewResponse>> GetPriceBySteamIdAsync(List<int> steamID);

        Task<PriceOverview> GetPriceBySteamIdAsync(int steamID);

    }
}