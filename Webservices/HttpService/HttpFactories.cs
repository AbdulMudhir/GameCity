using System.Net.Http;

namespace Webservices.HttpService
{
    public static class HttpFactories
    {


        public static HttpClient GetHttpClient()
        {
            return new HttpClient(handler: GetHttpClientHandler());
        }

        private static HttpClientHandler GetHttpClientHandler()
        {
            return new HttpClientHandler();
        }

    }
}