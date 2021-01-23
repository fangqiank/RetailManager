using System.Collections.Generic;
using System.Threading.Tasks;
using RMWPFUi.Library.Models;

namespace RMWPFUi.Library.Api
{
    public interface IUserEndPoint
    {
        Task<List<UserModel>> GetAll();
        Task<Dictionary<string,string>> GetAllRoles();
        Task AddUserToRole(string userId, string roleName);
        Task RemoveUserToRole(string userId, string roleName);
    }
}