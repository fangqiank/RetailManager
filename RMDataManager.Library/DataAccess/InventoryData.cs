using System.Collections.Generic;
using RMDataManager.Library.Internal;
using RMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class InventoryData
    {
        public List<InventoryModel> GetInventory()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<InventoryModel, dynamic>("dbo.spInventory_GetAll",
                new { }, "RMData");

            return output;
        }

        public void SaveInventoryRecord(InventoryModel inventory)
        {
            SqlDataAccess sql =new SqlDataAccess();
            ;
            sql.SaveData("dbo.spInventory_Insert",inventory,"RMData");
        }
    }
}
