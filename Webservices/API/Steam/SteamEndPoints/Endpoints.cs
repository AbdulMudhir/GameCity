namespace Webservices.API.Steam.SteamEndPoints
{
    public class EndPoints
    {
        public static string AllGAMESENDPOINT { get { return "https://api.steampowered.com/ISteamApps/GetAppList/v2/"; } }

        public static string APPDETAILSENDPOINT { get { return "https://store.steampowered.com/api/appdetails?appids="; } }

        public static string APPPRICEOVERVIEWENDPOINT { get { return "https://store.steampowered.com/api/appdetails?appids={0}&filters=price_overview"; } }

    }
}