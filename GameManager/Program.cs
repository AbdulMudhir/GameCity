
using System.Threading.Tasks;

using GameStoreServices.Steam;

namespace GameManager
{
    class Program
    {


        public async static Task Main(string[] args)
        {
            var SteamStore = new SteamGameService();

            SteamStore.RunAsync();

            SteamStore.updateReceived += OnUpdateReceived;

            await Task.Delay(-1);

        }

        public static void OnUpdateReceived(object source)
        {
            
          var game = (SteamGameService) source;

          System.Console.WriteLine(game.GetGame()?.Name);

           
        }

    }
}
