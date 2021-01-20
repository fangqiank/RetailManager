using System.Net.Http;
using System.Threading.Tasks;
using RMWPFUi.Library.Models;

namespace RMWPFUi.Library.Api
{
    public interface IAPIHelper
    {
        HttpClient ApiClient { get; }
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}