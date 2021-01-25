using RMDataManager.Library.Internal;
using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Linq;

namespace RMDataManager.Library.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly ISqlDataAccess _sql;

        public ProductData(ISqlDataAccess sql)
        {
            _sql = sql;
        }

        public List<ProductModel> GetProducts()
        {
            //SqlDataAccess sql = new SqlDataAccess(_configuration);

            var output = _sql.LoadData<ProductModel, dynamic>(
                "dbo.spProduct_GetAll",new{}, "RMData");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            //SqlDataAccess sql = new SqlDataAccess(_configuration);

            var output = _sql.LoadData<ProductModel, dynamic>(
                "dbo.spProduct_GetById", new { Id = productId }, "RMData")
                .FirstOrDefault();

            return output;
        }
    }
}
