using System.Threading.Tasks;

namespace Webservices.HttpService.Interface
{
    public interface IHttpRequestService
    {

        Task<string> GetStringAsync(string url);

    }
}