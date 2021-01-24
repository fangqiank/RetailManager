using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using RMDataManager.Library.Internal;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class InventoryData
    {
        private readonly IConfiguration _configuration;

        public InventoryData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<InventoryModel> GetInventory()
        {
            SqlDataAccess sql = new SqlDataAccess(_configuration);

            var output = sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll",
                new { }, "RMData");

            return output;
        }

        public void SaveInventoryRecord(InventoryModel inventory)
        {
            SqlDataAccess sql =new SqlDataAccess(_configuration);
            ;
            sql.SaveData("dbo.spInventory_Insert",inventory,"RMData");
        }
    }
}
