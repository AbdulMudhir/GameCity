
using System.Net.Http;

using SteamAPI.Interfaces;

namespace SteamAPI.Utilities
{
    public static class SteamFactory
    {
       

        public static HttpClient GetHttpClient()
        {
            return new HttpClient(handler:GetHttpClientHandler());
        }

        private static HttpClientHandler GetHttpClientHandler()
        {
            return new HttpClientHandler();
        }

        public static ISteamAPI GetSteamAPI()
        {

            return new SteamAPI.WebApi.SteamAPI(GetHttpClient());
        }

     
    }
}
