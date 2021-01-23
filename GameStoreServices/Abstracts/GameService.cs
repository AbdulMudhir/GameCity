using System.Threading;
using System.Threading.Tasks;

namespace GameStoreServices.Abstracts
{
    public abstract class GameService
    {
        public delegate void UpdateReceivedEventHandler(object source);

        public event UpdateReceivedEventHandler updateReceived;

        public abstract void RunAsync();


        protected void OnUpdateReceived(object source)
        {
            updateReceived?.Invoke(this);
        }
    }


}