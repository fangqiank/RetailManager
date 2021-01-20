using System.Threading.Tasks;
using RMWPFUi.Models;

namespace RMWPFUi.Helpers
{
    public interface IAPIHelper
    {
        Task<AuthenticatedUser> Authenticate(string username, string password);
    }
}