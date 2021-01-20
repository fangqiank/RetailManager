using RMDataManager.Library.Internal;
using RMDataManager.Library.Models;
using System.Collections.Generic;

namespace RMDataManager.Library.DataAccess
{
    public class ProductData
    {
        public List<ProductModel> GetProducts()
        {
            SqlDataAccess sql = new SqlDataAccess();

            var output = sql.LoadData<ProductModel, dynamic>(
                "dbo.spProduct_GetAll",new{}, "RMData");

            return output;
        }
    }
}
