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
        public void Post(SaleModel sale)
        {
            SaleData data = new SaleData();
            string userId = RequestContext.Principal.Identity.GetUserId();

            data.SaveSale(sale, userId);

        }

        [Route("getsalereport")]
        public List<SaleReportModel> GetSaleReport()
        {
            SaleData data = new SaleData();

            return data.GetSaleReport();
        }
    }
}
