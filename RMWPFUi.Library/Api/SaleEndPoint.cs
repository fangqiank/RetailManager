using RMWPFUi.Library.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RMWPFUi.Library.Api
{
    public class SaleEndPoint : ISaleEndPoint
    {
        private readonly IAPIHelper _apiHelper;

        public SaleEndPoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task PostSale(SaleModel sale)
        {
            using (HttpResponseMessage res = await _apiHelper.ApiClient.PostAsJsonAsync("/api/sale",sale))
            {
                if (res.IsSuccessStatusCode)
                {
                    //
                }
                else
                {
                    throw new Exception(res.ReasonPhrase);
                }
            }
        }
        
    }
}

