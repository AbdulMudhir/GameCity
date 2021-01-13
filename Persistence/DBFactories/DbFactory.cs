namespace Persistence.DBFactories
{
    public static class DbFactory
    {

        public static DatabaseContext GetDatabaseContext()
        {
            return new DatabaseContext();
        }
        
    }
}