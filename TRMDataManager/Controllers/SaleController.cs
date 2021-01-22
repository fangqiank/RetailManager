using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using RMDataManager.Library.DataAccess;
using System.Web.Http;
using RMDataManager.Library.Models;
using TRMDataManager.Library.Models;

namespace TRMDataManager.Controllers
{
    [Authorize]
    public class SaleController : ApiController
    {
        [Authorize(Roles = "Cashier,Admin")]
        //[Authorize(Users="")]
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();

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
