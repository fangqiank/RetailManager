using RMWPFUi.Library.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RMWPFUi.Library.Api
{
    public class ProductEndPoint : IProductEndPoint
    {
        private readonly IAPIHelper _apiHelper;

        public ProductEndPoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<ProductModel>> GetAll()
        {
            using (HttpResponseMessage res = await _apiHelper.ApiClient.GetAsync("/api/product"))
            {
                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsAsync<List<ProductModel>>();
                    return result;
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
            }
        }
    }
}
