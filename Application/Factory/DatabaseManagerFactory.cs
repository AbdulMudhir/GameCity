namespace Application.Factory
{
    public static class DatabaseManagerFactory
    {
        


        public static IDatabaseManager GetDatabaseManager()
        {
            return new DatabaseManager();
        }
    }
}