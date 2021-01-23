using System.Collections.Generic;
using System.Threading.Tasks;
using RMWPFUi.Library.Models;

namespace RMWPFUi.Library.Api
{
    public interface IUserEndPoint
    {
        Task<List<UserModel>> GetAll();
    }
}