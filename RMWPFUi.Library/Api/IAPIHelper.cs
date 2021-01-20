using System.Threading.Tasks;
using RMWPFUi.Library.Models;

namespace RMWPFUi.Library.Api
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
        Task GetLoggedInUserInfo(string token);
    }
}