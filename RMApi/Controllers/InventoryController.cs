using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;

namespace RMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryData _inventoryData;

        public InventoryController(IInventoryData inventoryData)
        {
            _inventoryData = inventoryData;
        }

        [Authorize(Roles = "Cashier,Admin")]
        [HttpGet]
        public List<InventoryModel> GetSaleReport()
        {
            //InventoryData data = new InventoryData(_configuration);

            return _inventoryData.GetInventory();
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public void Post(InventoryModel inventory)
        {
            //InventoryData data = new InventoryData(_configuration);
            _inventoryData.SaveInventoryRecord(inventory);
        }
    }
}
