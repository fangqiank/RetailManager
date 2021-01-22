using System.Collections.Generic;
using System.Web.Http;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
    //[Authorize]
    public class InventoryController : ApiController
    {
        public List<InventoryModel> GetSaleReport()
        {
            InventoryData data = new InventoryData();

            return data.GetInventory();
        }

        public void Post(InventoryModel inventory)
        {
            InventoryData data = new InventoryData();
            data.SaveInventoryRecord(inventory);
        }
    }
}
