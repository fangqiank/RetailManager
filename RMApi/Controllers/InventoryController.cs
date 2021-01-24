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
        private readonly IConfiguration _configuration;

        public InventoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize(Roles = "Cashier,Admin")]
        public List<InventoryModel> GetSaleReport()
        {
            InventoryData data = new InventoryData(_configuration);

            return data.GetInventory();
        }

        [Authorize(Roles = "Admin,Manager")]
        public void Post(InventoryModel inventory)
        {
            InventoryData data = new InventoryData(_configuration);
            data.SaveInventoryRecord(inventory);
        }
    }
}
