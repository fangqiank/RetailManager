using Microsoft.Extensions.Configuration;
using RMDataManager.Library.Internal;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TRMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class SaleData : ISaleData
    {
        private readonly IProductData _products;
        private readonly ISqlDataAccess _sql;
        private readonly IConfiguration _config;

        public SaleData(IProductData products,ISqlDataAccess sql,IConfiguration config)
        {
            _products = products;
            _sql = sql;
            _config = config;
        }

        public decimal GetTaxRate()
        {
            string rateText = _config.GetValue<string>("TaxRate");  //ConfigurationManager.AppSettings["taxRate"];

            bool IsValidTaxRate = decimal.TryParse(rateText, out decimal output);

            if (IsValidTaxRate == false)
            {
                throw new Exception("The tax rate is not set up properly");
            }

            output = output / 100;

            return output;
        }

        public void SaveSale(SaleModel saleInfo,string cashierId)
        {
            List<SaleDetailDBModel> details =new List<SaleDetailDBModel>();
           // ProductData products = new ProductData(_configuration);
            //var taxRate = ConfigHelper.GetTaxRate()/100;
            var taxRate = GetTaxRate();

            //filling the sale detail
            foreach (var item in saleInfo.SaleDetails)
            {
                var detail =new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = _products.GetProductById(item.ProductId);

                if (productInfo == null)
                {
                    throw new Exception($"The product id of {item.ProductId} could not be found in database.");
                }

                detail.PurchasePrice = (productInfo.RetailPrice * detail.Quantity);
                

                if (productInfo.IsTaxable)
                {
                    detail.Tax = (detail.PurchasePrice * taxRate);
                }

                details.Add(detail);
            }

            //Create the sale model
            SaleDbModel sale = new SaleDbModel
            {
                SubTotal = details.Sum(x=>x.PurchasePrice),
                Tax = details.Sum(x=>x.Tax),
                CashierId = cashierId
            };

            sale.Total = sale.SubTotal + sale.Tax;

            //using (SqlDataAccess sql = new SqlDataAccess(_configuration))
            //{
            try
            {
                _sql.StartTransaction("RMData");

                //save the sale model
                _sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                //get the Id from the sale model
                sale.Id = _sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup",
                    new { CashierId = sale.CashierId, SaleDate = sale.SaleDate }).FirstOrDefault();

                //Finish filling in the sale detail model
                foreach (var item in details)
                {
                    item.SaleId = sale.Id;

                    //save the sale detail model
                    _sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                }
                //sql.CommitTransaction();  //complain no necessary commit again?
            }
            catch(Exception ex)
            {
                _sql.RollbackTransaction();
                throw;
            }
            //}
                
        }

        public List<SaleReportModel> GetSaleReport()
        {
            //SqlDataAccess sql = new SqlDataAccess(_configuration);

            var output = _sql.LoadData<SaleReportModel, dynamic>("dbo.spSale_SaleReport",
                new { }, "RMData");
            return output;
        }
    }
}
