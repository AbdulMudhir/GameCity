using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SteamAPI.Models;
using SteamAPI.Interfaces;
using SteamAPI.Utilities;
using SteamAPI.Models.JsonDeserializeModel;
using Newtonsoft.Json;
using CasCap.Apis.TokenBucket;

namespace SteamAPI.WebApi
{
    public class SteamAPI : ISteamAPI
    {

        private readonly HttpClient _httpclient;
        // will be used for rate limiting
        private readonly ITokenBucket _steamApiBucket;

        public SteamAPI() : this(SteamFactory.GetHttpClient())
        {

        }

        public SteamAPI(HttpClient httpclient)
        {
            _httpclient = httpclient;

            _steamApiBucket = TokenBuckets.Construct()
              .WithCapacity(1)
            .WithFixedIntervalRefillStrategy(1, TimeSpan.FromSeconds(5))
             .Build();
        }


        public async Task<List<SteamApp>> GetAppsAsync()
        {
            var data = await RequestHelperAsync(SteamEndPointsConst.AllGAMESENDPOINTS);


            if (data != null)
            {

                return JsonConvert.DeserializeObject<SteamAppsJsonModel>(data).applist.apps;
            }

            throw new Exception("Something wrong I cannot access steam APPS");

        }

        public async Task<Dictionary<string, PriceOverviewDetailsJsonModel>> GetPriceBySteamIdAsync(List<int> steamID)
        {

            

            var formatedUrl = String.Format(SteamEndPointsConst.APPPRICEOVERVIEWENDPOINT, String.Join(",", steamID));

            var t = formatedUrl.Length;

            var data = await RequestHelperAsync(formatedUrl);

            if (data != null)
            {
                var deserializedData = JsonConvert.DeserializeObject<Dictionary<string, PriceOverviewDetailsJsonModel>>(data);

                return deserializedData;

            }

            return null;
        }

        public async Task<SteamAppDetails> GetAppBySteamIDAsync(int steamID)
        {
            var url = SteamEndPointsConst.APPDETAILSENDPOINT + steamID;

            var data = await RequestHelperAsync(url);

            if (data != null)
            {
                var deserializedData = JsonConvert.DeserializeObject<Dictionary<string, SteamAppsDetailsJsonModel>>(data);
                var parsedData = deserializedData[steamID.ToString()];

                return parsedData.Success ? parsedData.Data : null;
            }

            return null;
        }

        public async Task<string> RequestHelperAsync(string url)
        {
            _steamApiBucket.Consume(1);

            var retry = 0;
            var delayTimerIfStatusCodeFails = TimeSpan.FromMinutes(5);

            do
            {
                using (var request = await _httpclient.GetAsync(url))
                {

                    if (request.IsSuccessStatusCode)
                    {
                        var data = await request.Content.ReadAsStringAsync();

                        if (data.Length > 0)
                        {

                            return data;
                        }

                        return null;

                    }
                    else if ((int)request.StatusCode >= 500)
                    {
                        Console.WriteLine(request.StatusCode);
                        Console.WriteLine("Issue occured when making connection to the API retry attempt will be made");
                        await Task.Delay(delayTimerIfStatusCodeFails);
                        delayTimerIfStatusCodeFails += TimeSpan.FromMinutes(5);
                        retry++;
                    }
                    else
                    {

                        throw new Exception($"Something went wrong with the request {request.StatusCode}");
                    }
                }
            }
            while (retry != 5);
            throw new Exception($"Steam Server could not be connection after {retry} attempts");

        }
    }



}
