using System;
using System.Net.Http;
using System.Threading.Tasks;
using CasCap.Apis.TokenBucket;
using Webservices.HttpService.Interface;

namespace Webservices.HttpService
{
   

    public class HttpRequestService : IHttpRequestService
    {
        private readonly ITokenBucket _tokenBucket;

        private readonly HttpClient _httpclient;

        private readonly TimeSpan _delayTimerIfStatusCodeFailsForRetry;

        private readonly int _maximumRetryBeforeFail;


        public HttpRequestService() : this(HttpFactories.GetHttpClient(), TimeSpan.FromMinutes(5), 5, RequestUtilityFactory.GetTokenBucket())
        {

        }

        public HttpRequestService(HttpClient httpclient) : this(httpclient, TimeSpan.FromMinutes(5), 5, RequestUtilityFactory.GetTokenBucket())
        {

        }

        public HttpRequestService(HttpClient httpclient, TimeSpan delayTimerIfStatusCodeFailsForRetry, int maximumRetryBeforeFail, ITokenBucket tokenBucket)
        {
            _httpclient = httpclient;
            _delayTimerIfStatusCodeFailsForRetry = delayTimerIfStatusCodeFailsForRetry;
            _maximumRetryBeforeFail = maximumRetryBeforeFail;
            _tokenBucket = tokenBucket;
        }

        public async Task<string> GetStringAsync(string url)
        {
            int attemptCounter = 0;
            do
            {
                _tokenBucket.Consume(1);

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
                        Console.WriteLine($"Issue occured when making connection to the API retry attempt will be made in {_delayTimerIfStatusCodeFailsForRetry}");
                        await Task.Delay(_delayTimerIfStatusCodeFailsForRetry);
                        attemptCounter++;
                    }
                    else
                    {

                        throw new Exception($"Something went wrong with the request {request.StatusCode}");
                    }
                }
            }
            while (attemptCounter != _maximumRetryBeforeFail);
            throw new Exception($"Steam Server could not be connection after {attemptCounter} attempts");

        }

    }
}