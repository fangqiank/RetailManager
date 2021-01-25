using RMDataManager.Library.Internal;
using RMDataManager.Library.Models;
using System.Collections.Generic;

namespace RMDataManager.Library.DataAccess
{

    public class InventoryData : IInventoryData
    {
        private readonly ISqlDataAccess _sql;

        public InventoryData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<InventoryModel> GetInventory()
        {
            //SqlDataAccess sql = new SqlDataAccess(_configuration);

            var output = _sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll",
                new { }, "RMData");

            return output;
        }

        public void SaveInventoryRecord(InventoryModel inventory)
        {
            //SqlDataAccess sql =new SqlDataAccess(_configuration);
            
            _sql.SaveData("dbo.spInventory_Insert",inventory,"RMData");
        }
    }
}
