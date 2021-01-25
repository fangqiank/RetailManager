using System.Collections.Generic;
using RMDataManager.Library.Internal;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class UserData : IUserData
    {
        private readonly ISqlDataAccess _sql;

        public UserData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<UserModel> GetUserById(string id)
        {
            //SqlDataAccess sql = new SqlDataAccess(_configuration);

            var p = new
            {
                Id = id
            };

            var output =_sql.LoadData<UserModel, dynamic>(
                "dbo.spUserLookup", p, "RMData");
            return output;
        }
    }
}
