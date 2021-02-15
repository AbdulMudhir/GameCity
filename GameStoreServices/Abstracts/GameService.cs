using System.Threading;
using System.Threading.Tasks;
using Persistence;

namespace GameStoreServices.Abstracts
{
    public abstract class GameService
    {


        public delegate void UpdateReceivedEventHandler(GameService source);

        public event UpdateReceivedEventHandler updateReceived;

        public abstract void RunAsync();


        protected void OnUpdateReceived(GameService source)
        {
            updateReceived?.Invoke(this);
        }
    }


}