using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using RMWPFUi.Library.Models;

namespace RMWPFUi.Library.Api
{
    public class UserEndPoint : IUserEndPoint
    {
        private readonly IAPIHelper _apiHelper;

        public UserEndPoint(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        public async Task<List<UserModel>> GetAll()
        {
            using (HttpResponseMessage res = await _apiHelper.ApiClient.GetAsync("/api/user/admin/getalluasers"))
            {
                if (res.IsSuccessStatusCode)
                {
                    var result = await res.Content.ReadAsAsync<List<UserModel>>();
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
