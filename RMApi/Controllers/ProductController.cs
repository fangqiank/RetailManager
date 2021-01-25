using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMDataManager.Library.DataAccess;
using RMDataManager.Library.Models;
using System.Collections.Generic;

namespace RMApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Cashier")]
    public class ProductController : ControllerBase
    {
        private readonly IProductData _data;

        public ProductController(IProductData data)
        {
            _data = data;
        }

        [HttpGet]
        public List<ProductModel> Get()
        {
            //ProductData data = new ProductData(_configuration);
            return _data.GetProducts();
        }
    }
}
