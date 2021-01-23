using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Webservices.API.Steam.Interface;
using Webservices.API.Steam.SteamEndPoints;
using Webservices.HttpService.Interface;
using Webservices.Models.Steam.BaseAppModel;
using Webservices.Models.Steam.FullGameModel;
using Webservices.Models.Steam.PriceoverviewModel;

namespace Webservices.API.Steam
{
    public class SteamAPI : ISteamAPI
    {

        private readonly IHttpRequestService _HttpRequestService;

        public SteamAPI(IHttpRequestService iHttpRequestService)
        {
            _HttpRequestService = iHttpRequestService;
        }

        public async Task<List<SteamApp>> GetAppsAsync()
        {
            var data = await _HttpRequestService.GetStringAsync(EndPoints.AllGAMESENDPOINT);

            return JsonConvert.DeserializeObject<BaseSteamApp>(data).applist.apps;
        }

        public async Task<Dictionary<string, BasePriceOverviewResponse>> GetPriceBySteamIdAsync(List<int> steamID)
        {

            var formatedUrl = String.Format(EndPoints.APPPRICEOVERVIEWENDPOINT, String.Join(",", steamID));


            var data = await _HttpRequestService.GetStringAsync(formatedUrl);

            var deserializedData = JsonConvert.DeserializeObject<Dictionary<string, BasePriceOverviewResponse>>(data);

            return deserializedData;
        }



        public async Task<SteamAppDetails> GetAppBySteamIDAsync(int steamID)
        {

            var data = await _HttpRequestService.GetStringAsync(EndPoints.APPDETAILSENDPOINT + steamID);

            var deserializedData = JsonConvert.DeserializeObject<Dictionary<string, BaseSteamResponse>>(data);

            var parsedData = deserializedData[steamID.ToString()];

            return parsedData.Success ? parsedData.Data : null;

        }

        public async Task<PriceOverview> GetPriceBySteamIdAsync(int steamID)
        {
            var response = await GetPriceBySteamIdAsync(new List<int> { steamID });
            var price = response[steamID.ToString()];

            return price.Success ? price.Data.PriceOverview : null;
        }
    }



}
