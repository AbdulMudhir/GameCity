using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SteamAPI.Models;
using SteamAPI.Interfaces;
using SteamAPI.Utilities;
using SteamAPI.Models.JsonDeserializeModel;
using Newtonsoft.Json;
using Esendex.TokenBucket;

namespace SteamAPI.WebApi
{
    public class SteamAPI : ISteamAPI
    {

        private readonly HttpClient _httpclient;
        private readonly ITokenBucket _steamApiBucket;

        public SteamAPI() : this(SteamFactory.GetHttpClient())
        {

        }

        public SteamAPI(HttpClient httpclient)
        {
            _httpclient = httpclient;

            _steamApiBucket = TokenBuckets.Construct()
              .WithCapacity(1)
            .WithFixedIntervalRefillStrategy(1, TimeSpan.FromSeconds(1.5))
             .Build();
        }


        public async Task<List<SteamApp>> GetAppsAsync()
        {
            _steamApiBucket.Consume(1);
            var data = await _httpclient.GetStringAsync(SteamEndPointsConst.AllGAMESENDPOINTS);


            return JsonConvert.DeserializeObject<SteamAppsJsonModel>(data).applist.apps;

        }

        public async Task<Dictionary<string, PriceOverviewDetailsJsonModel>> GetPriceBySteamIdAsync(List<int> steamID)
        {

            _steamApiBucket.Consume(1);

            var formatedUrl = String.Format(SteamEndPointsConst.APPPRICEOVERVIEWENDPOINT, String.Join(",", steamID));


            var data = await _httpclient.GetStringAsync(formatedUrl);

            var deserializedData = JsonConvert.DeserializeObject<Dictionary<string, PriceOverviewDetailsJsonModel>>(data);

            return deserializedData;

        }

        public async Task<SteamAppDetails> GetAppBySteamIDAsync(int steamID)
        {


            _steamApiBucket.Consume(1);

            var data = await _httpclient.GetStringAsync(SteamEndPointsConst.APPDETAILSENDPOINT + steamID);

            var deserializedData = JsonConvert.DeserializeObject<Dictionary<string, SteamAppsDetailsJsonModel>>(data);

            var parsedData = deserializedData[steamID.ToString()];

            return parsedData.Success ? parsedData.Data : null;


        }



    }
