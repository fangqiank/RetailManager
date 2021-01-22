using RMDataManager.Library.Internal;
using RMDataManager.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TRMDataManager.Library.Models;

namespace RMDataManager.Library.DataAccess
{
    public class SaleData
    {
        public void SaveSale(SaleModel saleInfo,string cashierId)
        {
            List<SaleDetailDBModel> details =new List<SaleDetailDBModel>();
            ProductData products = new ProductData();
            var taxRate = ConfigHelper.GetTaxRate()/100;
            
            //filling the sale detail
            foreach (var item in saleInfo.SaleDetails)
            {
                var detail =new SaleDetailDBModel
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                };

                var productInfo = products.GetProductById(item.ProductId);

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

            using (SqlDataAccess sql = new SqlDataAccess())
            {
                try
                {
                    sql.StartTransaction("RMData");

                    //save the sale model
                    sql.SaveDataInTransaction("dbo.spSale_Insert", sale);

                    //get the Id from the sale model
                    sale.Id = sql.LoadDataInTransaction<int, dynamic>("spSale_Lookup",
                        new { CashierId = sale.CashierId, SaleDate = sale.SaleDate }).FirstOrDefault();

                    //Finish filling in the sale detail model
                    foreach (var item in details)
                    {
                        item.SaleId = sale.Id;

                        //save the sale detail model
                        sql.SaveDataInTransaction("dbo.spSaleDetail_Insert", item);
                    }
                    //sql.CommitTransaction();  //complain no necessary commit again?
                }
                catch(Exception ex)
                {
                    sql.RollbackTransaction();
                    throw;
                }
            }
                
        }
    }
}
