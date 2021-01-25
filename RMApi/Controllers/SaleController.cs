using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Security.Claims;
using TRMDataManager.Library.Models;

namespace RMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SaleController : ControllerBase
    {
        private readonly ISaleData _saleData;

        public SaleController(ISaleData saleData)
        {
            _saleData = saleData;
        }

        [Authorize(Roles = "Cashier,Admin")]
        //[Authorize(Users="")]
        [HttpPost]
        public void Post(SaleModel sale)
        {
            //SaleData data = new SaleData(_configuration);
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//RequestContext.Principal.Identity.GetUserId();

            _saleData.SaveSale(sale, userId);

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

            //SaleData data = new SaleData(_configuration);

            return _saleData.GetSaleReport();
        }
    }
}
