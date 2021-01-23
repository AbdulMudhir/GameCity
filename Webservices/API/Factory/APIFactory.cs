
using Webservices.API.Steam;
using Webservices.API.Steam.Interface;
using Webservices.HttpService;

namespace Webservices.API.Factory
{
    public static class APIFactory
    {




        public static ISteamAPI GetSteamAPI()
        {

            return new SteamAPI(new HttpRequestService());
        }
    }
}