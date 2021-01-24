using RMDataManager.Library.Internal;
using RMDataManager.Library.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace RMDataManager.Library.DataAccess
{
    public class ProductData
    {
        private readonly IConfiguration _configuration;

        public ProductData(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess(_configuration);

            var output = sql.LoadData<ProductModel, dynamic>(
                "dbo.spProduct_GetAll",new{}, "RMData");

            return output;
        }

        public ProductModel GetProductById(int productId)
        {
            SqlDataAccess sql = new SqlDataAccess(_configuration);

            var output = sql.LoadData<ProductModel, dynamic>(
                "dbo.spProduct_GetById", new { Id = productId }, "RMData")
                .FirstOrDefault();

            return output;
        }
    }
}
