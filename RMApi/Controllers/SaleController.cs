using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
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
        [Authorize(Roles = "Cashier,Admin")]
        //[Authorize(Users="")]
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userId);

        }

        [Authorize(Roles = "Admin")]
        [Route("getsalereport")]
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

            SaleData data = new SaleData();

            return data.GetSaleReport();
        }
    }
}
