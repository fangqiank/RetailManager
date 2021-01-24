using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using TRMDataManager.Library.Models;

namespace RMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SaleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize(Roles = "Cashier,Admin")]
        //[Authorize(Users="")]
        [HttpPost]
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData(_configuration);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userId);

        }

        [Authorize(Roles = "Admin")]
        [Route("getsalereport")]
        [HttpGet]
        public List<SaleReportModel> GetSaleReport()
        {
            /*if (RequestContext.Principal.IsInRole("Admin"))
            {
                //Do admin stuff
            }else if (RequestContext.Principal.IsInRole("Manager"))
            {
                //Do manager stuff
            }
            else
            {
                //Do other
            }*/

            SaleData data = new SaleData(_configuration);

            return data.GetSaleReport();
        }
    }
}
