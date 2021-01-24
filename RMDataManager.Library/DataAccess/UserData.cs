using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using RMDataManager.Library.Internal;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class UserData
    {
        private readonly IConfiguration _configuration;

        public UserData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<UserModel> GetUserById(string id)
        {
            SqlDataAccess sql = new SqlDataAccess(_configuration);

            var p = new
            {
                Id = id
            };

            var output =sql.LoadData<UserModel, dynamic>(
                "dbo.spUserLookup", p, "RMData");
            return output;
        }
    }
}
