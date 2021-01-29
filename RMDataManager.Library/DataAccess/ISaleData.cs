using System.Collections.Generic;
using RMDataManager.Library.Models;
using TRMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public interface ISaleData
    {
        decimal GetTaxRate();
        void SaveSale(SaleModel saleInfo,string cashierId);
        List<SaleReportModel> GetSaleReport();
    }
}