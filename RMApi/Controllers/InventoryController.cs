using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;

namespace RMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        [Authorize(Roles = "Cashier,Admin")]
        public List<InventoryModel> GetSaleReport()
        {
            InventoryData data = new InventoryData();

            return data.GetInventory();
        }

        [Authorize(Roles = "Admin,Manager")]
        public void Post(InventoryModel inventory)
        {
            InventoryData data = new InventoryData();
            data.SaveInventoryRecord(inventory);
        }
    }
}
