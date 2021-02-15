using Persistence;
using Persistence.DBFactories;
using Webservices.API.Factory;
using Webservices.API.Steam.Interface;

namespace GameStoreServices.Abstracts
{
    public abstract class BaseSteamService : GameService
    {
        protected readonly DatabaseContext _databaseContext;

        protected readonly ISteamAPI _steamAPI;

        protected BaseSteamService( ISteamAPI steamAPI,DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
            _steamAPI = steamAPI;
        }

        protected BaseSteamService(ISteamAPI api) : this(api, DbFactory.GetDatabaseContext())
        {

        }

        protected BaseSteamService() : this(APIFactory.GetSteamAPI())
        {

        }
    }
}